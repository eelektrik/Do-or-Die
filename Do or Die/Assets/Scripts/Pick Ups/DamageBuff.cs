using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBuff : PickUp
{


    //How long this pickup lasts
    [SerializeField] protected float buffDuration;

    //Damage bonus
    [SerializeField] protected int bonusDamage;


    protected override void OnPickUp()
    {


        //Display message to inform players
        base.OnPickUp();

        //Update the HUD to show buff is active
        PlayerHUD.instance.DisplayAttackBuff(buffDuration);

        //Play Pick up sound
        //PickUpAudio.instance.PlayPickUpDamage();

        PlayerStats.instance.BuffBonusDamage(buffDuration, bonusDamage);
        Destroy(this.gameObject);


    }


}
