using UnityEngine;
using System.Collections.Generic;
using System;


public class DecisionMaking : MonoBehaviour {

    private CharacterSheet characterSheet; // character sheet to be called from game object
    private Navigation navigation;
    private Interactions interactions;
    private bool retreat=false; // if the character is fleeing
    private string role; // decides what decision tree to use
    public bool Retreat // accessor for retreat bool
    {
        get
        { return retreat; }
        set
        { retreat = value; }
    }
        
    private bool atSafeZone;
    public bool AtSafeZone
    {
        get { return atSafeZone; }
        set { atSafeZone = value; }
    }

    private bool onPatrol; // bool to show that the unit is on patrol
    public bool OnPatrol
    {
        get { return onPatrol; }
        set { onPatrol = value; }
    }

    private bool inCombat; // bool to show that the unit is in combat
    public bool InCombat
    {
        get { return inCombat; }
        set { inCombat = value; }
    }

    private bool healing; // bool to show that the unit is healing
    public bool Healing
    {
        get { return healing; }
        set { healing = value; }
    }

    private bool respondToAlarm; // responding to an alarm
    public bool RespondToAlarm
    { get { return respondToAlarm; } }


    // START OF METHODS

    public void Awake()
    {
        characterSheet = GetComponent<CharacterSheet>();
        navigation = GetComponent<Navigation>();
        interactions = GetComponent<Interactions>();
        role = "Defend";
    }

    public void Update()
    {
        if (onPatrol) // check to see if the patrolling unit is close to destination position, if so, get new waypoint
        {
            Vector3 distanceToPatrolWayPoint = transform.position - navigation.Target.position;
            if (distanceToPatrolWayPoint.sqrMagnitude<10)
            {
                Patrol();
            }
        }
    }

    public Transform FindSafeZone() // finds the closest safezone out of all safezones in existance
    {
        Transform safeZone=null; // create empty transform reference
        GameObject[] allSafeZones; // create list of all safe zones - note this is all map so consider ways to reduce area for searching enemy lists or for big map
        allSafeZones = GameObject.FindGameObjectsWithTag(characterSheet.safeZoneTag); // fill the list with all game objects tagged friend zone - perhaps update method to take a string for safe zone
        Vector3 currentPosition = transform.position; // set a vector3 for where the game object is

        float bestDistanceSqr = Mathf.Infinity; // set the initial best distance to infinity

        foreach (GameObject zone in allSafeZones)
        {
            Vector3 distance = zone.transform.position - currentPosition; // create a distance from zone vector
            float distanceSqr = distance.sqrMagnitude; // square magnitude of distance

            if (distanceSqr<bestDistanceSqr) // if square distance smaller than previous best, update distance and transform.
            {
                bestDistanceSqr = distanceSqr;
                safeZone = zone.transform;
            }
        }

        return safeZone;
    }

    public void MoveToHealing() // move unit sensibly to the nearest healing position
    {
        navigation.UpdateTarget(FindSafeZone(), characterSheet.MovementSpeed);
    }

    public Transform FindEnemy(List<GameObject> senseEnemies) // finds the closest enemy in that can be sensed
    {
        Transform enemy = null; // create empty transform reference
        List<GameObject> enemies = senseEnemies; // assigns the detected enemies to be searched
               
        Vector3 currentPosition = transform.position; // set a vector3 for where the game object is

        float bestDistanceSqr = Mathf.Infinity; // set the initial best distance to infinity

        foreach (GameObject target in enemies)
        {
            Vector3 distance = target.transform.position - currentPosition; // create a distance from enemy vector
            float distanceSqr = distance.sqrMagnitude; // square magnitude of distance

            if (distanceSqr < bestDistanceSqr) // if square distance smaller than previous best, update distance and transform.
            {
                bestDistanceSqr = distanceSqr;
                enemy = target.transform;
            }
        }

        return enemy;
    }

    public void CombatAction(Collider enemy) // attack decision - basic attack only
    {
        float angle = Vector3.Angle(transform.forward, (enemy.transform.position - transform.position)); // angle that the attacker (this object) is facing to the collided oject
        float enemyAngle = Vector3.Angle(enemy.transform.forward, (transform.position - enemy.transform.position)); // the angle that the enemy is being attacked at. Note both angles at 0-180
        float enemyAngleBonus; // set up temp variable for the bonus vs the direction enemy is facing
        float enemyAngleBonusDamage;
        float enemyBlockModifier;

        CharacterSheet enemyCharacterSheet = enemy.gameObject.GetComponentInParent<CharacterSheet>();//enemy.transform.GetComponent<CharacterSheet>(); // this is one way, but might be slow

        //Debug.Log(angle.ToString());
        //Debug.Log(enemyCharacterSheet.PeripheralAngle.ToString());

        if (angle < characterSheet.ForwardAngle) // check if attacker is facing the target
        {
            if (characterSheet.AttackTimeToNext <= 0) // if the attacker can attack
            {
                characterSheet.OnAttack();
                if (enemyAngle < enemyCharacterSheet.ForwardAngle) // is the enemy hit in the front
                {
                    Debug.Log("Hit Front");
                    enemyAngleBonus = enemyCharacterSheet.AccuracyHitInFront;
                    enemyAngleBonusDamage = enemyCharacterSheet.DamageHitInFront;
                    enemyBlockModifier = enemyCharacterSheet.BlockModifierFront;
                }
                else if (enemyAngle >= enemyCharacterSheet.ForwardAngle && enemyAngle < enemyCharacterSheet.PeripheralAngle) // the periperal
                {
                    Debug.Log("Hit Periph");
                    enemyAngleBonus = enemyCharacterSheet.AccuracyHitInPeripheral;
                    enemyAngleBonusDamage = enemyCharacterSheet.DamageHitInPeripheral;
                    enemyBlockModifier = enemyCharacterSheet.BlockModifierPeripheral;
                }
                else // or the back
                {
                    Debug.Log("Hit Back");
                    enemyAngleBonus = enemyCharacterSheet.AccuracyHitInBack;
                    enemyAngleBonusDamage = enemyCharacterSheet.DamageHitInBack;
                    enemyBlockModifier = enemyCharacterSheet.BlockModifierBack;
                }

                if (MeleeAttackEngine.meleeAttackEngine.HitAttempt(characterSheet.AttackAccuracy, 1f, enemyAngleBonus)) // if the hit attempt is successful
                {
                    Debug.Log("Hit the enemy!");
                    MeleeAttackEngine.meleeAttackEngine.DamageTarget(characterSheet, enemyCharacterSheet, enemyAngleBonusDamage, enemyBlockModifier);
                    if (enemyCharacterSheet.HitPointsCurrent <= 0) // killed enemy
                    {
                        // perhaps add in a look around response to killing an enemy
                        inCombat = false; // no longer in combat
                    }
                }
                else
                {
                    Debug.Log("Missed the enemy");
                }
            }
        }
    }
    public void MoraleCheck(int damage) // checks how hard the unit has been hit, and triggers a run away based on max life percentage
    {
        float damagePercentage = damage / characterSheet.HitPointsMax;
        if (UnityEngine.Random.value<damagePercentage) // Note: UnityEngine.Random used as system also has a random. If the random roll is lower than the damage percent run safe
        {
            SetAllMindStatesFalse();
            retreat = true;
            navigation.UpdateTarget(FindSafeZone(), characterSheet.FleeIncrease*characterSheet.MovementSpeed);
        }
    }

    public List<GameObject> GetBuildingsList()
    {
        List<GameObject> buildings = new List<GameObject>();

        if (characterSheet.safeZoneTag == "FriendZone") // if friendly unit
        {
            buildings = FriendlyBuildingList.friendlyBuildingList.ListOfFriendlyBuildings; // take list of friendly buildings
        }
        if (characterSheet.safeZoneTag == "EnemyZone") // if enemy unit
        {
            buildings = EnemyBuildingList.enemyBuildingList.ListOfEnemyBuildings; // take list of enemy buildings
        }
        return buildings;
    }

    public void Patrol() // search building list and pick a random building to walk to
    {
        List<GameObject> buildings = new List<GameObject>();
        buildings = GetBuildingsList(); // call method to select friendly or enemy building list
        SetAllMindStatesFalse();
        onPatrol = true;
        float randomIndex = (UnityEngine.Random.value * (buildings.Count));
        if (randomIndex== buildings.Count) // stops a random value of 1 from giving an index out of range
        {
            randomIndex--;
        }
        navigation.UpdateTarget(buildings[(int)randomIndex].transform, characterSheet.MovementSpeed); // convert float to int
    }

    public List<GameObject> CheckForAlarms()
    {
        List<GameObject> buildings = new List<GameObject>();
        buildings = GetBuildingsList(); // call method to select friendly or enemy building list

        List<GameObject> alarmBuildings = new List<GameObject>();

        foreach (GameObject building in buildings)
        {
            if (building.GetComponent<BuildingSensing>().ThreatToStructure <= 0) // if there is no structural threat
            {
                alarmBuildings.Add(building); // remove the building from the list
            }
        }
        return alarmBuildings;
    }

    public void MoveToNearestAlarm(List<GameObject> buildings)
    {
        float bestDistanceSqr = Mathf.Infinity; // prepare to find closest alarm

        GameObject closestAlarm = new GameObject();
        Vector3 currentPosition = transform.position;

        foreach (GameObject alarm in buildings)
        {
            Vector3 distance = alarm.transform.position - currentPosition; // create a distance from alarm gameobject
            float distanceSqr = distance.sqrMagnitude; // square magnitude of distance

            if (distanceSqr < bestDistanceSqr) // if square distance smaller than previous best, update distance and transform.
            {
                bestDistanceSqr = distanceSqr;
                closestAlarm = alarm;
            }
        }
        SetAllMindStatesFalse();
        respondToAlarm = true;
        navigation.UpdateTarget(closestAlarm.transform, characterSheet.MovementSpeed);
    }

    

    public void PatrolDetect(List<Collider> senseEnemies) // unit is on patrol and detects enemy
    {
        List<GameObject> senseEnemiesGameObjects = new List<GameObject>(); // convert the colliders to the game object they are attached to
        foreach (Collider col in senseEnemies)
        {
            senseEnemiesGameObjects.Add(col.gameObject);
        }
        AttackEnemy(senseEnemiesGameObjects); // attack the detected enemy
    }

    public void AttackEnemy(List<GameObject> senseEnemies) // basic attack nearest enemy that is detected (this could be look and/or listen)
    {
        SetAllMindStatesFalse(); // turn off all mind states
        inCombat = true; // mind state for fighting
        navigation.UpdateTarget(FindEnemy(senseEnemies), characterSheet.MovementSpeed);
        return;
    }

    // Mind state management

    public void SetAllMindStatesFalse()
    {
        onPatrol = false;
        inCombat = false;
        retreat = false;
        respondToAlarm = false;
        healing = false;
    }

    // AI tree methods

    public void MakeADecision() // this method is used to cycle through decision trees to pick what action the unit should perform
    {
        Debug.Log("Making a decision");
        onPatrol = false; // reset patrol flag to false
        if (retreat) // if morale is broken the unit is fleeing home and cannot make another decision!
        {
            return;
        }
        if (atSafeZone&&characterSheet.HitPointsCurrent<characterSheet.HitPointsMax) // if healing at a safe zone wait to be fully healed
        {
            healing = true; // set healing mind state
            return;
        }
        healing = false; // fully healed so reset healing mind state
        switch (role)
        {
            case "Defend":
                {
                    DefendTree();
                    return;
                }
            default:
                {
                    Debug.Log("No role set in DecisionMaking.cs");
                    break;
                }
        }
    }

    public void DefendTree()
    {
        Debug.Log("Making Defense Decision");
        if (characterSheet.HitPointsCurrent < 0.5*characterSheet.HitPointsMax) // if less than 50% health, go to healing
        {
            healing = true; // set mind set
            MoveToHealing(); // move to healing zone
            return;
        }
        List<GameObject> senseEnemies = interactions.SenseEnemies(); // enemies that the unit can detect
        if (senseEnemies.Count!=0) // if the unit can detect an enemy, move to the closest enemy
        {
            Debug.Log("Sensed Enemy");
            AttackEnemy(senseEnemies);
            // create an alert
        }
        // if alarm
        // if alert
        else // patrol
        {
            onPatrol = true; // mind set for patrolling
            Patrol();
            return;
        }

    }
}
