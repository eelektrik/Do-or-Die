using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeManager : MonoBehaviour
{
    public EnemyMelee enemyMelee;
    private List<EnemyMelee> unusedProjectiles = new List<EnemyMelee>();
    public void AddUnusedProjectile(EnemyMelee p) { unusedProjectiles.Add(p); }

    //Separate manager for melee strikes, as melees will use sprites instead of meshes
    //Singleton
    public static EnemyMeleeManager instance;
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


    public EnemyMelee RequestProjectile(Vector3 enemyPosition, Quaternion enemyRotation)
    {
        if (unusedProjectiles.Count != 0)
        {
            EnemyMelee p = unusedProjectiles[0];
            p.gameObject.SetActive(true);
            p.transform.position = enemyPosition;

            unusedProjectiles.RemoveAt(0);
            return p;
        }
        else
        {
            return Instantiate(enemyMelee, enemyPosition, enemyRotation);
        }
    }

}
