using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{


    public bool destroyOnContact = false;


    private void OnCollisionStay(Collision other)
    {

        //Case: Enemy touches burning player
        if (other.gameObject.TryGetComponent(out PlayerCollision p)
            && p.IsBurning())
        {

            Debug.Log("SMASK");
            Enemy e = GetComponentInParent<Enemy>();
            if (e != null)
            {
            }

        }

        //Case: Enemy projectile hits player
        if (TryGetComponent(out EnemyProjectile projectile)
            && other.gameObject.TryGetComponent(out PlayerCollision player)
            && !player.IsInvulnerable())
        {
            player.TakeHit(transform, projectile.GetDamage());
            if (destroyOnContact) { Destroy(this.gameObject); }
        }
        //Case: Enemy melee hits player
        else if (TryGetComponent(out EnemyMelee melee)
            && other.gameObject.TryGetComponent(out PlayerCollision player_)
            && !player_.IsInvulnerable())
        {
            player_.TakeHit(transform, melee.GetDamage());
            if (destroyOnContact) { Destroy(this.gameObject); }
        }
        //Case: Enemy directly collides with player
        else if (other.gameObject.TryGetComponent(out PlayerCollision player__)
                 && !player__.IsInvulnerable())
        {
            player__.TakeHit(transform, GetComponentInParent<Enemy>().GetDamage());
            if (destroyOnContact) { Destroy(this.gameObject); }
        }
    }
    private void OnCollisionEnter(Collision other)
    {

        //Case: Enemy touches burning player
        if (other.gameObject.TryGetComponent(out PlayerCollision p)
            && p.IsBurning())
        {
            Debug.Log("SMASK");
            Enemy e = GetComponentInParent<Enemy>();
            if (e != null)
            {
            }
        }

        //Case: Enemy projectile hits player
        if (TryGetComponent(out EnemyProjectile projectile)
            && other.gameObject.TryGetComponent(out PlayerCollision player)
            && !player.IsInvulnerable())
        {
            player.TakeHit(transform, projectile.GetDamage());
            if (destroyOnContact) { Destroy(this.gameObject); }
        }
        //Case: Melee Enemy hits player
        else if (TryGetComponent(out EnemyMelee melee)
            && other.gameObject.TryGetComponent(out PlayerCollision player_)
            && !player_.IsInvulnerable())
        {
            player_.TakeHit(transform, melee.GetDamage());
            if (destroyOnContact) { Destroy(this.gameObject); }
        }
        //Case: Enemy directly collides with player
        else if (other.gameObject.TryGetComponent(out PlayerCollision player__)
                 && !player__.IsInvulnerable())
        {
            player__.TakeHit(transform, GetComponentInParent<Enemy>().GetDamage());
            //Debug.Log("OnCollisionEnter");
            if (destroyOnContact) { Destroy(this.gameObject); }
        }
        //Case: Enemy collides with Warrior Shield
        else if (other.gameObject.TryGetComponent(out ShieldCollision shield))
        {
            Debug.Log("Shield knockback");
            float mass = FindObjectOfType<PlayerController>().GetComponent<Rigidbody>().mass;
            float force = FindObjectOfType<PlayerController>().GetComponent<Rigidbody>().velocity.magnitude;
            GetComponentInParent<Enemy>().Knockback(player__.transform.position, force * mass * shield.getKnockback());
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        //Case: Enemy touches burning player
        if (other.gameObject.TryGetComponent(out PlayerCollision p)
            && p.IsBurning())
        {

            Enemy e = GetComponentInParent<Enemy>();
            if (e != null)
            {
                e.GetBurned();
            }

        }

        //Case: Enemy projectile hits player
        if (TryGetComponent(out EnemyProjectile projectile)
            && other.gameObject.TryGetComponent(out PlayerCollision player)
            && !player.IsInvulnerable())
        {
            player.TakeHit(transform, projectile.GetDamage());
            if (destroyOnContact) { Destroy(this.gameObject); }
        }
        //Case: Melee Enemy hits player
        else if (TryGetComponent(out EnemyMelee melee)
            && other.gameObject.TryGetComponent(out PlayerCollision player_)
            && !player_.IsInvulnerable())
        {
            player_.TakeHit(transform, melee.GetDamage());
            if (destroyOnContact) { Destroy(this.gameObject); }
        }
        //Case: Enemy directly collides with player
        else if (other.gameObject.TryGetComponent(out PlayerCollision player__)
                 && !player__.IsInvulnerable())
        {
            player__.TakeHit(transform, GetComponentInParent<Enemy>().GetDamage());
            //Debug.Log("OnCollisionEnter");
            if (destroyOnContact) { Destroy(this.gameObject); }
        }
        //Case: Enemy collides with Warrior Shield
        else if (other.gameObject.TryGetComponent(out ShieldCollision shield))
        {
            Debug.Log("Shield knockback");
            float mass = FindObjectOfType<PlayerController>().GetComponent<Rigidbody>().mass;
            float force = FindObjectOfType<PlayerController>().GetComponent<Rigidbody>().velocity.magnitude;
            GetComponentInParent<Enemy>().Knockback(shield.transform.position, force * mass * shield.getKnockback());
        }
    }
    private void OnTriggerStay(Collider other)
    {
        /*if (TryGetComponent(out EnemyProjectile projectile)
            && other.gameObject.TryGetComponent(out PlayerCollision player)
            && !player.IsInvulnerable())
        {
            player.TakeHit(transform, projectile.GetDamage());
            if (destroyOnContact) { Destroy(this.gameObject); }
        }
        else if (TryGetComponent(out EnemyMelee melee)
            && other.gameObject.TryGetComponent(out PlayerCollision player_)
            && !player_.IsInvulnerable())
        {
            player_.TakeHit(transform, melee.GetDamage());
            if (destroyOnContact) { Destroy(this.gameObject); }
        }
        else if (other.gameObject.TryGetComponent(out PlayerCollision player__)
                 && !player__.IsInvulnerable())
        {
            player__.TakeHit(transform, GetComponentInParent<Enemy>().GetDamage());
            //Debug.Log("OnTriggerStay");
            if (destroyOnContact) { Destroy(this.gameObject); }
        }*/
    }


    //Not used right now
    //private void StrikePlayer (Transform player)
    //{

    //    PlayerStats.instance.ModifyHealth(-1);

    //    //Push the player away from this enemy
    //    Vector3 strikeDirection = player.position - transform.position;
    //    player.gameObject.GetComponent<Rigidbody>().AddForce(strikeDirection.normalized * 20, ForceMode.Impulse);

    //    if (destroyOnContact) { Destroy(this.gameObject); }

    //}


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
