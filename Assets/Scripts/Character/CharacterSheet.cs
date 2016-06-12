using UnityEngine;
using System.Collections;

public class CharacterSheet : MonoBehaviour // This will hold all the character stats, and information - Note values done as ints for speed so will be rounding - will probably have sub classes at a later point
{
    public CharacterSheet characterSheet; // place holder for the character sheet instance on this gameobject
    private DecisionMaking decisionMaking; // same to cross talk to decision making script

    // Attack properties
    private int attackChargeMax; // charge maximium
    private float attackChargeCurrent; // charge current - float due to time step multiplication
    private int attackRechargeRate; // how quickly it recovers
    private int attackSpeed; // attack action rate
    private float attackTimeToNext; // time to be able to attack again
    //private int attackDamageMax; // attack damage maximum -- probably don't need then
    private int attackDamageMin; // attack damage minimum
    private int attackDamageRange; // attack damage range
    private int attackAccuracy; // likelyhood of attack to hit

    public string enemyTag;
    public string safeZoneTag;

    public int AttackAccuracy
    {
        get { return attackAccuracy; }
        set { attackAccuracy = value; }
    }
    public int AttackDamageMin
    {
        get { return attackAccuracy; }
        set { attackAccuracy = value; }
    }
    /*public int AttackDamageMax
    {
        get { return attackAccuracy; }
        set { attackAccuracy = value; }
    }*/
    public int AttackDamageRange
    {
        get { return attackDamageRange; }
        set { attackDamageRange = value; }
    }

    public float AttackTimeToNext
    {
        get { return attackTimeToNext; }
        //set { attackTimeToNext = attackSpeed; }
    }
    public void OnAttack()
    {
        attackTimeToNext = attackSpeed;
    }

    // Defense
    private int blockChance; // chance to block
    private int blockChargeMax; // maximum block charge
    private float blockChargeCurrent; // current block charge
    private int blockRechargeRate; // recharge rate of blocking
    private float blockModifierFront; // modifier for blocking at the front
    private float blockModifierPeripheral; // peripheral
    private float blockModifierBack; // back
    private int damageMitigate; // flat damage reduction
    private float damageDecrease; // percentage damage decrease
    private int evasion; // difficulty of target to hit - not sure if I want to implement this though (character move speed could be a better way)
    private float accuracyHitInFront; // enemies bonus to hit in the front zone
    private float accuracyHitInPeripheral; // enemies bonus to hit in the peripheral zone
    private float accuracyHitInBack; // enemies bonus to hit in the back
    private float damageHitInFront; // enemies bonus to damage the front
    private float damageHitInPeripheral; // enemies bonus to damage the peripheral
    private float damageHitInBack; // enemies bonus to damage the back
    private bool inSafeZone; // is the unit within a safe zone
    private int safeZoneHealRate; // healing rate of the safe zone

    public float AccuracyHitInFront
    {
        get { return accuracyHitInFront; }
        set { accuracyHitInFront = value; }
    }
    public float AccuracyHitInPeripheral
    {
        get { return accuracyHitInPeripheral; }
        set { accuracyHitInPeripheral = value; }
    }
    public float AccuracyHitInBack
    {
        get { return accuracyHitInBack; }
        set { accuracyHitInBack = value; }
    }
    public float DamageHitInFront
    {
        get { return damageHitInFront; }
        set { damageHitInFront = value; }
    }
    public float DamageHitInPeripheral
    {
        get { return damageHitInPeripheral; }
        set { damageHitInPeripheral = value; }
    }
    public float DamageHitInBack
    {
        get { return damageHitInBack; }
        set { damageHitInBack = value; }
    }
    public float DamageDecrease
    {
        get { return damageDecrease; }
        set { damageDecrease = value; }
    }
    public int DamageMitigate
    {
        get { return damageMitigate; }
        set { damageMitigate = value; }
    }
    public int BlockChance
    {
        get { return blockChance; }
        set { blockChance = value; }
    }
    public float BlockChargeCurrent
    {
        get { return blockChargeCurrent; }
        set { blockChargeCurrent = value; }
    }
    public float BlockModifierFront
    {
        get { return blockModifierFront; }
        set { blockModifierFront = value; }
    }
    public float BlockModifierPeripheral
    {
        get { return blockModifierPeripheral; }
        set { blockModifierPeripheral = value; }
    }
    public float BlockModifierBack
    {
        get { return blockModifierBack; }
        set { blockModifierBack = value; }
    }

    // Character Zones
    private int forwardAngle; // angle in degrees that the character can perform forward actions within
    private int peripheralAngle; // angle in degrees that the character can perform perihpheral action within
    // private int backAngle; - probably not needed as this will be greater than perihperal but less than 180;
    private int movementSpeed; // how fast the charcter moves
    private int rotationSpeed; // how fast the character can turn

    public int ForwardAngle
    {
        get { return forwardAngle; }
        set { forwardAngle = value; }
    }
    public int PeripheralAngle
    {
        get { return peripheralAngle; }
        set { peripheralAngle = value; }
    }

    // Character Life
    private int hitPointsMax;
    private float hitPointsCurrent;
    private int hitPointsRegenerate;

    public int HitPointsMax
    {
        get { return hitPointsMax; }
    }

    public CharacterSheet() // constructer called on awake()
    {
        attackChargeMax = 0;
        attackChargeCurrent = 0;
        attackRechargeRate = 0;
        attackSpeed = 2;
        attackTimeToNext = 0;
        //attackDamageMax = 50;
        attackDamageMin = 25;
        attackDamageRange = 25;
        attackAccuracy = 50;

        blockChance = 80;
        blockChargeMax = 50;
        blockChargeCurrent = blockChargeMax;
        blockRechargeRate = 5;
        blockModifierFront = 1f; 
        blockModifierPeripheral = 0.5f;
        blockModifierBack = 0f;

        damageMitigate = 0;
        damageDecrease = 1f;
        evasion = 0;

        accuracyHitInFront = 1f;
        accuracyHitInPeripheral = 1.25f;
        accuracyHitInBack = 1.5f;
        damageHitInFront = 1f;
        damageHitInPeripheral = 1.25f;
        damageHitInBack = 1.5f;

        forwardAngle = 60;
        peripheralAngle = 110;
        movementSpeed = 0;
        rotationSpeed = 0;

        hitPointsMax = 100;
        hitPointsCurrent = hitPointsMax;
        hitPointsRegenerate = 0;



    }

    public void Awake()
    {
        characterSheet = GetComponent<CharacterSheet>(); //CharacterSheet(); // set the contents of the character sheet to the constructor values
        decisionMaking = GetComponent<DecisionMaking>(); // reference to decision making script
    }

    public void Update() // every frame
    {
        Recharging(); // recharge the character properties
    }

    private void Recharging() // recharges the players consumable attributes
    {
        if (attackChargeCurrent<attackChargeMax) // if the attack charge is not full (or higher)
        {
            attackChargeCurrent += attackRechargeRate * Time.deltaTime; // add amount recharged
        }
        if (attackTimeToNext > 0) // if the time to next attack is greater than 0
        {
            attackTimeToNext -= Time.deltaTime; // time to next attack decreases
        }
        if (blockChargeCurrent < blockChargeMax) // if the attack charge is not full (or higher)
        {
            blockChargeCurrent += blockRechargeRate * Time.deltaTime; // recharge blocking
        }
        if (hitPointsCurrent<hitPointsMax) // probably add in a regenerate condition like a bool to check if world conditions allow or have this as a rare/weak
        {
            hitPointsCurrent += hitPointsRegenerate * Time.deltaTime; // regenerate health
            if (inSafeZone)
            {
                hitPointsCurrent += safeZoneHealRate * Time.captureFramerate; // safe zone healing
                Debug.Log("safe zone healing");
            }
        }

    }

    // methods affecting Hit Points
    public void ReduceHitPoints(int damage)
    {
        hitPointsCurrent -= damage;
        Debug.Log(hitPointsCurrent.ToString());
        if (hitPointsCurrent<=0)
        {
            Destroy(this.gameObject);
        }
        decisionMaking.MoraleCheck(damage);
    }
    public void IncreaseHitPoints(float heal) // regular heal to maximum hit points
    {
        if (hitPointsCurrent < hitPointsMax) // if hitpoints are less than max (protects from super heal reset!)
        {
            hitPointsCurrent += heal;
            if (hitPointsCurrent>hitPointsMax)
            {
                hitPointsCurrent = hitPointsMax;
            }
        }
    }
    public void OverHeal(float overHeal, float overHealCap) // over heal to increase hit points past normal max
    {
        if (hitPointsCurrent < overHealCap*hitPointsMax)
        {
            hitPointsCurrent += overHeal;
            if (hitPointsCurrent > overHealCap*hitPointsMax)
            {
                hitPointsCurrent = overHealCap*hitPointsMax;
            }
        }
    }
    
    // Methods affecting blocks
    public void ReduceBlockChargeCurrent(int blockChargeDecrease) // decrease the block charge
    {
        blockChargeCurrent -= blockChargeDecrease;
        if (blockChargeCurrent<0) // if the block charge would be reduced below 0, reset it to 0.
        {
            blockChargeCurrent = 0;
        }
    }

    public void SafeZoneHealing(int _safeZoneHealRate)
    {
        inSafeZone = true; // update that the unit is within a safe zone
        safeZoneHealRate = _safeZoneHealRate; // update the rate that the unit will heal by
    }
    public void LeavingSafeZone()
    {
        inSafeZone = false; // update unit so they do not heal as they have left the safe zone
    }
}
