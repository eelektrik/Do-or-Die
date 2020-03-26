using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionPickUp : PickUp
{
    [SerializeField] protected int healthAmount;


    protected override void OnPickUp()
    {


        //Display message to inform players
        base.OnPickUp();

        //Play Pick up sound
        //PickUpAudio.instance.PlayPickUpHealth();

        PlayerStats.instance.ModifyHealth(healthAmount);
        Destroy(this.gameObject);


    }
}
