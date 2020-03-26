using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallManager : MonoBehaviour
{


    //[SerializeField] protected Transform player;

    private float fallThreshold = -5f;
    private bool fallen = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        CheckForFall();

    }


    private void CheckForFall ()
    {

        float height = GameObject.FindGameObjectWithTag("Player").transform.position.y;

        if (height <= fallThreshold)
        {

            if(fallen) { return; }
            PlayerStats.instance.ModifyHealth(-20);
            fallen = true;

        }

    }

}
