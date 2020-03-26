using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileManager : MonoBehaviour
{

    public EnemyProjectile enemyProjectile;


    private List<EnemyProjectile> unusedProjectiles = new List<EnemyProjectile>();
    public void AddUnusedProjectile(EnemyProjectile p) { unusedProjectiles.Add(p); }


    //Singleton
    public static EnemyProjectileManager instance;
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public EnemyProjectile RequestProjectile(Vector3 enemyPosition)
    {
        if (unusedProjectiles.Count != 0)
        {
            EnemyProjectile p = unusedProjectiles[0];
            p.gameObject.SetActive(true);
            p.transform.position = enemyPosition;

            unusedProjectiles.RemoveAt(0);
            return p;
        }
        else
        {
            return Instantiate(enemyProjectile, enemyPosition, Quaternion.identity);
        }
    }

}
