using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [SerializeField] protected bool reach;
    [SerializeField] protected int meleeDamage;
    void Update()
    {
        //if (beingPushed) { UpdateKnockback(); }
        MoveOrAttack();
    }

    public override void Attack()
    {
        EnemyMelee e = EnemyMeleeManager.instance.RequestProjectile(transform.position, transform.rotation);
        if (!reach)
        {
            e.SetMelee();
            e.SetMeleeSpeed(20f);
            e.SetMeleeDuration(1.25f);
        }
        else
        {
            e.SetReach();
            e.SetMeleeSpeed(22f);
            e.SetMeleeDuration(2f);
        }
        e.MeleeSwing(GetDirectionTowardsPlayer().normalized, meleeDamage);
        StartCoroutine(PostAttack());

        Debug.Log("Enemy Attack!");

        //SpeedRTPCControl.instance.PlayEnemyProjectile();  // Play Attack Sound

    }
}
