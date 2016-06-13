using UnityEngine;
using System.Collections.Generic;

public class BuildingSensing : MonoBehaviour {

    public string enemy; // sets the tag that will trigger and enemy response

    private float threatToStructure; // to implement - based on observed enemies - nearby friendlies
    private int sightRadius = 50; // radius that the building can see

    public List<Collider> BuildingLook()
    {
        Collider[] inSightRange;
        inSightRange = Physics.OverlapSphere(transform.position, sightRadius);

        List<Collider> detectableEnemy = new List<Collider>(); // any collider within the hearing radius

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

                        Debug.Log(transform.tag + "I see them!");
                    }
                }
            }
        }

        return detectableEnemy;
    }

    public void Update()
    {
        BuildingLook();
    }
}
