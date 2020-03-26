using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarpingEnemy : Enemy
{


    //Level transform, to know where to warp
    [SerializeField] protected Transform level;

    //If the distance between the player and enemy is greater than this, the enemy will warp
    [SerializeField] protected float warpTriggerDistance;
    //How far away the enemy will warp from the player
    [SerializeField] protected float warpDistance;
    //Cooldown between warping
    [SerializeField] protected float warpCooldown;
    //Warp cooldown counter
    private float warpCounter = 0;


    // Update is called once per frame
    void Update()
    {

        if (actionable)
        {
            CheckForWarp();
            MoveOrAttack();
        }

    }


    void CheckForWarp ()
    {

        //If warp is on cooldown, don't do anything
        if (warpCounter >= 0)
        {
            warpCounter--;
            return;
        }

        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance >= warpTriggerDistance)
        {
            //Far enough to warp!
            warpCounter = warpCooldown;
            GetComponent<Animator>().Play("Warp Up");
        }

    }
    public void Warp ()
    {

        GetComponent<Animator>().Play("Warp Down");
        transform.position = GetSpawnLocation();

    }


    private Vector3 GetSpawnLocation()
    {

        while (true)
        {

            Vector3 randomPosition = GetRandomPositionAwayFromPlayer();
            if (IsOnLevel(randomPosition))
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPosition, out hit, 3.0f, NavMesh.AllAreas)) return hit.position;
            }

        }

    }
    private Vector3 GetRandomPositionAwayFromPlayer()
    {

        Vector3 randomDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized;
        Vector3 randomPosition = GameObject.FindGameObjectWithTag("Player").transform.position + (randomDirection * warpDistance);

        return new Vector3(randomPosition.x, 0, randomPosition.z);

    }
    private bool IsOnLevel(Vector3 position)
    {

        //Minus 1 to keep things from spawning on the very edge of the level
        Vector2 bounds = EnemyWaveManager.instance.GetSpawnBounds();
        float xBound = bounds.x;
        float zBound = bounds.y;

        if (position.x <= xBound && position.x >= -xBound &&
            position.z <= zBound && position.z >= -zBound)
        {
            return true;
        }

        return false;

    }


}
