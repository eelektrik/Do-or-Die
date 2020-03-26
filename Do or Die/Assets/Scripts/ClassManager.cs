using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassManager : MonoBehaviour
{


    //Handles what class the player chooses
    //Don't ever destroy; holds info needed at the start of the main game


    public enum PlayerClass { WARRIOR, MAGE, RANGER };
    private PlayerClass chosenClass;


    //Singleton
    public static ClassManager instance;
    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this.gameObject); }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ChooseClass (int c)
    {

        //0: Warrior; 1: Mage; 2: Ranger
        if (c == 0) { chosenClass = PlayerClass.WARRIOR; }
        else if (c == 1) { chosenClass = PlayerClass.MAGE; }
        else if (c == 2) { chosenClass = PlayerClass.RANGER; }

    }


    public PlayerClass GetClass () { return chosenClass; }


}
