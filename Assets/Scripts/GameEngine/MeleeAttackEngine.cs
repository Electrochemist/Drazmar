using UnityEngine;
using System.Collections;

public class MeleeAttackEngine : MonoBehaviour {

    public static MeleeAttackEngine meleeAttackEngine; // static instance holder
    //private Random seed; // random seed holder
    
    public void Awake() // called once before start
    {
        if (meleeAttackEngine!=null) // singleton format if there is an instance already destroy this object, else this is the instance
        {
            Destroy(this);
        }
        else
        {
            meleeAttackEngine = this;
            //seed = new System.Random(); // create the seed!
        }
    }

    public bool HitAttempt(int _baseAccuracy, float _attackAccuracyMod, float _targetOrientationMod) // multiplies the hit chances, sets max chance to 99, random number hit determination
    {
        float hitChance = _baseAccuracy * _attackAccuracyMod * _targetOrientationMod;
        
        if (hitChance > 99) // fix max hit rate to 99%
        {
            hitChance = 99;
        }


        if (Random.value * 100 > hitChance) // unity random is only between 0 and 1
        {
            return false;
        }

        else
        {
            return true;
        }

    }

    public void DamageTarget(CharacterSheet attackerCharacterSheet, CharacterSheet targetCharacterSheet, float targetAngleBonusDamage, float targetBlockModifier)
    {
        int baseDamage = (int)(Random.value * attackerCharacterSheet.AttackDamageRange); // unity random 0 to 1, times the variable range of the attacker damage
        int damage = (int)(((attackerCharacterSheet.AttackDamageMin + baseDamage) * targetAngleBonusDamage*targetCharacterSheet.DamageDecrease)-targetCharacterSheet.DamageMitigate); // attack damage calculator - min + the random range addition, times attack zone, times damage decrease, - damage mitigated. - note mitigated damage is after decrease
        Debug.Log("Hit damage = " + damage.ToString());
        if (damage < 1) { return; }
        else if (BlockAttempt(targetCharacterSheet.BlockChance*targetBlockModifier)) // try to block
        {
            damage = BlockDamage(damage, targetCharacterSheet); // if block works decrease damage by blocked amount
            Debug.Log("Damage after block = " + damage.ToString());
        }
        
        if (damage>0)
        {
            targetCharacterSheet.ReduceHitPoints(damage);
        }
    }

    
    public bool BlockAttempt(float blockChance) // does the target block?
    {
        float blockRoll = Random.value*100;
        if (blockRoll < 95 && blockRoll <= blockChance) // if random number is less than or equal to block chance
        {
            return true;
        }
        else { return false; }
    }

    public int BlockDamage(int damage, CharacterSheet target) // reduces the damage by the amount blocked
    {
        int reducedDamage = damage - (int)target.BlockChargeCurrent;
        target.ReduceBlockChargeCurrent(damage);
        return reducedDamage;
    }

}
