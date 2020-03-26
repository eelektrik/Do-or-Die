using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPickUp : PickUp
{


    //How long this pickup lasts
    [SerializeField] protected float buffDuration;

    //Damage bonus
    [SerializeField] protected float speedMultiplier;


    protected override void OnPickUp()
    {


        //Display message to inform players
        base.OnPickUp();

        //Update the HUD to show buff is active
        PlayerHUD.instance.DisplaySpeedBuff(buffDuration);





        PlayerStats.instance.BuffSpeed(buffDuration, speedMultiplier);
        Destroy(this.gameObject);


    }


}
