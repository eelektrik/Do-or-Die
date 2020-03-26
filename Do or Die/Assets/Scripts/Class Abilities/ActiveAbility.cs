using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveAbility : MonoBehaviour
{


    //How much meter is needed to use ability
    public float meterMax;
    protected float meter;
    protected bool depleted;
    protected bool usable;
    protected bool buttonPressed;
    protected bool buttonReleased;
    [SerializeField] protected UnityEngine.Rendering.Volume abilityEffects;
    [SerializeField] protected float screenEffectsDuration;
    protected float effectsTimer;
    //Multiplier for how much meter is recharged (used for recharge pickup)
    protected float rechargeMultiplier = 1;

    //Button that is mapped to trigger skill
    [SerializeField] protected string button;

    [SerializeField] protected Sprite icon;


    void Awake()
    {
        if (abilityEffects != null) abilityEffects = Instantiate(abilityEffects);
        meter = meterMax;
        depleted = false;
        usable = true;
        buttonPressed = false;
        buttonReleased = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (usable)
        {
            if (Input.GetButtonDown(button)) buttonPressed = true;
            if (Input.GetButtonUp(button)) buttonReleased = true;
        }

        if (IsMeterFull() && !Input.GetButton(button))
        {
            usable = true;
        }
        if (depleted || !usable)
        {
            meter += Time.deltaTime * PlayerStats.instance.GetRecharge() * rechargeMultiplier;
        }

        //Handle meter going over/under
        if (meter <= 0)
        {
            meter = 0;
            depleted = true;
        }
        if (meter > meterMax)
        {
            meter = meterMax;
        }

        if (depleted && !Input.GetButton(button))
        {
            depleted = false;
        }

        
    }


    public Sprite GetIcon ()
    {
        return icon;
    }
    public float GetMeterPercentage ()
    {
        return meter / meterMax;
    }


    public void BuffAbilityRecharge (float duration)
    {
        StopAllCoroutines();
        StartCoroutine(StartRechargeBuff(duration));
    }
    IEnumerator StartRechargeBuff (float duration)
    {


        float timer = 0f;

        //How effective the buff is
        rechargeMultiplier = 2;

        //Play music speed up
        //PickUpAudio.instance.PlayFasterSpeed();

        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        rechargeMultiplier = 1;

        //Play music speed down
        //PickUpAudio.instance.PlayNormalSpeed();


    }


    //Returns TRUE if player has full meter, otherwise FALSE
    protected bool IsMeterFull ()
    {

        if (meter >= meterMax) { return true; }
        else { return false; }

    }
    protected IEnumerator ScreenEffects()
    {
        if (abilityEffects != null)
        {
            effectsTimer = 0;
            abilityEffects.gameObject.SetActive(true);
            while (effectsTimer < screenEffectsDuration)
            {
                effectsTimer += Time.deltaTime;
                yield return null;
            }
            abilityEffects.gameObject.SetActive(false);
        }
    }
    /*
    protected void DisableMovement()
    {

    }

    protected void DisableStandardShooting()
    {

    }

    protected void EnableMovement()
    {

    }

    protected void EnableStandardShooting()
    {

    }*/
}
