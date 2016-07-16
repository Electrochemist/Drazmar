using UnityEngine;
using System.Collections;

public class RestPointScript : MonoBehaviour {

    private bool occupied; // is a anyone using this rest point
    public bool Occupied
    {
        get { return occupied; }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Ground" && other.tag != "Untagged")
        {
            occupied = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag != "Ground" && other.tag != "Untagged")
        {
            occupied = false;
        }
    }
}
