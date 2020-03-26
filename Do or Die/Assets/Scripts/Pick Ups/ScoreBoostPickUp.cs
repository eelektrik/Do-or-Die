using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoostPickUp : PickUp
{


    //How long this pickup lasts
    [SerializeField] protected float buffDuration;


    protected override void OnPickUp()
    {


        //Display message to inform players
        base.OnPickUp();

        //Update the HUD to show buff is active
        PlayerHUD.instance.DisplayScoreBuff(buffDuration);

        //Play Pick up sound
        //PickUpAudio.instance.PlayPickUpScore();



        ScoreManager.instance.IncreaseMultiplier(buffDuration);
        Destroy(this.gameObject);


    }

}
