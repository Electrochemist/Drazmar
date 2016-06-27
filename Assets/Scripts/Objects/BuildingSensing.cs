using UnityEngine;
using System.Collections.Generic;

public class BuildingSensing : MonoBehaviour {

    public string enemy; // sets the tag that will trigger and enemy response
    public string friendly; // sets the tag for a friendly

    private float threatToStructure; // to implement - based on observed enemies - nearby friendlies
    private int sightRadius = 50; // radius that the building can see


    public List<Collider> BuildingLook()
    {
        Collider[] inSightRange;
        inSightRange = Physics.OverlapSphere(transform.position, sightRadius);

        List<Collider> detectableEnemy = new List<Collider>(); // any collider within the hearing radius
        threatToStructure = 0; // reset threat to structure

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
                        threatToStructure += col.gameObject.GetComponent<CharacterSheet>().Threat; // adds the enemies threat to the buildings danger level
                        Debug.Log(transform.tag + "I see them!");
                    }
                }
            }
            if (col.tag==friendly)
            {
                threatToStructure-= col.gameObject.GetComponent<CharacterSheet>().Threat; // subtract the friendlies threat from the building danger level
            }
        }

        return detectableEnemy;
    }

    public void Update()
    {
        BuildingLook();
        if (threatToStructure>0) // if there is a threat to the building 
        {
            Alarm(threatToStructure); // raise the alarm
        }
    }

    public void Alarm(float _threatToStructure)
    {
        Debug.Log("Alarm Raised!");
    }
}
