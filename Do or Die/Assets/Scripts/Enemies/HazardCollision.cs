using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardCollision : MonoBehaviour
{

    //Collision script to be put on things like explosions, lava, etc
    //For now, assume all collisions will be using TRIGGER colliders


    //How much damage this hazard should do (assume all hazards deal damage right now)
    [SerializeField] protected int hazardDamage = 1;

    //How much the player should be knocked back
    [SerializeField] protected float knockback = 20;


    private void OnTriggerStay(Collider other)
    {


        if (other.TryGetComponent(out PlayerCollision player) && !player.IsInvulnerable())
        {

            player.TakeHit(transform, hazardDamage);

        }


        //if (TryGetComponent(out EnemyProjectile projectile)
        //    && other.gameObject.TryGetComponent(out PlayerCollision player)
        //    && !player.IsInvulnerable())
        //{
        //    player.TakeHit(transform, projectile.GetDamage());
        //    if (destroyOnContact) { Destroy(this.gameObject); }
        //}
        //else if (TryGetComponent(out EnemyMelee melee)
        //    && other.gameObject.TryGetComponent(out PlayerCollision player_)
        //    && !player_.IsInvulnerable())
        //{
        //    player_.TakeHit(transform, melee.GetDamage());
        //    if (destroyOnContact) { Destroy(this.gameObject); }
        //}
        //else if (other.gameObject.TryGetComponent(out PlayerCollision player__)
        //         && !player__.IsInvulnerable())
        //{
        //    player__.TakeHit(transform, GetComponentInParent<Enemy>().GetDamage());
        //    if (destroyOnContact) { Destroy(this.gameObject); }
        //}
    }


    public void DestroyHazard ()
    {
        Destroy(this.gameObject);
    }


    //private void StrikePlayer(Transform player)
    //{

    //    PlayerStats.instance.ModifyHealth(-hazardDamage);

    //    //Push the player away from this enemy
    //    Vector3 strikeDirection = player.position - transform.position;
    //    player.gameObject.GetComponent<Rigidbody>().AddForce(strikeDirection.normalized * knockback, ForceMode.Impulse);

    //}

}
