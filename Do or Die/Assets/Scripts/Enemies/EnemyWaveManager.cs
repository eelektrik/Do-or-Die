using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyWaveManager : MonoBehaviour
{


    //List of waves in the game
    [SerializeField] protected List<EnemyWave> waves;

    EnemyWave currentWave;
    public EnemyWave GetWave () { return currentWave; }

    private int waveCounter = -1;
    [SerializeField] protected int maxWave;
    //Bool of whether we can spawn right now
    private bool canSpawn = true;

    //Reference to the level, so we can find where to spawn
    [SerializeField] protected GameObject level;
    private float xBound;
    private float zBound;

    //Current number of active enemies
    private int activeEnemiesNum;
    //List of all active enemies
    private List<Enemy> activeEnemies = new List<Enemy>();

    //Reference to the location of the player, for spawning
    //public Transform player;
    //How far away enemies should be spawned from the player
    [SerializeField] protected float spawnDistance;

    //Delay before spawning the next enemy
    [SerializeField] protected float spawnCooldown;
    private float spawnTimer;


    //Singleton
    public static EnemyWaveManager instance;
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {

        //Spawning boundaries of where enemies can be spawned
        SetSpawnBounds();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateWave();
    }



    //Call this when any enemy dies
    public void OnEnemyDeath ()
    {
        activeEnemiesNum--;
    }



    private void UpdateWave ()
    {

        if (!canSpawn) { return; }


        if (spawnTimer < spawnCooldown) {
            spawnTimer += Time.deltaTime;
            return;
        }


        if (activeEnemiesNum < currentWave.GetMaxEnemies())
        {

            //If there aren't enough enemies deployed, then spawn a new one
            Enemy e = GetRandomEnemyType();
            Enemy spawnedEnemy = Instantiate(e, GetSpawnLocation() + new Vector3(0, 3, 0), Quaternion.identity);
            activeEnemiesNum++;

            //Add the enemy to the list of active enemies
            activeEnemies.Add(spawnedEnemy);
            Debug.Log(activeEnemies.Count);

            //Play Enemy_Spawn Sound    
            //SpeedRTPCControl.instance.PlayEnemySpawn();   //currently with no spatial position...

            //Also reset the spawn cooldown
            spawnTimer = 0;

        }

    }


    public void ClearActiveEnemies ()
    {

        foreach (Enemy e in activeEnemies)
        {
            if (e != null)
            {

                //Play enemy's clear animation
                //e.ClearEnemy();
                StartCoroutine(DelayedClear(e));

            }
        }

        //Reset the enemy list
        activeEnemies.Clear();
        //Reset active enemy count
        activeEnemiesNum = 0;

    }
    //Delayed clear for juiciness
    IEnumerator DelayedClear (Enemy e)
    {

        float timer = 0;
        //Random range of possible delay times
        float delay = Random.Range(0.0f, 0.5f);

        //Destroy the health bar
        e.DestroyHealthBar();

        while (timer <= delay)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        e.GetComponent<Animator>().Play("Clear");

    }


    public void IncrementWave ()
    {
       
        waveCounter++;

        if (waveCounter < maxWave)
        { 
            currentWave = waves[waveCounter];
        }
        else
        {
            currentWave.GetComponent<EnemyWave>().IncrementMaxEnemies();
        }
        

    }



    private Vector3 GetSpawnLocation ()
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
    private Vector3 GetRandomPositionAwayFromPlayer ()
    {

        Vector3 randomDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized;
        Vector3 randomPosition = GameObject.FindGameObjectWithTag("Player").transform.position + (randomDirection * spawnDistance);

        return new Vector3(randomPosition.x, 0, randomPosition.z);

    }
    private bool IsOnLevel (Vector3 position)
    {

        if (position.x <= xBound && position.x >= -xBound &&
            position.z <= zBound && position.z >= -zBound)
        {
            return true;
        }

        return false;

    }


    //Returns a random enemy type from the types listed for this level
    private Enemy GetRandomEnemyType ()
    {

        return currentWave.GetEnemyTypes()[Random.Range(0, currentWave.GetEnemyTypes().Count)];

    }


    public int GetClearedWaves()
    {
        int cleared = waveCounter;
        return cleared;
    }
    public int GetCurrentWaveNumber()
    {
        int cleared = waveCounter + 1;
        return cleared;
    }


    public void ToggleSpawning () { canSpawn = !canSpawn; }


    //Set the spawning boundaries, based on the size of the level
    private void SetSpawnBounds ()
    {

        //Minus 1 to keep things from spawning on the very edge of the level
        xBound = (level.transform.localScale.x / 2) - 1;
        zBound = (level.transform.localScale.z / 2) - 1;

    }
    public Vector2 GetSpawnBounds ()
    {

        return new Vector2(xBound, zBound);

    }

}
