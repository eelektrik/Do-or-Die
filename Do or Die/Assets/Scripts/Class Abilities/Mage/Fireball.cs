using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : RightButtonAbility
{


    [SerializeField] protected GameObject fireball;

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
                CastFireball();
                meter = 0;
                usable = false;
                buttonPressed = false;
            }
        }  
    }


    private void CastFireball ()
    {
        FireballObject f = Instantiate(fireball, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity).GetComponent<FireballObject>();
        f.SetFireball(InputManager.instance.GetRightJoystickInputs());
    }


}
