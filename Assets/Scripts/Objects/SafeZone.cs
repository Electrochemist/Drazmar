using UnityEngine;
using System.Collections.Generic;

public class SafeZone : MonoBehaviour {

    public string friend; // sets the tag that will create a trigger response
    
    private string enterSafeZone = "EnteredSafeZone"; // name of method to call on entering the safe zone
    private int healRate = 10; // health per second

    private SphereCollider healingCollider; // place holder for sphere collider
    public float healingRadius; // place holder for radius. These are set in awake

    private string leftSafeZone = "LeftSafeZone"; // name of method to call on leaving the safe zone

    private List<Transform> restPoint = new List<Transform>();
    public List<Transform> RestPointList
    {
        get { return restPoint; }
    }

    public void Awake()
    {
        healingCollider = GetComponent<SphereCollider>();
        healingRadius = healingCollider.radius;
    }

    public void Start()
    {
        restPoint = CreateRestingPlaceList();

    }

	public void OnTriggerEnter(Collider other)
    {
        if (other.tag == friend)
        {
            //other.SendMessage("EnteredSafeZone");
            //unitsAtRest++; // increase the number of units at the safezone
            other.SendMessage(enterSafeZone, healRate);
        }
    }
    

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == friend)
        {
            //unitsAtRest--; // decrease the number of units at the safezone
            other.SendMessage(leftSafeZone);
        }
    }
    
    public List<Transform> CreateRestingPlaceList()
    {
        Collider[] collidersWithinHealingZone;
        collidersWithinHealingZone = Physics.OverlapSphere(transform.position, healingRadius); // generate array of all colliders within healing range

        List<Transform> restingPlaces = new List<Transform>();

        foreach(Collider col in collidersWithinHealingZone) // check colliders within the healing zone
        {
            if (col.tag=="RestPoint") // if labelled a restPoint
            {
                restingPlaces.Add(col.transform); // add to the restingPlaces list
            }
        }

        return restingPlaces;
    }
      
}
