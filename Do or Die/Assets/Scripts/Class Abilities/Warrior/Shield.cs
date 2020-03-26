using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : RightButtonAbility
{
    [SerializeField] protected float shieldResponsiveness;
    [SerializeField] protected float responsivenessChange;
    [SerializeField] protected float accelerationChange;
    [SerializeField] protected float maxSpeedChange;
    [SerializeField] protected int armorChange;
    [SerializeField] protected float massChange;
    [SerializeField] protected float shieldKnockback; public float getKnockback() { return shieldKnockback; }
    public GameObject shield;
    public GameObject shieldBase;
    private Rigidbody player_rigidBody;
    private Vector3 shieldForward;
    private Vector2 input;
    private bool buffed;
    // Start is called before the first frame update
    void Start()
    {
        buffed = false;
        player_rigidBody = GetComponent<Rigidbody>();
        shield = FindObjectOfType<ShieldCollision>().gameObject;
        shield.SetActive(false);
        shieldBase = FindObjectOfType<ShieldFollow>().gameObject;

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
                if (shield.activeSelf == false)     EnableShield();
                if (!buffed)                        StartBuffs();
                buttonPressed = false;
                StartCoroutine(ScreenEffects());
            }
            if (Input.GetButton(button))
            {
                meter -= Time.deltaTime;
                effectsTimer = 0; //Reset effects timer while button is held down so post processing won't end until button released.
                RotateShield();
            }
            if (depleted || buttonReleased)
            {
                if (shield.activeSelf == true)      DisableShield();
                if (buffed)                         EndBuffs();
                usable = false;
                meter = 0f;
                buttonReleased = false;
            }
        }
        
    }

    private void EnableShield()
    {
        //  enable shield object
        input = InputManager.instance.GetLeftJoystickInputs();
        shieldForward = new Vector3(input.x, 0f, input.y);
        shieldBase.transform.forward = shieldForward.normalized;
        shield.SetActive(true);  
    }

    private void RotateShield()
    {
        //  rotate shield object
        input = InputManager.instance.GetLeftJoystickInputs();
        shieldForward = new Vector3(player_rigidBody.velocity.x, 0f, player_rigidBody.velocity.z) + new Vector3(input.x, 0f, input.y);
        shieldBase.transform.forward = Vector3.Slerp(shieldBase.transform.forward, shieldForward.normalized, shieldResponsiveness * Time.deltaTime);
    }

    private void DisableShield()
    {
        //  disable shield object
        shield.SetActive(false);  
    }
    private void StartBuffs()
    {
        //  increase acceleration
        PlayerStats.instance.UpgradeAcceleration(accelerationChange);
        //  increase max speed
        PlayerStats.instance.UpgradeSpeed(maxSpeedChange);
        //  increase armor
        PlayerStats.instance.UpgradeArmor(armorChange);
        //  decrease responsiveness
        PlayerStats.instance.UpgradeResponsiveness(responsivenessChange);
        //  increase mass
        player_rigidBody.mass += massChange;
        buffed = true;
    }

    private void EndBuffs()
    {
        //  decrease acceleration
        PlayerStats.instance.UpgradeAcceleration(-accelerationChange);
        //  decrease max speed
        PlayerStats.instance.UpgradeSpeed(-maxSpeedChange);
        //  decrease armor
        PlayerStats.instance.UpgradeArmor(-armorChange);
        //  increase responsiveness
        PlayerStats.instance.UpgradeResponsiveness(-responsivenessChange);
        //  decrease mass
        player_rigidBody.mass -= massChange;
        buffed = false;
    }
}
