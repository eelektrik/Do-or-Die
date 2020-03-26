using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour
{


    private GameObject player;

    //Turn on one gameobject based on the player's class
    [SerializeField] protected GameObject warriorHat;
    [SerializeField] protected GameObject mageHat;
    [SerializeField] protected GameObject rangerHat;

    //How high the hat floats above 
    [SerializeField] protected float hatHeightOffset;


    // Start is called before the first frame update
    void Start()
    {
        LoadHat();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) player = FindObjectOfType<PlayerCollision>().gameObject;
        transform.position = player.transform.position + Vector3.up * hatHeightOffset;
    }


    //Call this at the beginning of the game to load the correct hat
    void LoadHat ()
    {
        if (ClassManager.instance.GetClass() == ClassManager.PlayerClass.WARRIOR)
        {
            warriorHat.SetActive(true);
        }
        else if (ClassManager.instance.GetClass() == ClassManager.PlayerClass.MAGE)
        {
            mageHat.SetActive(true);
        }
        else if (ClassManager.instance.GetClass() == ClassManager.PlayerClass.RANGER)
        {
            rangerHat.SetActive(true);
        }
    }


}
