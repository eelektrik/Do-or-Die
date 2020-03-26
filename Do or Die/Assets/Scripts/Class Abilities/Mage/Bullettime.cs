using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullettime : LeftButtonAbility
{
    [SerializeField] protected float time_slow_rate;    //How fast time reaches maximum slow factor
    [SerializeField] protected float time_revert_rate;  //Fow fast time reverts to normal 
    [SerializeField] protected float responsivenessChange;
    [SerializeField] protected float accelerationChange;
    [SerializeField] protected float maxSpeedChange;
    [SerializeField] protected int fireRateChange;

    private bool buffed;

    private void Start()
    {
        buffed = false;
    }
    void FixedUpdate()
    {
        if (usable)
        {
            if (Input.GetButtonDown(button)) buttonPressed = true;
            if (Input.GetButtonUp(button)) buttonReleased = true;
            if (buttonPressed)
            {
                //Set stat bonuses
                StartCoroutine(ScreenEffects());
                if (!buffed) StartBuffs();
                buttonPressed = false;
            }
            if (Input.GetButton(button))
            {
                meter -= Time.deltaTime;
                effectsTimer = 0; //Reset effects timer while button is held down so post processing won't end until button released.
                SlowTime();
                
            }
            if (depleted || buttonReleased)
            {
                //Unset stat bonuses
                if (buffed) EndBuffs();
                meter = 0f;
                usable = false;
                buttonReleased = false;
            }
        }

        if (!usable && Time.timeScale != 1.0f && Time.timeScale != 0.0f)
        {
            RestoreTimescale();
        }
    }

    private void SlowTime()
    {
        Time.timeScale = Mathf.Lerp(Time.timeScale, PlayerStats.instance.GetBTSlow(), time_slow_rate * Time.deltaTime);
    }

    private void RestoreTimescale()
    {
        Time.timeScale = Mathf.Lerp(Time.timeScale, 1.0f, time_revert_rate * Time.deltaTime);
    }

    private void StartBuffs()
    {
        //  increase acceleration
        PlayerStats.instance.UpgradeAcceleration(accelerationChange);
        //  increase max speed
        PlayerStats.instance.UpgradeSpeed(maxSpeedChange);
        //  increase responsiveness
        PlayerStats.instance.UpgradeResponsiveness(responsivenessChange);
        //  increase FireRate
        PlayerStats.instance.UpgradeFireRate(fireRateChange);
        buffed = true;
    }

    private void EndBuffs()
    {
        //  decrease acceleration
        PlayerStats.instance.UpgradeAcceleration(-accelerationChange);
        //  decrease max speed
        PlayerStats.instance.UpgradeSpeed(-maxSpeedChange);
        //  decrease responsiveness
        PlayerStats.instance.UpgradeResponsiveness(-responsivenessChange);
        //  decrease FireRate
        PlayerStats.instance.UpgradeFireRate(-fireRateChange);
        buffed = false;
    }
}
