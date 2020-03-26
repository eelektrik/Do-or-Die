using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePickUp : PickUp
{


    //How long this pickup lasts
    [SerializeField] protected float buffDuration;



    protected override void OnPickUp()
    {


        //Display message to inform players
        base.OnPickUp();

        //Update the HUD to show buff is active
        PlayerHUD.instance.DisplayFireBuff(buffDuration);

        //Play Pick up sound
        //PickUpAudio.instance.PlayPickUpFire();


        FindObjectOfType<PlayerCollision>().StartBurn(buffDuration);
        Destroy(this.gameObject);


    }


}
