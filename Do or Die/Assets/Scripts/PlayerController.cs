using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    //Speed (gravity) acceleration
    /*
    public float smooth;
    private ConstantForce gravity;
    */
    [SerializeField] protected float maxTilt;
    [SerializeField] protected float smooth;
    [SerializeField] protected float gravity;
    [SerializeField] protected float slow_roll_multiplier;
    [SerializeField] protected float fast_roll_multiplier;
    [SerializeField] protected float direction_responsiveness;
    [SerializeField] protected float stopping_responsiveness;
    [SerializeField] protected float fall_multiplier;
    [SerializeField] protected float rolling_radius;
    [SerializeField] protected float stopping_radius;
    private float speed, roll_multiplier, direction_change;
    private ConstantForce player_gravity;
    private Rigidbody m_rigidbody;
    private MeshCollider m_meshCollider;
    private SphereCollider m_sphereCollider;
    private Vector2 input, current_direction;

    public Vector2 GetCurrentDirection() { return current_direction; }

    //Singleton
    public static PlayerController instance;
    private void Awake()
    {
        instance = this;
        m_rigidbody = GetComponent<Rigidbody>();
        m_sphereCollider = GetComponent<SphereCollider>();
        m_meshCollider = GetComponent<MeshCollider>();
        player_gravity = gameObject.AddComponent<ConstantForce>();
        player_gravity.force = new Vector3(0.0f, gravity, 0.0f);
    }


    // Start is called before the first frame update
    void Start()
    {
        speed = PlayerStats.instance.GetBaseSpeed();
        maxTilt = maxTilt * Mathf.PI / 180; //Converts degrees to radians for use with Sin and Cos functions

        CameraFollow.instance.SetSmooth(smooth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void FixedUpdate()
    {
        current_direction = new Vector2(m_rigidbody.velocity.x, m_rigidbody.velocity.z);
        if (m_rigidbody.velocity.y < 0)
        {
            m_rigidbody.velocity += Vector3.up * Physics.gravity.y * (fall_multiplier - 1) * Time.deltaTime;
        }
        GetInput();
        CalculateDirectionChange();
        UpdateCollider();
        UpdateSpeed();
        UpdateGravity();
    }

    private void GetInput()
    {
        input = InputManager.instance.GetLeftJoystickInputs();
    }
    void CalculateDirectionChange()
    {
        direction_change = Vector2.Dot(input.normalized, current_direction.normalized) * 0.25f + 0.75f; //Final value should be between 0.5 and 1.0
        //Debug.Log("Direction_change: " + direction_change);
    }
    void UpdateCollider()
    {
        if (m_rigidbody.velocity.magnitude < PlayerStats.instance.GetPlayerSpeed() * 1.15f)
        {
            m_sphereCollider.radius = Mathf.Lerp(m_sphereCollider.radius, stopping_radius, (PlayerStats.instance.GetDeceleration() + stopping_responsiveness) * Time.deltaTime);
            roll_multiplier = Mathf.Lerp(roll_multiplier, slow_roll_multiplier, (PlayerStats.instance.GetDeceleration() + stopping_responsiveness) * Time.deltaTime);
        }
        else
        {
            m_sphereCollider.radius = Mathf.Lerp(m_sphereCollider.radius, rolling_radius, (PlayerStats.instance.GetAcceleration() + direction_responsiveness) * Time.deltaTime);
            roll_multiplier = Mathf.Lerp(roll_multiplier, fast_roll_multiplier, (PlayerStats.instance.GetAcceleration() + direction_responsiveness) * Time.deltaTime);
        }
        //Debug.Log("roll multiplier: " + roll_multiplier);
        //Debug.Log("Sphere radius: " + m_sphereCollider.radius);
    }
    void UpdateSpeed()
    {
        if (input != Vector2.zero)
        {
            speed = Mathf.Lerp(speed, PlayerStats.instance.GetPlayerSpeed() * direction_change, PlayerStats.instance.GetAcceleration() * Time.deltaTime);
        }
        else
        {
            speed = Mathf.Lerp(speed, PlayerStats.instance.GetBaseSpeed(), PlayerStats.instance.GetDeceleration() * Time.deltaTime);
        }
        //Debug.Log("Speed: " + speed);
    }

    void UpdateGravity()
    {
        float gravity_x = Mathf.Sin(maxTilt * -input.x) * speed;
        float gravity_z = Mathf.Sin(maxTilt * -input.y) * speed;
        float gravity_y = Mathf.Cos(maxTilt * -input.x) * Mathf.Cos(maxTilt * -input.y);
        Vector3 inputForce = new Vector3(gravity_x, gravity_y, gravity_z) * gravity;
        Vector3 newForce = Vector3.zero;
        //Debug.Log("Velocity Magnitude: " + m_rigidbody.velocity.magnitude);

        //======Version 4=============
        if (input != Vector2.zero)
        {
            //Force slerping is influenced by the following:
            // 1.    PlayerStats.instance.GetResponsiveness(): Player responsiveness stat, default 1.0, increased through upgrades
            // 2.  * direction_change: How close your desired direction is to your current velocity. Value between 0.5(Opposite direction) and 1.0(Same direction)
            // 3a. * direction_responsiveness: Developer set modifier to direction reponsiveness, current value for testing is 12
            // 3b. * stopping_responsiveness: Developer set modifier to stopping responsiveness, current value for testing is 5
            // 4.  / GetComponent<Rigidbody>().velocity.magnitude: Magnituded of current velocity, change in force is less responsive when you are moving faster
            newForce = Vector3.Slerp(player_gravity.force, inputForce, PlayerStats.instance.GetResponsiveness() * direction_change * direction_responsiveness / GetComponent<Rigidbody>().velocity.magnitude * Time.deltaTime);
            player_gravity.force = newForce;
            m_rigidbody.AddTorque(new Vector3(-gravity_z, 0, gravity_x).normalized * roll_multiplier);
        }
        else
        {
            newForce = Vector3.Slerp(player_gravity.force, inputForce, PlayerStats.instance.GetResponsiveness() * stopping_responsiveness / GetComponent<Rigidbody>().velocity.magnitude * Time.deltaTime);
            player_gravity.force = newForce;
        }

        /*
        float gravity_x = Mathf.Sin(maxTilt * -input.x) * PlayerStats.instance.GetPlayerSpeed();
        float gravity_z = Mathf.Sin(maxTilt * -input.y) * PlayerStats.instance.GetPlayerSpeed();
        float gravity_y = Mathf.Cos(maxTilt * -input.x) * Mathf.Cos(maxTilt * -input.y);
        Vector3 inputForce = new Vector3(gravity_x, gravity_y, gravity_z) * gravity;
        Vector3 newForce = Vector3.zero;
        if(m_rigidbody.velocity.magnitude < 8)
        {
            m_sphereCollider.radius = Mathf.Lerp(m_sphereCollider.radius, stopping_radius, stopping_responsiveness * Time.deltaTime);
        }
        else
        {
            m_sphereCollider.radius = Mathf.Lerp(m_sphereCollider.radius, rolling_radius, direction_responsiveness * Time.deltaTime);
        }
        //======Version 3=============
        if (input != Vector2.zero)
        {
            //Creates a value between -1 and 1, plus the response offset, higher values will result when the input direction is closer to the current direction
            float direction_change = Vector3.Dot(inputForce.normalized, player_gravity.force.normalized) + response_offset;
            //Using the direction_change and direction_responsiveness to determine the smoothness of slerp
            //how close to the desired direction the player will actually move. Harder to change directions/move against your current momentum.
            newForce = Vector3.Slerp(player_gravity.force, inputForce, PlayerStats.instance.GetResponsiveness() * direction_change * direction_responsiveness * Time.deltaTime);
            player_gravity.force = newForce;
            m_rigidbody.AddTorque(new Vector3(-gravity_z, 0, gravity_x).normalized * roll_multiplier);
            //Debug.Log(m_rigidbody.angularVelocity.magnitude);
        }
        else
        {
            newForce = Vector3.Slerp(player_gravity.force, inputForce, PlayerStats.instance.GetResponsiveness() * stopping_responsiveness * Time.deltaTime);
            player_gravity.force = newForce;
        }
        //======Version 3============*/


        /*======Version 2============
        newForce = Vector3.Slerp(player_gravity.force, inputForce, smooth * Time.deltaTime);
        player_gravity.force = newForce;
        m_rigidbody.AddTorque(new Vector3(-gravity_z, 0, gravity_x).normalized);
        //========Version 2============ */

        /* =====Version 1============
        Vector2 input = GetAxisInputs() * PlayerStats.instance.playerSpeed;
        Vector3 inputForce = new Vector3(input.x, -1, input.y);
        Vector3 currentForce = GetComponent<ConstantForce>().force;
        Vector3 newForce = Vector3.Slerp(currentForce, inputForce, smooth * Time.deltaTime);
        gravity.force = new Vector3(newForce.x, -1, newForce.z);
        =======Version 1============ 


        //Quaternion r = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * smooth);
        //transform.rotation = r;
        //normal = r * new Vector3(0, 1, 0);
        ////transform.rotation = q;
        */
    }
}
