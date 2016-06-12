using UnityEngine;
using System.Collections;

public class Interactions : MonoBehaviour // this class is designed to pass interactions from the game world to the different AI aspects
{

    //public string enemyTag;
    private Navigation navigation; // create empty Navigation object to assign in start
    private DecisionMaking decisionMaking; // as above for DecisionMaking
    private CharacterSheet characterSheet;
    //private MeleeAttackEngine meleeAttackEngine;

    public void Awake() // need this to happen before start
    {
        navigation = GetComponent<Navigation>(); // creates a reference to the Navigation component on the game object
        decisionMaking = GetComponent<DecisionMaking>(); // as above except decision making
        characterSheet = GetComponent<CharacterSheet>();
        navigation.UpdateTarget(decisionMaking.FindEnemy()); // on creation find an enemy to attack - replace this with a what should I do call in decision making
    }


    public void EnterAttackZone(Collider other)
    {
        if (other.tag == characterSheet.enemyTag)
        {
            Debug.Log("Entered Zone"); // writes a message to console
            navigation.UpdateTarget(decisionMaking.FindSafeZone());
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

    public void EnteredSafeZone()
    {
        decisionMaking.AtSafeZone = true;
        Debug.Log("Entered Safe Zone");
        navigation.UpdateTarget(decisionMaking.FindEnemy());
    }

    public void LeftSafeZone()
    {
        decisionMaking.AtSafeZone = false;
        Debug.Log("Left Safe Zone");
    }

}
