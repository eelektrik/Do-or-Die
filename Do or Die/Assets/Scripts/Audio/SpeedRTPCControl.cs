/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedRTPCControl : MonoBehaviour
{
    public AK.Wwise.RTPC SpeedOfDice;
    public AK.Wwise.RTPC PlayerHealth;
    public AK.Wwise.State GameOver;
    public AK.Wwise.State Healthy;
    public AK.Wwise.State LowHealth;
    public AK.Wwise.Event DiceRoll;
    public AK.Wwise.Event DamageSound;
    public AK.Wwise.Event NextLevel;
    public AK.Wwise.Event NewSkill;
    public AK.Wwise.Event PopSound;
    public AK.Wwise.Event FireSound;
    //public AK.Wwise.Event EnemySpawn;
    //public AK.Wwise.Event EnemyDeadSound;
    public AK.Wwise.Event SoundPause;
    public AK.Wwise.Event SoundUnPause;
    public AK.Wwise.Event Upgrade;
    public AK.Wwise.Event EnemyProjectile;
    public AK.Wwise.Event EnemyGotHit;
    public AK.Wwise.Event BulletDrop;

    public AK.Wwise.Switch RegularShoot;
    public AK.Wwise.Switch BuffShoot;










    GameObject dice;


    public float Speed;
    public float Health;
    private Rigidbody speed_rigidbody;



    //Game Process
    public void NextLevelSound()
    {
        NextLevel.Post(gameObject);
    }

    public void PlayNewSkill()
    {
        NewSkill.Post(gameObject);
    }

    public void PlayUpgrade()
    {
        Upgrade.Post(gameObject);
    }



    //Damage
    public void PlayGetHitSound()
    {
        DamageSound.Post(gameObject);
    }

    public void PlayerDead()
    {
        DamageSound.Stop(gameObject);
    }


    //PlayerAction
    public void PlayPopSound()
    {
        PopSound.Post(gameObject);
    }

    public void PlayFireSound()
    {
        FireSound.Post(gameObject);
    }

    public void PlayBulletDrop()
    {
        BulletDrop.Post(gameObject);
    }

    public void PlayRegularShoot()
    {
        RegularShoot.SetValue(gameObject);
    }

    public void PlayBuffShoot()
    {
        BuffShoot.SetValue(gameObject);
    }


    //Pause Game
    public void PauseSound()
    {
        SoundPause.Post(gameObject);
    }

    public void ResumeSound()
    {
        SoundUnPause.Post(gameObject);
    }



    


    //Enemy sound
    public void PlayEnemyProjectile()
        {
            EnemyProjectile.Post(gameObject);
        }

    public void PlayEnemyGotHit()
        {
            EnemyGotHit.Post(gameObject);
        }





    //Singleton
    public static SpeedRTPCControl instance;

    private void Awake()
    {
        //Singleton
        instance = this;

        //Get Speed
        speed_rigidbody = GetComponent<Rigidbody>();
        dice = this.gameObject;
    }


    // Start is called before the first frame update
    void Start()
    {
        DiceRoll.Post(gameObject);
        RegularShoot.SetValue(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        var speedVector = new Vector2(speed_rigidbody.velocity.x, speed_rigidbody.velocity.z);
        Speed = speedVector.magnitude;
        SpeedOfDice.SetGlobalValue(Speed);

        //Get Health
        Health = PlayerStats.instance.health;
        PlayerHealth.SetGlobalValue(Health);

        if (Health == 0)
        {
            GameOver.SetValue();
            FireSound.Stop(gameObject);

        }

        if (Health > 4 && Health <= 20)
        {
            Healthy.SetValue();
        }

        if (Health <= 4 && Health>0)
        {
            LowHealth.SetValue();
        }
    }
}
*/