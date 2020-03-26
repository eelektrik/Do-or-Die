using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFallManager : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {

        //Check if the object that fell off is an enemy
        Enemy e = other.GetComponent<Enemy>();
        if (e != null)
        {
            //If it is, simply kill it
            e.FallDeath();
        }

    }


}
