using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : LeftButtonAbility
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
                Vector3 dash_vector = m_rigidbody.velocity;
                dash_vector.y = 0;
                dash_vector = dash_vector.normalized;
                m_rigidbody.AddForce(dash_vector * PlayerStats.instance.GetDashForce());
                m_rigidbody.AddTorque(m_rigidbody.angularVelocity * PlayerStats.instance.GetDashForce());
                meter = 0f;
                usable = false;
                buttonPressed = false;
            }
        }
    }
}
