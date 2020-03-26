using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDodgeBehavior : MonoBehaviour
{
    [SerializeField] protected UnityEngine.AI.NavMeshAgent nma;
    [SerializeField] protected float dodgeCooldown;
    [SerializeField] protected float dodgeSpeed;
    [SerializeField] protected float dodgeRadius;
    [SerializeField] protected float maxDistance;
    [SerializeField] protected Transform enemy;

    private bool canDodge;
    private Vector3 most_recent_projectile, movement;
    private float distance;
    // Start is called before the first frame update
    void Start()
    {
        canDodge = true;
        most_recent_projectile = Vector3.zero;
        dodgeSpeed = Mathf.Sqrt(dodgeSpeed * nma.speed);
        GetComponent<SphereCollider>().radius = dodgeRadius;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Projectile proj))
        {
            most_recent_projectile = proj.GetDirection();
            if (canDodge) StartCoroutine(Dodge());
        }  

        
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Projectile p))
        {
            if (canDodge) StartCoroutine(Dodge());
        }
        
    }
    IEnumerator Dodge()
    {
        canDodge = false;
        float timer = 0f;
        StartCoroutine(Move());
        while (timer <= dodgeCooldown)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        canDodge = true;
    }

    IEnumerator Move()
    {
        movement = Vector3.Cross(most_recent_projectile.normalized, Vector3.up);
        distance = 0f;
        while (distance < maxDistance)
        {
            nma.velocity = movement * dodgeSpeed;
            distance += (movement*dodgeSpeed*Time.deltaTime).magnitude;
            yield return null;
        }
    }
}
