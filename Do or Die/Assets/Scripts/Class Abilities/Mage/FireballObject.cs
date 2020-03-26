using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballObject : MonoBehaviour
{


    //Game object to spawn after collision
    [SerializeField] protected GameObject miniBalls;

    //Direction the fireball is set to travel in
    private Vector3 direction = Vector3.zero;

    //Speed that the fireball travels at
    [SerializeField] protected int damage;

    //Speed that the fireball travels at
    [SerializeField] protected float speed;

    //Should this fireball spawn more fireballs?
    private bool multiply;

    //How many mini fireballs to spawn on collision
    [SerializeField] protected int miniballsNum;

    //How high the fireball should hover above the ground
    float hoverHeight = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }


    private void OnTriggerEnter (Collider other)
    {

        //Debug.Log(other.name);

        Enemy e = other.GetComponent<Enemy>();
        if (e != null)
        {
            //If this hits an enemy
            //Damage the enemy
            e.TakeDamage(damage);
            SpawnMiniballs();
        }
        else if (other.tag == "Wall")
        {
            //If this collides with a wall
            SpawnMiniballs();
        }

    }


    void SpawnMiniballs()
    {

        Destroy(gameObject);

        if (!multiply) { return; }

        float degreeInterval = 360.0f / (float) miniballsNum;

        for (int i = 0; i < miniballsNum; i++)
        {

            //Spawn a mini fireball in a surrounding direction
            float angle = degreeInterval * i;
            Vector3 ballDir = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle)).normalized;
            //Debug.Log(ballDir);
            FireballObject f = Instantiate(miniBalls, transform.position, Quaternion.identity).GetComponent<FireballObject>();
            f.SetFireball(new Vector2(ballDir.x, ballDir.z), false);

        }

    }


    public void SetFireball (Vector2 dir, bool m = true)
    {

        multiply = m;
        direction = new Vector3(dir.x, 0 ,dir.y);

        //Set the hover height
        RaycastHit hit;
        int floorMask = 1 << 14;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, floorMask))
        {
            hoverHeight = hit.distance;
        }


    }

    private void UpdatePosition()
    {

        transform.position = transform.position + direction * speed * Time.deltaTime;

        //Have the fireball hover over the floor
        RaycastHit hit;
        int floorMask = 1 << 14;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, floorMask))
        {
            transform.Translate(0, (hoverHeight - hit.distance), 0);
        }

    }

}
