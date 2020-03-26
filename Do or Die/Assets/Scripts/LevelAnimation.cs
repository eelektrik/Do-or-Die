using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAnimation : MonoBehaviour
{


    private Animator a;


    //Singleton
    public static LevelAnimation instance;
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {

        //Reference calls
        a = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayAnimation ()
    {
        Debug.Log("Boom");
        //a.Play("Level Animation");
    }

}
