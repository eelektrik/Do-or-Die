using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snipe : RightButtonAbility
{
    [SerializeField] protected float time_slow_rate;    //How fast time reaches maximum slow factor
    [SerializeField] protected float time_revert_rate;  //Fow fast time reverts to normal 
    [SerializeField] protected float reticleResponsiveness;
    [SerializeField] protected SnipeProjectile projectile;
    [SerializeField] protected int die;
    public GameObject reticle;
    private Vector3 reticleForward;
    private Vector2 input;
    
    // Start is called before the first frame update
    void Start()
    {
        reticle = FindObjectOfType<Reticle>().gameObject;
        reticle.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (usable)
        {
            if (Input.GetButtonDown(button)) buttonPressed = true;
            if (Input.GetButtonUp(button)) buttonReleased = true;

            if (buttonPressed)
            {
                if (reticle.activeSelf == false)
                    DrawReticle();
                buttonPressed = false;
                StartCoroutine(ScreenEffects());
            }
            if (Input.GetButton(button))
            {
                meter -= Time.deltaTime;
                effectsTimer = 0; //Reset effects timer while button is held down so post processing won't end until button released.
                SlowTime();
                RotateReticle();
            }
            if (depleted || buttonReleased)
            {
                if (reticle.activeSelf == true)
                    EndReticle();
                FireShot();
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

    private void DrawReticle()
    {
        //  enable reticle
        input = InputManager.instance.GetRightJoystickInputs();
        reticleForward = new Vector3(input.x, 0f, input.y).normalized;
        reticle.transform.forward = reticleForward;
        reticle.SetActive(true);
    }

    private void RotateReticle()
    {
        //  rotate reticle
        input = InputManager.instance.GetRightJoystickInputs();
        reticleForward = new Vector3(input.x, 0f, input.y).normalized;
        reticle.transform.forward = Vector3.Slerp(reticle.transform.forward, reticleForward.normalized, reticleResponsiveness * Time.deltaTime);
    }

    private void SlowTime()
    {
        Time.timeScale = Mathf.Lerp(Time.timeScale, PlayerStats.instance.GetBTSlow(), time_slow_rate * Time.deltaTime);
    }
    private void EndReticle()
    {
        reticle.SetActive(false);
    }

    private void FireShot()
    {
        SnipeProjectile newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
        newProjectile.transform.forward = reticleForward;
        newProjectile.StartProjectile(reticleForward);
    }

    private void RestoreTimescale()
    {
        Time.timeScale = Mathf.Lerp(Time.timeScale, 1.0f, time_revert_rate * Time.deltaTime);
    }
}
