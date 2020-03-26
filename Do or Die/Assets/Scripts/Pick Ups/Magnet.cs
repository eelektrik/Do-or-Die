using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{


    [SerializeField] protected float magnetRange;
    [SerializeField] protected float magnetStrength;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMagnet();
    }


    void UpdateMagnet ()
    {

        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        float distance = Vector3.Distance(playerPos, transform.position);

        //Check if the player is close enough to this object
        if (distance <= magnetRange)
        {
            UpdatePosition(playerPos, magnetRange - distance);
        }

    }
    void UpdatePosition (Vector3 playerPos, float strength)
    {
        transform.position = Vector3.Slerp(transform.position, playerPos, magnetStrength * Time.deltaTime * strength);
    }

}
