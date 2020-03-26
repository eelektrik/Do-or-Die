/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAudio : MonoBehaviour
{

    //public AK.Wwise.Switch PickUpHealth;
    public AK.Wwise.Event PickUpScore;
    public AK.Wwise.Event PickUpSpeed;
    public AK.Wwise.Event PickUpDamage;
    public AK.Wwise.Event PickUpFire;
    public AK.Wwise.Event Clicking;
    public AK.Wwise.Event ConfirmButton;
    public AK.Wwise.Event WaveComplete;
    public AK.Wwise.Event PickUpHealth;
    public AK.Wwise.Event StartBurn;
    public AK.Wwise.Event StopBurn;



    //public AK.Wwise.Event PlayPickUps;
    public AK.Wwise.State StartingSpeed;
    public AK.Wwise.Event NormalSpeed;
    public AK.Wwise.Event FasterSpeed;




    //burningbuff
    public void PlayStartBurn()
    {
        StartBurn.Post(gameObject);
    }

    public void PlayStopBurn()
    {
        StopBurn.Post(gameObject);
    }


    //Pickups
    public void PlayPickUpHealth()
    {
        PickUpHealth.Post(gameObject);
    }

    public void PlayPickUpScore()
    {
        PickUpScore.Post(gameObject);
    }

    public void PlayPickUpSpeed()
    {
        PickUpSpeed.Post(gameObject);
    }

    public void PlayPickUpDamage()
    {
        PickUpDamage.Post(gameObject);
    }


    public void PlayPickUpFire()
    {
        PickUpFire.Post(gameObject);
    }



    public void PlayNormalSpeed()
    {
        NormalSpeed.Post(gameObject);

    }

    public void PlayFasterSpeed()
    {
        FasterSpeed.Post(gameObject);
    }


    //UI Sounds
    public void PlayClicking()
    {
        Clicking.Post(gameObject);
    }

    public void PlayConfirmButton()
    {
        ConfirmButton.Post(gameObject);
    }

    public void PlayWaveComplete()
    {
        WaveComplete.Post(gameObject);
    }



    //Singleton
    public static PickUpAudio instance;




    private void Awake()
    {
        //Singleton
        instance = this;

    }


    // Start is called before the first frame update
    void Start()
    {
        StartingSpeed.SetValue();
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/