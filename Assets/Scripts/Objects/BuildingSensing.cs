using UnityEngine;
using System.Collections.Generic;

public class BuildingSensing : MonoBehaviour {

    public string enemy; // sets the tag that will trigger and enemy response
    public string friendly; // sets the tag for a friendly

    private float totalThreat; // total enemies observed
    private float totalSupport; // total allies nearby
    private float threatToStructure; // based on observed enemies - nearby friendlies
    private int sightRadius = 50; // radius that the building can see

    public float TotalThreat
    {
        get { return totalThreat; }
    }
    public float TotalSupport
    {
        get { return totalSupport; }
    }
    public float ThreatToStructure
    {
        get { return threatToStructure; }
    }

    List<Collider> detectedEnemies = new List<Collider>();
    List<Collider> detectedFriendlies = new List<Collider>();

    public List<Collider> DetectedEnemies
    { get { return detectedEnemies; } }
    public List<Collider> DetectedFriendlies
    { get { return detectedFriendlies; } }

    public void BuildingDetectEnemy()
    {
        Collider[] inSightRange;
        inSightRange = Physics.OverlapSphere(transform.position, sightRadius);

        List<Collider> detectableEnemy = new List<Collider>(); // any collider within the hearing radius
        totalThreat = 0; // reset threat calculations to zero

        foreach (Collider col in inSightRange)
        {
            if (col.tag == enemy) // only adds those with the enemy tag to the list
            {
                Vector3 directionToTarget = col.transform.position - transform.position; // calculate the direction to the target

                RaycastHit hit; // gameobject that is hit by the ray
                if (Physics.Raycast(transform.position + transform.up, directionToTarget.normalized, out hit, sightRadius))
                {
                    if (hit.transform.tag == enemy)
                    {
                        detectableEnemy.Add(col); // add to the collider list that is detectable
                        totalThreat += col.gameObject.GetComponent<CharacterSheet>().Threat; // adds the enemies threat to the buildings danger level

                    }
                }
            }
        }
        detectedEnemies = detectableEnemy;
    }

    public void BuildingDetectFriendly()
    {
        totalSupport = 0;
        Collider[] inSightRange;
        List<Collider> detectableFriend = new List<Collider>();
        inSightRange = Physics.OverlapSphere(transform.position, sightRadius);
        foreach (Collider col in inSightRange)
        {
            if (col.tag == friendly)
            {
                totalSupport += col.gameObject.GetComponent<CharacterSheet>().Threat;
                detectableFriend.Add(col);
            }
        }
        detectedFriendlies = detectableFriend;
    }

    private void UpdateThreatToStructure()
    {
        threatToStructure = totalThreat - totalSupport;
    }

    public List<GameObject> BuildingLookGameObject() // returns the sensed enemies as a list of game objects
    {
        List<GameObject> sensedEnemiesGameObject = new List<GameObject>();
        foreach(Collider enemy in detectedEnemies)
        {
            sensedEnemiesGameObject.Add(enemy.gameObject);
        }
        
        return sensedEnemiesGameObject;
    }

    public void Update()
    {
        BuildingDetectEnemy();
        BuildingDetectFriendly();
        UpdateThreatToStructure();
    }

    /*public void Alarm()
    {
        GameObject[] friendlyList = GameObject.FindGameObjectsWithTag(friendly); // creates array of game objects



    }*/
}
