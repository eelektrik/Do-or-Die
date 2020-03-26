using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] protected float projectileBounce;
    [SerializeField] protected float gravity_multiplier;
    [SerializeField] protected float minimumSpeedResponsiveness;
    private float projectileLife;
    private float player_speed_inheritance;
    [SerializeField] protected GameObject d4, d6, d8, d10, d12, d20;
    private Vector3 direction; public void SetDirection(Vector3 v) { direction = v; } public Vector3 GetDirection() { return direction; }
    private int max_damage;
    private float damage_bonus; public void SetDamageBonus(int d) { damage_bonus = d; }
    private float traveledDistance;
    private Rigidbody m_rigidBody;
    //private new Transform camera;
    private Vector3 originalScale;
    private Passive passive;
    private float projectileRange;
    private float assistRange;
    private float projectileMinimumSpeed;
    private float projectileSpeed;
    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        passive = PlayerController.instance.GetComponent<Passive>();
        
        transform.localScale = transform.localScale * PlayerStats.instance.GetShotSize();
        max_damage = 4; //Just an initial value, set to lowest dice value, updates during projectilestart
        UpdateStats();
    }

    private void UpdateStats()
    {
        projectileRange = PlayerStats.instance.GetProjectileRange();
        projectileMinimumSpeed = PlayerStats.instance.GetShotMinSpeed();
        projectileSpeed = PlayerStats.instance.GetShotSpeed();
        projectileLife = PlayerStats.instance.GetProjectileLife();
        player_speed_inheritance = PlayerStats.instance.GetInheritance();

        //Bonus damage shouldn't be calculated when the projectile is set, but when it deals damage
        //damage_bonus = PlayerStats.instance.GetBonusDamage();

        assistRange = PlayerStats.instance.GetAssistDistance();
        traveledDistance = 0f;
    }
    private void FixedUpdate()
    {
        if (traveledDistance < projectileRange)
        {
            if (m_rigidBody.velocity.magnitude < projectileMinimumSpeed)
            {
                float distanceRatio = projectileRange / (traveledDistance + 1);
                Vector3 directionxz = new Vector3(direction.x, 0, direction.z);
                Vector3 velocityxz = new Vector3(m_rigidBody.velocity.x, 0, m_rigidBody.velocity.z);
                Vector3 adjustment = (velocityxz.normalized + directionxz).normalized * projectileMinimumSpeed
                                + Vector3.up * (m_rigidBody.velocity.y - (Physics.gravity.y * (gravity_multiplier - 1) * Time.deltaTime));

                if (Mathf.Abs(m_rigidBody.velocity.y) < 0.25f && traveledDistance > 1f) //Keep it bouncing
                {
                    adjustment += (m_rigidBody.velocity.normalized + direction).normalized;
                    adjustment += Vector3.up * projectileBounce * distanceRatio;
                }
                Vector3 force_direction = Vector3.Lerp(m_rigidBody.velocity, adjustment, minimumSpeedResponsiveness * Time.deltaTime);
                m_rigidBody.velocity = Vector3.zero;
                m_rigidBody.AddForce(force_direction, ForceMode.Impulse);
                m_rigidBody.AddTorque(-m_rigidBody.velocity.z, 0f, m_rigidBody.velocity.x);
            }
        }
        else if(traveledDistance < assistRange)
        {
            if (m_rigidBody.velocity.magnitude < projectileMinimumSpeed/2)
            {
                float distanceRatio = assistRange / (traveledDistance + 1);
                Vector3 directionxz = new Vector3(direction.x, 0, direction.z);
                Vector3 velocityxz = new Vector3(m_rigidBody.velocity.x, 0, m_rigidBody.velocity.z);
                Vector3 adjustment = (velocityxz.normalized + directionxz).normalized * projectileMinimumSpeed/2
                                + Vector3.up * (m_rigidBody.velocity.y - (Physics.gravity.y * (gravity_multiplier - 1) * Time.deltaTime));

                if (Mathf.Abs(m_rigidBody.velocity.y) < 0.25f && traveledDistance > 1f) //Keep it bouncing
                {
                    adjustment += (m_rigidBody.velocity.normalized + direction).normalized;
                    adjustment += Vector3.up * projectileBounce * distanceRatio;
                }
                Vector3 force_direction = Vector3.Lerp(m_rigidBody.velocity, adjustment, minimumSpeedResponsiveness * Time.deltaTime);
                m_rigidBody.velocity = Vector3.zero;
                m_rigidBody.AddForce(force_direction, ForceMode.Impulse);
                m_rigidBody.AddTorque(-m_rigidBody.velocity.z, 0f, m_rigidBody.velocity.x);
            }
        }

        
    }


    private void Update()
    {
        traveledDistance += new Vector3(m_rigidBody.velocity.x, 0, m_rigidBody.velocity.z).magnitude * Time.deltaTime;
        Vector3 directionxz = new Vector3(direction.x, 0, direction.z);
        Vector3 velocityxz = new Vector3(m_rigidBody.velocity.x, 0, m_rigidBody.velocity.z);
        m_rigidBody.velocity += Vector3.up * Physics.gravity.y / 1.5f * (gravity_multiplier - 1) * Time.deltaTime;
    }
    public void StartProjectile (Vector3 d, int damage, float bonus)
    {
        UpdateStats();
        /*if (passive is ElementalDice)
        {
            //Set dice material
        }*/
        //Set direction
        Vector3 directionxz = new Vector3(d.x, 0, d.z);
        if (d.magnitude == 1)
        {
            direction = directionxz * projectileRange + Vector3.up * d.y;
        }
        else { direction = d;}

        //Set damage
        max_damage = damage;
        //Set Mesh - You can find the appropriate mesh based on first 2/3 characters from string damage
        SetMesh();
        originalScale = transform.localScale;

        directionxz = new Vector3(direction.x, 0, direction.z);
        /*Vector3 force_direction = directionxz.normalized * projectileSpeed
                                  + Vector3.up * Mathf.Sqrt(directionxz.magnitude) * (1 + projectileSpeed / projectileRange)
                                  + PlayerController.instance.gameObject.GetComponent<Rigidbody>().velocity * player_speed_inheritance;*/
        Vector3 force_direction = directionxz.normalized * projectileSpeed
                                  + Vector3.up * direction.y * (1 + directionxz.magnitude / assistRange) * Mathf.Sqrt(directionxz.magnitude)
                                  + PlayerController.instance.gameObject.GetComponent<Rigidbody>().velocity * player_speed_inheritance;
                                                  

        //Reset the velocity to zero, then set it
        m_rigidBody.velocity = Vector3.zero;
        m_rigidBody.AddForce(force_direction, ForceMode.Impulse);
        m_rigidBody.AddTorque(-force_direction.z, 0f, force_direction.x);

        //Vector3 force_direction = new Vector3(direction.x, 0, direction.z).normalized * velocity + Mathf.Sqrt(PlayerStats.instance.GetDiceMass() * (Mathf.Sqrt(direction.magnitude * Mathf.Sqrt(PlayerStats.instance.GetShotSpeed() * Mathf.Sqrt(PlayerStats.instance.GetAssistDistance()))) + direction.y*2)) * Vector3.up;
        //Reset the velocity to zero, then set it
        //GetComponent<Rigidbody>().velocity = Vector3.zero;
        //GetComponent<Rigidbody>().AddForce(force_direction + , ForceMode.Impulse);
        //GetComponent<Rigidbody>().AddTorque(-force_direction.z, 0, force_direction.x);


        //Start life timer
        StartCoroutine(ProjectileLifeDuration());
    }

    public int GetDamage()
    {

        //Update damage bonus here
        damage_bonus = PlayerStats.instance.GetBonusDamage();

        //Crit occured
        if (Random.Range(.00f, 1.0f) <= PlayerStats.instance.GetCritChance())
        {
            return Mathf.RoundToInt(
                (max_damage + Random.Range(1, max_damage + 1)   //Max damage from die added to random roll from die
                + damage_bonus *2)                              //Flat damage bonus added twice
                *PlayerStats.instance.GetCritBonus()            //Starts at 1.0, as crit damage is already calculating bonus damage into it
                +0.5f                                           //To force rounding up
                );
        }

        else return Random.Range(1, max_damage + 1) + Mathf.RoundToInt(damage_bonus);

    }

    private void SetMesh()
    {
        if (max_damage == 4)        SwapMesh(d4);
        else if (max_damage == 6)   SwapMesh(d6);
        else if (max_damage == 8)   SwapMesh(d8);
        else if (max_damage == 10)  SwapMesh(d10);
        else if (max_damage == 12)  SwapMesh(d12);
        else if (max_damage == 20)  SwapMesh(d20);
    }
    private void SwapMesh(GameObject active)
    {
        d4.SetActive(false);
        d6.SetActive(false);
        d8.SetActive(false);
        d10.SetActive(false);
        d12.SetActive(false);
        d20.SetActive(false);

        active.SetActive(true);
        this.GetComponent<MeshCollider>().sharedMesh = active.GetComponent<MeshFilter>().sharedMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.GetComponent<Enemy>() != null) { DamageEnemy(other.gameObject.GetComponent<Enemy>()); }
        if (other.gameObject.TryGetComponent(out Enemy e)) {DamageEnemy(e);}
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("EnemyProjectile"))
        {
            direction = new Vector3(m_rigidBody.velocity.x, 0, m_rigidBody.velocity.z).normalized;
        }
        //if (collision.gameObject.GetComponent<Enemy>() != null) { DamageEnemy(collision.gameObject.GetComponent<Enemy>()); }
        if (collision.gameObject.TryGetComponent(out Enemy e)) { DamageEnemy(e); }
    }


    private void DamageEnemy (Enemy e)
    {
        //passive.ActivatePassive(this);
        
        /*if (!e.IsDead())
        {
            e.Knockback(transform.position, m_rigidBody.velocity.magnitude * PlayerStats.instance.GetShotKnockback());
        }*/
        e.Knockback(transform.position, PlayerStats.instance.GetShotKnockback());
        e.TakeDamage(GetDamage());

        //For now, if a projectile hits an enemy, we'll delete the projectile
        //if (passive is Cleave || passive is ElementalDice) { FinishProjectile(); }
        FinishProjectile();
    }


    //Start the life duration of the projectile, if it runs out of time without being destroyed, then destroy it anyways
    IEnumerator ProjectileLifeDuration ()
    {
        float timer = 0;
        while (timer < projectileLife)
        {
            if (timer > 0.5 && m_rigidBody.velocity.magnitude < 0.75f) break; //Stop projectile when it stops moving
            timer+=Time.deltaTime;
            yield return null;
        }
        FinishProjectile();
    }


    //Deactivates the projectile and sets it as unused
    private void FinishProjectile ()
    {

        //Reset its size
        transform.localScale = originalScale;

        ProjectileManager.instance.AddUnusedProjectile(this);
        gameObject.SetActive(false);

    }
}
