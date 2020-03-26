using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{


    public int maxHealth;
    protected int health;

    public int exp;
    public int points;

    protected HealthBar healthBar;


    protected bool actionable = true;
    public int touchDamage;  public int GetDamage() { return touchDamage; }
    public float attackRange;
    [SerializeField] protected float rangeVariance;
    //Time to wait before actionable after attacking
    public float attackCooldown;
    [SerializeField] protected float knockbackDisableTime;  //Base knockback disable time, multiplied by force of impacts
    protected float kbdt;                                     //Actual time disabled recalculated in knockback()
    public bool beingPushed = false;

    private NavMeshAgent nma;
    protected Rigidbody rb;
    private float initialAngularSpeed;
    private float initialSpeed;
    //Speed that enemies used to be knocked back; Same for every enemy
    private static float knockbackSpeed = 3000;
    [SerializeField] protected float moveTowardsRatio;
    [SerializeField] protected float moveAwayRatio;
    [SerializeField] protected float moveParallelRatio;
    [SerializeField] protected float timeBetweenRedirection;
    //[SerializeField] protected float rubberbandDistance;
    //[SerializeField] protected float rubberbandBoost;
    private Vector3 target;
    private float choice, timer;
    private bool redirect, towards, away, parallel;

    [SerializeField] protected Material redMaterial;
    [SerializeField] protected Mesh redMesh;
    [SerializeField] protected MeshRenderer redMeshRenderer;


    // Start is called before the first frame update
    void Start()
    {
        float temp = moveAwayRatio + moveParallelRatio + moveTowardsRatio;
        //Normalize values, accounting for any user input amounts.
        moveTowardsRatio = moveTowardsRatio / temp;
        moveParallelRatio = moveParallelRatio / temp;
        moveAwayRatio = moveAwayRatio / temp;
        health = maxHealth;
        healthBar = HealthBarManager.instance.SetupHealthBar(this);
        //Reference calls
        nma = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        initialSpeed = nma.speed;
        initialAngularSpeed = nma.angularSpeed;
        timer = 0f;
        redirect = true;
        towards = false;
        away = false;
        parallel = false;
        attackRange = attackRange + Random.Range(-rangeVariance, rangeVariance);
    }

    // Update is called once per frame
    void Update()
    {

        //if (beingPushed) { UpdateKnockback(); }
        if (actionable) { MoveOrAttack(); }

    }


    //Function to be called for each enemy when they attack
    //Different enemies will have their own attacks
    public virtual void Attack ()
    {

    }


    protected virtual void MoveTowardsPlayer()
    {

        //NMA is destroyed before this script is destroyed, so check if we still have the NMA
        if (nma == null) { return; }

        target = PlayerController.instance.transform.position;
        GetComponent<Animator>().SetBool("Moving", true);
        towards = true;
        redirect = false;
        nma.destination = target;

    }

    protected void MoveAwayFromPlayer()
    {

        //NMA is destroyed before this script is destroyed, so check if we still have the NMA 
        if (nma == null) { return; }

        target = transform.position - GetDirectionTowardsPlayer().normalized;
        GetComponent<Animator>().SetBool("Moving", true);
        away = true;
        redirect = false;
        nma.destination = target;

    }

    protected void MoveParallelToPlayer()
    {

        //NMA is destroyed before this script is destroyed, so check if we still have the NMA
        if (nma == null) { return; }

        Vector2 movement = PlayerController.instance.GetCurrentDirection().normalized;
        target = new Vector3(transform.position.x + movement.x, transform.position.y, transform.position.z + movement.y);
        GetComponent<Animator>().SetBool("Moving", true);
        parallel = true;
        redirect = false;
        nma.destination = target;

    }

    protected void StopMoving()
    {

        //NMA is destroyed before this script is destroyed, so check if we still have the NMA
        if (nma == null) { return; }

        nma.destination = transform.position;
        GetComponent<Animator>().SetBool("Moving", false);

    }

    

    public virtual bool CanAttackPlayer()
    {

        //Shield layer is layer 16
        int shieldLayerMask = 1 << 16;

        Vector2 position = new Vector2(transform.position.x, transform.position.z);
        Vector2 playerPosition = new Vector2(GameObject.FindGameObjectWithTag("Player").transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.z);

        RaycastHit hit;
        Physics.Raycast(transform.position, GetDirectionTowardsPlayer(), out hit, attackRange);
        if (attackRange >= Vector2.Distance(position, playerPosition)) {

            if (hit.transform.gameObject.GetComponent<PlayerCollision>() != null)
            {
                return true;
            }

        }
        //if (attackRange >= Vector2.Distance(position, playerPosition)) { return true; }
        return false;

    }


    public virtual void MoveOrAttack ()
    {
        //If the enemy is still in cooldown, they can't move or attack
        if (!actionable)
        {
            return;
        }
        /*if ((PlayerController.instance.transform.position - transform.position).magnitude >= rubberbandDistance)
        {
            redirect = false;
            towards = true;
            away = false;
            parallel = false;
            nma.speed = nma.speed + rubberbandBoost;
            MoveTowardsPlayer();
            timer += Time.deltaTime;
        }*/
        else
        {
            nma.speed = initialSpeed;
            //If the enemy is being pushed, they can't move or attack
            if (beingPushed)
            {
                return;
            }

            if (!CanAttackPlayer())
            {
                //If enemy isn't close enough to the player, keep moving

                if (nma.velocity.magnitude <= 0.2f || timer >= timeBetweenRedirection)
                {
                    redirect = true;
                    towards = false;
                    away = false;
                    parallel = false;
                }
                if (redirect) //If enough time has passed to pick a new movement, reset the timer and choose a new random value
                {
                    choice = Random.value;
                    timer = 0f;
                }
                if (towards || (redirect && choice <= moveTowardsRatio))
                {
                    MoveTowardsPlayer();
                }
                else if (away || (redirect && choice <= moveTowardsRatio + moveAwayRatio))
                {
                    MoveAwayFromPlayer();
                }
                else
                {
                    MoveParallelToPlayer();
                }
                timer += Time.deltaTime;
            }
            else
            {
                //If enemy is close enough, fire
                Attack();
                StopMoving();
            }
        }
    }


    //Not currently used
    protected void UpdateKnockback ()
    {
        if (HasReachedDestination())
        {

            //Reset knockback state
            beingPushed = false;
            //Reset angular speed so the object doesn't rotate
            nma.angularSpeed = initialAngularSpeed;
            //Reset the speed to the push speed
            nma.speed = initialSpeed;

        } 
    }


    public virtual void Knockback (Vector3 source, float playerKnockback)
    {
        if (!beingPushed)
        {
            kbdt = (knockbackDisableTime * playerKnockback);
        }
        else
        {
            kbdt += (knockbackDisableTime * playerKnockback);
        }
        StartCoroutine(KnockbackDisable(source, playerKnockback));
        /*
        Vector3 direction = (transform.position - source);
        direction = new Vector3(direction.x, 0, direction.z).normalized;



        if (beingPushed)
        {
            //If the enemy is already being pushed, then add to the push destination
            //nma.destination += distance * direction;
        }
        else
        {

            //Otherwise, set a new destination to where they should be pushed
            nma.destination = transform.position + (direction * distance);
            //Set angular speed so the object doesn't rotate
            nma.angularSpeed = 0;
            //Set the speed to the push speed
            nma.speed = knockbackSpeed;

            //Finally set that we're being pushed
            beingPushed = true;

        }
        */
    }


    //Called when this enemy gets touched by the player when on fire
    public void GetBurned ()
    {
        OnZeroHealth();
    }
    //Called when the enemy falls off the board
    public void FallDeath ()
    {
        OnZeroHealth();
    }


    //Called when any enemy hits 0 health
    private void OnZeroHealth ()
    {


        //See if this enemy spawns a drop
        PickUpManager.instance.OnEnemyDeath(transform.position);

        //Turn off enemy NMA, health bar, etc

        //Destroy health bar
        DestroyHealthBar();

        //Freeze Y rotation?
        rb.freezeRotation = true;
        //Deactivate collision
        GetComponent<CapsuleCollider>().enabled = false;
        //Deactivate NMA
        if (nma != null) { nma.enabled = false; }

        //See if there are any effects on death
        if (TryGetComponent<SpawnOnDeath>(out SpawnOnDeath death)) { death.Spawn(); }
        
        //Finally play the death animation
        GetComponent<Animator>().Play("Death");

    }

    
    //Called when the enemy has finished its death animation
    public void DestroyEnemy()
    {

        //Need to increment score/EXP
        ExperienceManager.instance.GainExperience(exp);
        ScoreManager.instance.IncreaseScore(points);

        //Alert the wave manager that an enemy has died
        EnemyWaveManager.instance.OnEnemyDeath();

        Destroy(nma);
        Destroy(this.gameObject);

    }

    //Call when you need to delete this enemy's health bar
    public void DestroyHealthBar ()
    {

        if (healthBar == null) { return; }

        //Destroy health bar
        Destroy(healthBar);
        Destroy(healthBar.gameObject);

    }

    //Called when all active enemies are cleared (NOT KILLED BY THE PLAYER)
    public void ClearEnemy ()
    {

        Destroy(nma);
        Destroy(this.gameObject);

    }


    //Call this when hit by the player projectile
    public virtual void TakeDamage (int d)
    {

        //If the enemy is already dead, don't take damage
        if (health <= 0) { return; }

        health -= d;
        DamageNumberManager.instance.DisplayDamage(transform, d);

        //Play Enemy got hit sound.
        //SpeedRTPCControl.instance.PlayEnemyGotHit();

        GetComponent<Animator>().Play("Take Damage");

        //For now, if the enemy dies, just delete the object
        if (IsDead())
        {
            //Play Enemy_Dead Sound
            //SpeedRTPCControl.instance.PlayEnemyDeadSound();    //currently with no spatial position...
            OnZeroHealth();
        }

        float percent = (float) health / maxHealth;
        StartCoroutine(healthBar.UpdateHealthBar(percent));

    }


    IEnumerator FlashRed ()
    {

        float timer = 0;
        float flashTime = 60;

        float baseAlpha = 0.8f;

        while (timer < flashTime)
        {
            yield return null;
        }

    }


    private bool HasReachedDestination ()
    {
        //Check if we've reached the destination
        if (!nma.pathPending)
        {
            if (nma.remainingDistance <= nma.stoppingDistance)
            {
                if (!nma.hasPath || nma.velocity.sqrMagnitude == 0f)
                {
                    //Done
                    return true;
                }
            }
        }

        return false;
    }


    public virtual IEnumerator PostAttack ()
    {

        actionable = false;
        float timer = 0f;

        while (timer <= attackCooldown)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        actionable = true;

    }

    public virtual IEnumerator KnockbackDisable(Vector3 source, float knockbackForce)
    {
        actionable = false;
        nma.enabled = false;
        beingPushed = true;
        rb.isKinematic = false;

        float kbtimer = 0f;
        Vector3 direction = (transform.position - source);
        rb.AddForce(new Vector3(direction.x, 0, direction.z).normalized * knockbackForce);
        while (kbtimer <= kbdt)
        {
            kbtimer += Time.deltaTime;
            if (rb.velocity.magnitude < 0.05f)
                kbtimer += (Time.deltaTime + (kbtimer * Time.deltaTime)); //Speeds up enemy recovery when the rigidbody is not moving much, scaling up faster the higher kbtimer is
            yield return null;
        }
        nma.velocity = rb.velocity;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        beingPushed = false;
        rb.isKinematic = true;
        nma.enabled = true;
        actionable = true;
    }

    protected Vector3 GetDirectionTowardsPlayer ()
    {
        return PlayerController.instance.transform.position - transform.position;
    }


    //Is this enemy dead?
    public bool IsDead ()
    {
        if (health <= 0) { return true; }
        else return false;
    }


}
