﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interactions : MonoBehaviour // this class is designed to pass interactions from the game world to the different AI aspects
{

    //public string enemyTag;
    private Navigation navigation; // create empty Navigation object to assign in start
    private NavMeshAgent navigationAgent; // create empty Navigation agent
    private DecisionMaking decisionMaking; // as above for DecisionMaking
    private CharacterSheet characterSheet;
    //private MeleeAttackEngine meleeAttackEngine;

    public void Awake() // need this to happen before start
    {
        navigation = GetComponent<Navigation>(); // creates a reference to the Navigation component on the game object
        navigationAgent = GetComponent<NavMeshAgent>();
        decisionMaking = GetComponent<DecisionMaking>(); // as above except decision making
        characterSheet = GetComponent<CharacterSheet>();
        
    }

    public void Start()
    {
        decisionMaking.MakeADecision(); // on start make a decision!
    }

    public void EnterAttackZone(Collider other)
    {
        if (other.tag == characterSheet.enemyTag)
        {
            //Debug.Log("Entered Zone"); // writes a message to console
            //navigation.UpdateTarget(decisionMaking.FindSafeZone(), characterSheet.MovementSpeed);
            //decisionMaking.Retreat = true;
            //navigation.UpdateTarget(transform);
        }
    }

    public void InsideAttackZone(Collider other) // if the enemy is within the attack zone
    {
        //Debug.Log("called interaction method");
        if (other.tag == characterSheet.enemyTag)
        {
            decisionMaking.CombatAction(other); // call the combat action AI section
            //Debug.Log("detected enemy tag");
        }
    }

    public void NotFacing(Collider other)
    {
        if (other.tag == characterSheet.enemyTag)
        {
            Debug.Log("Not Facing Enemy");
            
        }
    }

    public void EnteredSafeZone(int _safeZoneHealRate)
    {
        decisionMaking.AtSafeZone = true;
        decisionMaking.Retreat = false;
        Debug.Log("Entered Safe Zone");
        characterSheet.SafeZoneHealing(_safeZoneHealRate);
        
    }

    public void LeftSafeZone()
    {
        decisionMaking.AtSafeZone = false;
        characterSheet.LeavingSafeZone();
        Debug.Log("Left Safe Zone");
    }

    public List<Collider> Listen() // returns an array of all colliders that have the enemies tag within the listening range of the character
    {
        Collider[] inHearingRange;
        inHearingRange = Physics.OverlapSphere(transform.position, characterSheet.HearingRadius);

        List<Collider> detectableEnemy = new List<Collider>(); // any collider within the hearing radius

        foreach (Collider col in inHearingRange)
        {
            if (col.tag==characterSheet.enemyTag) // only adds those with the enemy tag to the list
            {
                if (CalculatePathLength(col.transform.position) <= characterSheet.HearingRadius)
                {
                    detectableEnemy.Add(col);

                    Debug.Log("I hear them!");
                }
            }
        }

        return detectableEnemy;

    }

    public float CalculatePathLength(Vector3 detectablePosition) // how far to walk to the target (i.e. nav mesh path length!)
    {
        NavMeshPath path = new NavMeshPath();
        navigationAgent.CalculatePath(detectablePosition, path); // create path to target

        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2]; // add 2 to include start and finish
        allWayPoints[0] = transform.position; // set start waypoint to the unit position
        allWayPoints[allWayPoints.Length - 1] = detectablePosition; // set last waypoint to the target position

        for (int i = 0; i<path.corners.Length;i++) // add the corners to the array
        {
            allWayPoints[i + 1] = path.corners[i]; // not +1 as index 0 is the transform position
        }

        float pathLength = 0;

        for (int i = 0;i<allWayPoints.Length-1;i++) // distance between each point (-1 length as segments is 1 less than points)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]); // add segment to path length
        }

        return pathLength;

    }

    public List<Collider> Look() // line of sight enemy detection
    {
        Collider[] inSightRange;
        inSightRange = Physics.OverlapSphere(transform.position, characterSheet.SightRadius);

        List<Collider> detectableEnemy = new List<Collider>(); // any collider within the hearing radius

        foreach (Collider col in inSightRange)
        {
            if (col.tag == characterSheet.enemyTag) // only adds those with the enemy tag to the list
            {
                Vector3 directionToTarget = col.transform.position - transform.position; // calculate the direction to the target
                float angleToTarget = Vector3.Angle(transform.forward, directionToTarget); // calculates the angle from forward to the target in degrees
                if (angleToTarget<=characterSheet.PeripheralAngle) // if the target is within the peripheral vision range
                {
                    RaycastHit hit; // gameobject that is hit by the ray
                    if (Physics.Raycast(transform.position+transform.up, directionToTarget.normalized, out hit, characterSheet.SightRadius))
                    {
                        if (hit.transform.tag == characterSheet.enemyTag)
                        {
                            detectableEnemy.Add(col); // add to the collider list that is detectable

                            Debug.Log(transform.tag + "I see them!");
                        }
                    }
                }
            }
        }

        return detectableEnemy;
    }

    public List<GameObject> SenseEnemies() // using all senses return a list of detectable enemies as game objects
    {
        List<GameObject> senseEnemies = new List<GameObject>();
        List<Collider> detectableEnemies = Look(); // creates list of enemies that can be seen
        detectableEnemies.AddRange(Listen());
        if (detectableEnemies!=null)
        {
            foreach (Collider col in detectableEnemies)
            {
                senseEnemies.Add(col.gameObject);
            }
        }
        return senseEnemies;
    }
    
}
