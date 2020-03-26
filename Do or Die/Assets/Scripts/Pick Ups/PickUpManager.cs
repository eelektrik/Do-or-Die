using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{


    //Decimal chance that an enemy will drop a pick up
    [SerializeField] protected float dropChance;


    //Pool of possible drops the player can get
    [SerializeField] protected List<GameObject> itemPool;


    //Singleton
    public static PickUpManager instance;
    private void Awake()
    {
        instance = this;
    }


    public void OnEnemyDeath (Vector3 location)
    {

        if (!CalculateDropBool()) { return; }

        GameObject drop = GetRandomDrop();
        Instantiate(drop, location, Quaternion.identity);

    }

    //Does the player get a drop?
    private bool CalculateDropBool ()
    {

        float rng = Random.Range(0.0f, 1.0f);

        if (rng < dropChance) { return true; }
        else return false;

    }

    private GameObject GetRandomDrop ()
    {

        int rng = Random.Range(0, itemPool.Count);
        return itemPool[rng];

    }


}
