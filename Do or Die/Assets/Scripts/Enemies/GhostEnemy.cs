using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemy : Enemy
{


    [SerializeField] protected float speed;


    private void Start()
    {

        float temp = moveAwayRatio + moveParallelRatio + moveTowardsRatio;
        //Normalize values, accounting for any user input amounts.
        moveTowardsRatio = moveTowardsRatio / temp;
        moveParallelRatio = moveParallelRatio / temp;
        moveAwayRatio = moveAwayRatio / temp;
        health = maxHealth;
        healthBar = HealthBarManager.instance.SetupHealthBar(this);

        //Reference calls
        rb = GetComponent<Rigidbody>();
        attackRange = attackRange + Random.Range(-rangeVariance, rangeVariance);

        //The ghost enemy will loop its float animation on start
        GetComponent<Animator>().Play("Float");

    }


    protected override void MoveTowardsPlayer()
    {

        //Move via transform, and not navmesh
        transform.position = Vector3.MoveTowards(transform.position, PlayerController.instance.transform.position + new Vector3(0,0.5f,0), speed * Time.deltaTime);
        //Rotate object to face the player
        transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);

    }

    public override void MoveOrAttack()
    {
        //If the enemy is still in cooldown, they can't move or attack
        if (!actionable)
        {
            return;
        }
        /*if ((PlayerController.instance.transform.position - transform.position).magnitude >= rubberbandDistance)
        {
            //redirect = false;
            //towards = true;
            //away = false;
            //parallel = false;
            //nma.speed = nma.speed + rubberbandBoost;
            MoveTowardsPlayer();
            //nma.destination = Vector3.Lerp(nma.destination, target, directionChangeResponsiveness * Time.deltaTime);
            //timer += Time.deltaTime;
        }*/
        else
        {
            //nma.speed = initialSpeed;
            //If the enemy is being pushed, they can't move or attack
            if (beingPushed)
            {
                return;
            }

            if (!CanAttackPlayer())
            {
                //If enemy isn't close enough to the player, keep moving

                MoveTowardsPlayer();

            }
            else
            {
                //If enemy is close enough, fire
                Attack();
                StopMoving();
            }
        }
    }


    public override void Knockback(Vector3 source, float distance)
    {
        return;
    }


    public override IEnumerator KnockbackDisable(Vector3 source, float knockbackForce)
    {
        actionable = false;
        //nma.enabled = false;
        beingPushed = true;
        rb.isKinematic = false;

        float kbtimer = 0f;
        Vector3 direction = (transform.position - source);
        rb.AddForce(new Vector3(direction.x, 0, direction.z).normalized * knockbackForce);
        while (kbtimer <= kbdt)
        {
            kbtimer += Time.deltaTime;
            yield return null;
        }
        //nma.velocity = rb.velocity;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        beingPushed = false;
        rb.isKinematic = true;
        //nma.enabled = true;
        actionable = true;
    }


}
