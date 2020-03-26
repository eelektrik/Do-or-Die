using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //private int health;
    //foraudio
    public int health;

    public int GetHealth() { return health; }
    //Current player health                                     

    //Projectile Stats
    [SerializeField] protected bool Assist;                                 public bool GetAssist() { return Assist; }
    [SerializeField] protected float AssistAngleThreshold;                  public float GetAssistAngle() { return AssistAngleThreshold; }
    [SerializeField] protected float AimAssistMaxDistance;                  public float GetAssistDistance() { return AimAssistMaxDistance; }
    [SerializeField] protected float dice_spread_variance;                  public float GetDiceSpreadVariance() { return dice_spread_variance; }
    [SerializeField] protected float dice_spread_angle;                     public float GetDiceSpreadAngle() { return dice_spread_angle; }
    [SerializeField] protected float projectile_life;                       public float GetProjectileLife() { return projectile_life; }
    [SerializeField] protected float projectile_player_speed_inheritance;   public float GetInheritance() { return projectile_player_speed_inheritance; }

    [SerializeField] protected float shotSpeed;         //Force that projectiles are fired at
    public float GetShotSpeed() { return shotSpeed; }
    public void UpgradeShotSpeed(float increase) { shotSpeed += increase; }

    [SerializeField] protected float shotMinSpeed;      //minimum force used when projectile is within range
    public float GetShotMinSpeed() { return shotMinSpeed; }

    [SerializeField] protected float projectileRange;   //Range that projectile will stay at minspeed
    public float GetProjectileRange() { return projectileRange; }

    [SerializeField] protected float shotKnockback = 1; //Force of knockback from shots against enemies
    public float GetShotKnockback() { return shotKnockback; }
    public void UpgradeShotKnockback(float increase) { shotKnockback += increase; }
    [SerializeField] protected float shotSize = 1;                          public float GetShotSize() { return shotSize; }
    public void UpgradeShotSize(float increase) { shotSize += increase; }

    [SerializeField] protected List<int> dice_list = new List<int>(); //Stores list of dice values in projectile attack
    public List<int> GetDice() { return dice_list; }
    public void AddDie(int sides) { dice_list.Add(sides); }



    [SerializeField] protected float bonusDamage = 0;   //Flat damage bonus added to all attacks
    public float GetBonusDamage() { return bonusDamage; }
    public void UpgradeBonusDamage(float increase) { bonusDamage += increase; }
    
    

    [SerializeField] protected float jumpForce; //Vertical force for jump movement ability
    public float GetJumpForce() { return jumpForce; }                       public void UpgradeJump(float increase) { jumpForce += increase; }




    [SerializeField] protected float acceleration;
    public float GetAcceleration() { return acceleration; }                 public void UpgradeAcceleration(float increase) { acceleration += increase; }

    [SerializeField] protected float deceleration;
    public float GetDeceleration() { return deceleration; }

    [SerializeField] protected float baseSpeed;
    public float GetBaseSpeed() { return baseSpeed; }                       public void UpgradeBaseSpeed(float increase) { baseSpeed += increase; }

    [SerializeField] protected float playerSpeed; //Max Speed of the player
    public float GetPlayerSpeed() { return playerSpeed; }                   public void UpgradeSpeed(float increase) { playerSpeed += increase; }

    [SerializeField] protected float shotInterval; //Interval between player shots (decrease to fire more)
    public float GetShotInterval() { return shotInterval; }                 public void UpgradeFireRate(int decrease) { shotInterval -= decrease; }
    
    [SerializeField] protected float responsiveness; //How fast player movement responds to input changes
    public float GetResponsiveness() { return responsiveness; }             public void UpgradeResponsiveness(float increase) { responsiveness += increase; }
    
    [SerializeField] protected float dashForce; //Horizontal force for dash movement ability
    public float GetDashForce() { return dashForce; }                       public void UpgradeDash(float increase) { dashForce += increase; }
    


    [SerializeField] protected float critHitChance; //Chance for a player to score a critical hit
    public float GetCritChance() { return critHitChance; }                  public void UpgradeCritChance(float increase) { critHitChance += increase; }
    
    [SerializeField] protected float critHitBonus;  //Damage multiplier for critical hits
    public float GetCritBonus() { return critHitBonus; }                    public void UpgradeCritDamage(float increase) { critHitBonus += increase; }
    
    [SerializeField] protected float bulletTimeSlow;    //Percentage of game speed during bullet time, lower = slower
    public float GetBTSlow() { return bulletTimeSlow; }                     public void UpgradeBTSlow(float decrease) { bulletTimeSlow -= decrease; }
    


    [SerializeField] protected int maxHealth; //Player Maximum Health
    public int GetMaxHealth() { return maxHealth; }                         public void UpgradeMaxHealth(int increase) { maxHealth += increase; ModifyHealth(increase); }
    
    [SerializeField] protected float abilityRecharge; //Recharge multiplier for ability cooldowns, lower = faster recharge
    public float GetRecharge() { return abilityRecharge; }                  public void UpgradeRecharge(float decrease) { abilityRecharge -= decrease; }
    
    [SerializeField] protected float invulnerabilityTime;   //Time that the player is invulnerable after being hit
    public float GetInvulnerabilityTime() { return invulnerabilityTime; }   public void UpgradeInvulnerabilityTime(float increase) { invulnerabilityTime += increase; }
    
    [SerializeField] protected float receivedKnockbackMulti; //Multiplier for the force of knockback received when hit by an enemy, lower = less knockback
    public float GetKnockbackMultiplier() { return receivedKnockbackMulti; } public void UpgradeKnockbackMultiplier(float decrease) { receivedKnockbackMulti -= decrease; }




        //Other stats
    [SerializeField] protected int armor;
    public int GetArmor() { return armor; }                                 public void UpgradeArmor(int increase) { armor += increase; }

    [SerializeField] protected float armorScaling;
    public float GetArmorScaling() { return armorScaling; }
    public void UpgradeArmorScaling(float increase) { armorScaling += increase; }


    //Original stats for resetting 




    //Singleton
    public static PlayerStats instance;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        health = maxHealth;
        PlayerHUD.instance.UpdateHealth();
        //dice_list.Add(6); //Initial starting d6 projectile
    }

    void Update()
    {
        
    }

    public void ModifyHealth (int modify)
    {

        health = Mathf.Clamp(health + modify, 0, 20);
        if (health > maxHealth) health = maxHealth;
        PlayerHUD.instance.UpdateHealth();


        //Play Get-Hit Sound
        if (health < maxHealth && health != 0)
        {
            //SpeedRTPCControl.instance.PlayGetHitSound();  //Play Get_Hit Sound.
        }

        if (health <= 0)
        {
            //Player died
            LevelManager.instance.LoseLevel();
            //SpeedRTPCControl.instance.PlayerDead();  //Stop Get_Hit Sound.

        }


    }


    //Save this to turn off when a buff is reset
    Coroutine currentDamageBuff;
    //Call this when a damage-pickup is picked up
    public void BuffBonusDamage (float duration, int b)
    {
        if (currentDamageBuff != null) {
            StopCoroutine(currentDamageBuff);
            bonusDamage -= b;
        }
        currentDamageBuff = StartCoroutine(BonusDamageDuration(duration, b));
    }
    public IEnumerator BonusDamageDuration(float duration, int b)
    {

        //How much bonus damage the player gets
        int buff = b;

        float timer = 0f;
        bonusDamage += buff;

        //Sound of regular damage
        //SpeedRTPCControl.instance.PlayRegularShoot();

        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        bonusDamage -= buff;

        //Sound of bonus damage
        //SpeedRTPCControl.instance.PlayBuffShoot();
    }



    //Save this to turn off when a buff is reset
    Coroutine currentSpeedBuff;
    //Call this when a speed-pickup is picked up
    public void BuffSpeed(float duration, float b)
    {
        if (currentSpeedBuff != null) {
            StopCoroutine(currentSpeedBuff);
            baseSpeed /= b;
            playerSpeed /= b;
            acceleration /= b;

        }
        currentSpeedBuff = StartCoroutine(SpeedBuffDuration(duration, b));
    }
    public IEnumerator SpeedBuffDuration(float duration, float b)
    {


        float timer = 0f;

        baseSpeed *= b;
        playerSpeed *= b;
        acceleration *= b;

        //Play music speed up
        //PickUpAudio.instance.PlayFasterSpeed();
        //Play Sound
        //PickUpAudio.instance.PlayPickUpSpeed();

        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        baseSpeed /= b;
        playerSpeed /= b;
        acceleration /= b;

        //Play music speed down
        //PickUpAudio.instance.PlayNormalSpeed();

    }


    

}