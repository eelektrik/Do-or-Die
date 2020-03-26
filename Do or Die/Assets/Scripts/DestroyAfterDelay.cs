using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{


    //Destroy the gameobject this is attached to after some seconds
    //Used for lava


    [SerializeField] protected float delay;


    // Start is called before the first frame update
    void Start()
    {

        Destroy(this.gameObject, delay);

    }


}
