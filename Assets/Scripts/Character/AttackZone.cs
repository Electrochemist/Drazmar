using UnityEngine;
using System.Collections;

public class AttackZone : MonoBehaviour {

    public GameObject attacker;

    private string attackString = "EnterAttackZone"; // sets the method to call when using send message later!
    private string facing = "Facing";
    private string notFacing = "NotFacing";
    private string enemy = "Enemy";
    private string insideAttackZone = "InsideAttackZone";

    
    // On Enemy Target Entering Attack Zone
    public void OnTriggerEnter(Collider other)
    {
        //attacker.SendMessage(attackString, other); // calls the method set in attack string from the game object
    }
    public void OnTriggerStay(Collider other) // need to set layer controls here I think to seperate characters from walls and ground etc for performance
    {
        attacker.SendMessage(insideAttackZone, other);
        /*  if (other.tag == "Enemy") // check tag... should paramaterise this - probably set a gameobject tag reference
         {
             attacker.SendMessage(insideAttackZone, other.transform.parent.gameObject);

         }
             float angle = Vector3.Angle(attacker.transform.forward, (other.transform.position-attacker.transform.position)); // angle that the attacker is facing to the collided oject
             float enemyAngle = Vector3.Angle(other.transform.forward, (attacker.transform.position - other.transform.position)); // the angle that the enemy is being attacked at. Note both angles at 0-180
             if (angle < 60) // if the closest point of the object collided with is within 60 degrees of the forward direction
             {
                 Debug.Log(enemyAngle.ToString());
                 attacker.SendMessage(facing, other);
             }
             else
             {
                 Debug.Log(angle.ToString());
                 attacker.SendMessage(notFacing, other);
             }
         }*/
    }
    public void OnTriggerExit(Collider other)
    {

    }
}
