using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : LeftButtonAbility
{
    private Rigidbody m_rigidbody;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (usable)
        {
            if (Input.GetButtonDown(button)) buttonPressed = true;
            //if (Input.GetButtonUp(button)) buttonReleased = true;

            if (buttonPressed)
            {
                StartCoroutine(ScreenEffects());
                m_rigidbody.AddForce(Vector3.up * PlayerStats.instance.GetJumpForce());
                m_rigidbody.AddTorque(m_rigidbody.angularVelocity * PlayerStats.instance.GetJumpForce());
                meter = 0f;
                usable = false;
                buttonPressed = false;
            }
        }
    }
}
