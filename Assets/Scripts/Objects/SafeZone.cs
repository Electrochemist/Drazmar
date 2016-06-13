using UnityEngine;
using System.Collections.Generic;

public class SafeZone : MonoBehaviour {

    public string friend; // sets the tag that will create a trigger response
    
   
    private string enterSafeZone = "EnteredSafeZone"; // name of method to call on entering the safe zone
    private int healRate = 5; // health per second
    

    private string leftSafeZone = "LeftSafeZone"; // name of method to call on leaving the safe zone

	public void OnTriggerEnter(Collider other)
    {
        if (other.tag == friend)
        {
            //other.SendMessage("EnteredSafeZone");

            other.SendMessage(enterSafeZone, healRate);
        }
    }
    

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == friend)
        {
            other.SendMessage(leftSafeZone);
        }
    }

    
}
