using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipeProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected float minimumSpeedResponsiveness;
    [SerializeField] protected float projectileLife;
    private Vector3 direction; public void SetDirection(Vector3 v) { direction = v; }
    public Vector3 GetDirection() { return direction; }
    [SerializeField] protected int max_damage;
    [SerializeField] protected float damage_bonus; public void SetDamageBonus(int d) { damage_bonus = d; }
    private float traveledDistance;
    private Rigidbody m_rigidBody;
    private MeshCollider m_meshCollider;

    [SerializeField] protected float projectileMinimumSpeed;
    [SerializeField] protected float projectileSpeed;
    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_meshCollider = GetComponent<MeshCollider>();
    }

    private void FixedUpdate()
    {
        if (!m_meshCollider.isTrigger && m_rigidBody.velocity.magnitude < projectileMinimumSpeed)
        {
            Vector3 directionxz = new Vector3(direction.x, 0, direction.z);
            Vector3 adjustment = directionxz.normalized * projectileMinimumSpeed+Vector3.up;
            Vector3 force_direction = Vector3.Lerp(m_rigidBody.velocity, adjustment, minimumSpeedResponsiveness * Time.deltaTime);
            m_rigidBody.velocity = Vector3.zero;
            m_rigidBody.AddForce(force_direction, ForceMode.Impulse);
            m_rigidBody.AddTorque(-m_rigidBody.velocity.z, 0f, m_rigidBody.velocity.x);
        }
    }

    public void StartProjectile(Vector3 d)
    {
        //Set direction
        direction = d;
        Vector3 force_direction = new Vector3(d.x, 0, d.z).normalized * projectileSpeed + Vector3.up;
        //Reset the velocity to zero, then set it
        m_rigidBody.velocity = Vector3.zero;
        m_rigidBody.AddForce(force_direction, ForceMode.Impulse);
        m_rigidBody.AddTorque(-force_direction.z, 0f, force_direction.x);
        //Start life timer
        StartCoroutine(ProjectileLifeDuration());
    }

    public int GetDamage()
    {
        //Crit occured
        if (Random.Range(.00f, 1.0f) <= PlayerStats.instance.GetCritChance())
        {
            return Mathf.RoundToInt(
                (max_damage + Random.Range(1, max_damage + 1)   //Max damage from die added to random roll from die
                + damage_bonus * 2)                              //Flat damage bonus added twice
                * PlayerStats.instance.GetCritBonus()            //Starts at 1.0, as crit damage is already calculating bonus damage into it
                + 0.5f                                           //To force rounding up
                );
        }
        else return Random.Range(1, max_damage + 1) + Mathf.RoundToInt(damage_bonus);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.GetComponent<Enemy>() != null) { DamageEnemy(other.gameObject.GetComponent<Enemy>()); }
        if (other.gameObject.TryGetComponent(out Enemy e)) { DamageEnemy(e); }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Wall"))
        {
            direction = new Vector3(m_rigidBody.velocity.x, 0, m_rigidBody.velocity.z).normalized;
        }
        if (collision.gameObject.TryGetComponent(out Enemy e))
        {
            //DamageEnemy(e);
            m_rigidBody.velocity = Vector3.zero;
            m_meshCollider.isTrigger = true;
            m_rigidBody.useGravity = false;
            m_rigidBody.velocity = Vector3.zero;
            transform.Translate(direction.normalized * Time.deltaTime);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("EnemyProjectile"))
        {
            transform.Translate(direction.normalized * Time.deltaTime);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("EnemyProjectile"))
        {
            m_meshCollider.isTrigger = false;
            m_rigidBody.useGravity = true;
            Vector3 force_direction = new Vector3(direction.x, 0, direction.z).normalized * projectileMinimumSpeed/2 + Vector3.up * Time.deltaTime;
            //Reset the velocity to zero, then set it
            m_rigidBody.velocity = Vector3.zero;
            m_rigidBody.AddForce(force_direction, ForceMode.Impulse);
            m_rigidBody.AddTorque(-force_direction.z, 0f, force_direction.x);
        }
        if (collision.gameObject.TryGetComponent(out Enemy e))
        {
            DamageEnemy(e);
        }
    }
    private void DamageEnemy(Enemy e)
    {
        e.Knockback(transform.position, PlayerStats.instance.GetShotKnockback() * m_rigidBody.mass);
        e.TakeDamage(GetDamage());
    }

    //Start the life duration of the projectile, if it runs out of time without being destroyed, then destroy it anyways
    IEnumerator ProjectileLifeDuration()
    {
        float timer = 0;
        while (timer < projectileLife)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        FinishProjectile();
    }

    private void FinishProjectile()
    {
        Destroy(this.gameObject);
    }
}
