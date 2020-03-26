using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] protected float projectileBounce;
    [SerializeField] protected float gravity_multiplier;
    [SerializeField] protected float minimumSpeedResponsiveness;
    public bool destroyOnContact;                               public void SetDestroyOnContact() { destroyOnContact = true; }
    private float projectileSpeed;           public void SetSpeed(float speed) { projectileSpeed = speed; }
    private float projectileMinimumSpeed;    public void SetMinSpeed(float minSpeed) { projectileMinimumSpeed = minSpeed;}
    private float projectileDuration;        public void SetDuration(float duration) { projectileDuration = duration; }
    private float projectileRange;           public void SetRange(float range) { projectileRange = range; }
      
    private Vector3 direction;                                  public void SetDirection(Vector3 v) {direction = v;}
    private bool isConstant;                                    public void SetConstant()           {isConstant = true;}
    private bool wallBounce;                                    public void SetWallBounce()         {wallBounce = true;}
    private bool endEarly;  //Used to make ProjectileLifeDuration end early, set where needed, mostly on collision.
    private float traveledDistance;
    private Rigidbody m_rigidBody;
    [SerializeField] protected GameObject d4, d6, d8, d10, d12, d20;
    [SerializeField] protected Material lowMat, medMat, normMat;
    private int[] damage_profile; //This objects damage profile
    //All available damage profiles listed below
    //Size of each damage profile is equal to dice value max
    //Numbers stored are percentiles checked against a random percentile number 1-100
    //Damage dealt is the array index +1 of the highest stored value the random percentile is larger then
    static int[] d4_low =   {0, 70, 85, 95};  //Average Damage: 1.50
    static int[] d4_med =   {0, 40, 70, 90};  //Average Damage: 2.00
    static int[] d4_norm =  {0, 25, 50, 75};  //Average Damage: 2.50
    static int[] d6_low =   {0, 31, 56, 76, 91, 96}; //Average Damage: 2.50
    static int[] d6_med =   {0, 23, 45, 63, 79, 90}; //Average Damage: 3.00
    static int[] d6_norm =  {0, 16, 33, 50, 67, 84}; //Average Damage: 3.50
    static int[] d8_low =   {0, 20, 40, 56, 72, 80, 88, 94}; //Average Damage: 3.50
    static int[] d8_med =   {0, 16, 31, 47, 62, 72, 81, 91}; //Average Damage: 4.00
    static int[] d8_norm =  {0, 13, 25, 38, 50, 63, 75, 88}; //Average Damage: 4.50
    static int[] d10_low =  {0, 16, 30, 43, 54, 64, 74, 83, 90, 96}; //Average Damage: 4.50
    static int[] d10_med =  {0, 13, 25, 36, 47, 58, 67, 76, 85, 93}; //Average Damage: 5.00
    static int[] d10_norm = {0, 10, 20, 30, 40, 50, 60, 70, 80, 90}; //Average Damage: 5.50
    static int[] d12_low =  {0, 12, 24, 36, 45, 53, 61, 69, 77, 86, 91, 96}; //Average Damage: 5.50
    static int[] d12_med =  {0, 10, 20, 30, 39, 48, 56, 64, 72, 80, 87, 94}; //Average Damage: 6.00
    static int[] d12_norm = {0, 09, 17, 25, 33, 42, 50, 59, 67, 75, 84, 92}; //Average Damage: 6.50
    static int[] d20_low =  {0, 08, 16, 24, 31, 38, 44, 50, 55, 60, 65, 70, 74, 78, 82, 85, 88, 91, 94, 97}; //Average Damage: 8.50
    static int[] d20_med =  {0, 06, 12, 18, 24, 30, 36, 42, 48, 54, 60, 64, 68, 72, 76, 80, 84, 88, 92, 96}; //Average Damage: 9.50
    static int[] d20_norm = {0, 05, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95}; //Average Damage: 10.50
    static Dictionary<string, int[]> damage_dict = new Dictionary<string, int[]>{
        { "d4_low", d4_low}, {"d4_med", d4_med}, {"d4_norm", d4_norm}, {"d6_low", d6_low}, {"d6_med", d6_med}, {"d6_norm", d6_norm},
        { "d8_low", d8_low}, {"d8_med", d8_med}, {"d8_norm", d8_norm}, {"d10_low",d10_low},{"d10_med",d10_med},{"d10_norm",d10_norm},
        { "d12_low",d12_low},{"d12_med",d12_med},{"d12_norm",d12_norm},{"d20_low",d20_low},{"d20_med",d20_med},{"d20_norm",d20_norm}};

    void Awake()
    {
        m_rigidBody = GetComponentInChildren<Rigidbody>();
        traveledDistance = 0f;
        destroyOnContact = false;
        isConstant = false;
        wallBounce = false;
        endEarly = false;
    }
    
    void FixedUpdate()
    {
        float distanceRatio = projectileRange / (traveledDistance + 1);
        if (isConstant == true)
        {
            projectileConstant();
        }
        else
        {
            if (traveledDistance < projectileRange)
            {
                if (m_rigidBody.velocity.magnitude < projectileMinimumSpeed)
                {
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
        }
        
    }

    private void Update()
    {
        traveledDistance += new Vector3(m_rigidBody.velocity.x, 0, m_rigidBody.velocity.z).magnitude * Time.deltaTime;
        //Debug.Log(traveledDistance);
        Vector3 directionxz = new Vector3(direction.x, 0, direction.z);
        Vector3 velocityxz = new Vector3(m_rigidBody.velocity.x, 0, m_rigidBody.velocity.z);
        m_rigidBody.velocity += Vector3.up * Physics.gravity.y / 1.5f * (gravity_multiplier - 1) * Time.deltaTime;
    }
    public void StartProjectile(Vector3 d, string damage)
    {
        //Set direction
        direction = d;
        //Set damage
        damage_profile = damage_dict[damage];
        //Set Mesh - You can find the appropriate mesh based on first 2/3 characters from string damage
        FindMeshMat(damage);
        //Reset traveled distance
        traveledDistance = 0f;

        //Start life timer
        StartCoroutine(ProjectileLifeDuration());
    }

    public void projectileConstant()
    {
        Vector3 force_direction = (direction.normalized * projectileSpeed);
        m_rigidBody.velocity = Vector3.zero;
        m_rigidBody.AddForce(force_direction, ForceMode.Impulse);
    }

    public void projectileImpulse()
    {
        Vector3 directionxz = new Vector3(direction.x, 0, direction.z);
        Vector3 force_direction = directionxz.normalized * projectileSpeed
                                  + Vector3.up * direction.y * (1 + directionxz.magnitude / projectileRange) * Mathf.Sqrt(directionxz.magnitude);

        //Reset the velocity to zero, then set it
        m_rigidBody.velocity = Vector3.zero;
        m_rigidBody.AddForce(force_direction, ForceMode.Impulse);
        m_rigidBody.AddTorque(-force_direction.z, 0f, force_direction.x);
    }

    IEnumerator ProjectileLifeDuration()
    {
        float timer = 0;
        while (timer < projectileDuration)
        {
            if (endEarly) break;
            if (timer > 0.5 && m_rigidBody.velocity.magnitude < .5f) break; //Stop projectile when it stops moving
            timer+= Time.deltaTime;
            yield return null;
        }
        FinishProjectile();
    }

    private void FinishProjectile()
    {
        EnemyProjectileManager.instance.AddUnusedProjectile(this);
        gameObject.SetActive(false);
        this.destroyOnContact = false;
        this.isConstant = false;
        this.wallBounce = false;
        this.endEarly = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((wallBounce && collision.transform.CompareTag("Wall")) || collision.transform.CompareTag("PlayerProjectile"))
        { 
            direction = GetComponent<Rigidbody>().velocity;
            direction = new Vector3(direction.x, 0, direction.z).normalized;
        }
        else if (collision.transform.CompareTag("Wall"))
        {
            direction = Vector3.zero;
            endEarly = true;
        }

        if (collision.gameObject.TryGetComponent(out PlayerCollision player)
            && !player.IsInvulnerable())
        {
            player.TakeHit(transform, this.GetDamage());
            if (destroyOnContact) { Destroy(this.gameObject); }
        }
    }
    public int GetDamage()
    {
        int percentile = Random.Range(1, 101);
        int i = 0;
        while (i + 1 < damage_profile.Length && damage_profile[i] < percentile && damage_profile[i + 1] <= percentile)
        {
            i++;
        }
        return i + 1;
    }

    private void FindMeshMat(string damage)
    {
        if (damage.Contains("d4"))
        {
            SwapMesh(d4);
            SwapMaterial(d4, damage);
        }
        else if (damage.Contains("d6"))
        {
            SwapMesh(d6);
            SwapMaterial(d6, damage);
        }
        else if (damage.Contains("d8"))
        {
            SwapMesh(d8);
            SwapMaterial(d8, damage);
        }
        else if (damage.Contains("d10"))
        {
            SwapMesh(d10);
            SwapMaterial(d10, damage);
        }
        else if (damage.Contains("d12"))
        {
            SwapMesh(d12);
            SwapMaterial(d12, damage);
        }
        else if (damage.Contains("d20"))
        {
            SwapMesh(d20);
            SwapMaterial(d20, damage);
        }

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

    private void SwapMaterial(GameObject die, string damage)
    {
        if (damage.Contains("low"))
        {
            die.GetComponent<MeshRenderer>().material = lowMat;
        }
        else if (damage.Contains("med"))
        {
            die.GetComponent<MeshRenderer>().material = medMat;
        }
        else if (damage.Contains("norm"))
        {
            die.GetComponent<MeshRenderer>().material = normMat;
        }
        
    }

}
