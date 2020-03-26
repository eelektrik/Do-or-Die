using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : Enemy
{
    [SerializeField] protected float projectileStartForce, projectileMinimumSpeed, projectileDuration, delayBetweenShots;
    [SerializeField] protected int shotsPerAttack;
    [SerializeField] protected bool isConstant, wallBounce;
   
    private string d4_low = "d4_low"; private string d4_med = "d4_med"; private string d4_norm = "d4_norm";
    private string d6_low = "d6_low"; private string d6_med = "d6_med"; private string d6_norm = "d6_norm";
    private string d8_low = "d8_low"; private string d8_med = "d8_med"; private string d8_norm = "d8_norm";
    private string d10_low = "d10_low"; private string d10_med = "d10_med"; private string d10_norm = "d10_norm";
    private string d12_low = "d12_low"; private string d12_med = "d12_med"; private string d12_norm = "d12_norm";
    private string d20_low = "d20_low"; private string d20_med = "d20_med"; private string d20_norm = "d20_norm";
    [SerializeField] protected damage_profiles damage_profile;

    void Update()
    {
        //if (beingPushed) { UpdateKnockback(); }
        MoveOrAttack();

    }

    public override void Attack()
    {
        StartCoroutine(AttackVolley());
        StartCoroutine(PostAttack());
        //SpeedRTPCControl.instance.PlayEnemyProjectile();  // Play Attack Sound

    }

    [System.Serializable]
    public enum damage_profiles
    {
        d4_low, d4_med, d4_norm, d6_low, d6_med, d6_norm,
        d8_low, d8_med, d8_norm, d10_low, d10_med, d10_norm,
        d12_low, d12_med, d12_norm, d20_low, d20_med, d20_norm
    };

    IEnumerator AttackVolley()
    {
        //TODO: Add spread shot patterns
        float timer;
        Vector3 direction = GetDirectionTowardsPlayer(); //Use here to set an initial direction and fire every shot that way.
        for (int i = 0; i < shotsPerAttack; i++)
        {
            timer = 0;
            EnemyProjectile e = EnemyProjectileManager.instance.RequestProjectile(transform.position);
            e.StartProjectile(direction, damage_profile.ToString()); //Redo GetDirectionTowardsPlayer() here for volleys that track the player movement.
                                                                    //TODO: Allow support for fixed or tracking volleys.
            e.SetDuration(projectileDuration);
            e.SetSpeed(projectileStartForce);
            e.SetMinSpeed(projectileMinimumSpeed);
            e.SetDestroyOnContact();
            e.SetRange(attackRange);
            if (isConstant) e.SetConstant(); else e.projectileImpulse();
            if (wallBounce) e.SetWallBounce();
            while (timer < delayBetweenShots)
            {
                timer += Time.deltaTime;
                //Debug.Log(timer);
                yield return null;
            }
            
        }
        
    }
}