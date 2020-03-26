using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDeath : MonoBehaviour
{


    //Put this on an enemy to have it spawn a gameobject(s?) on death


    [SerializeField] protected GameObject toSpawn;


    public void Spawn ()
    {

        Vector3 spawnLocation = new Vector3(transform.position.x, toSpawn.transform.position.y + transform.position.y - 1.3f, transform.position.z);
        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        Instantiate(toSpawn, spawnLocation, randomRotation);

    }


}
