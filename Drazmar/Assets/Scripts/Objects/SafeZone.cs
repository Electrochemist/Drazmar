using UnityEngine;
using System.Collections;

public class SafeZone : MonoBehaviour {

    public string friend = "Friendly"; // sets the tag that will create a trigger response

	public void OnTriggerEnter(Collider other)
    {
        if (other.tag == friend)
        {
            other.SendMessage("EnteredSafeZone");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == friend)
        {
            other.SendMessage("LeftSafeZone");
        }
    }
}
