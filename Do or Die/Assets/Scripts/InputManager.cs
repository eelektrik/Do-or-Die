using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{


    [SerializeField] protected bool controllerInputs;

    private bool disabled = false;
    public void ToggleInputsOff() { disabled = true; }
    public void ToggleInputsOn() { disabled = false; }


    //Singleton
    public static InputManager instance;
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public Vector2 GetLeftJoystickInputs()
    {
        if (disabled) { return new Vector2(); }

        if (controllerInputs) { return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); }

        else
        {
            return new Vector2(Input.GetAxis("KeyboardHorizontal"), Input.GetAxis("KeyboardVertical"));
        }

    }
    public Vector2 GetRightJoystickInputs()
    {
        if (disabled) { return new Vector2(); }

        if (controllerInputs) { return new Vector2(Input.GetAxis("RightThumbstickX"), Input.GetAxis("RightThumbstickY")); }

        else
        {
            return new Vector2(Input.GetAxis("KeyboardRightHorizontal"), Input.GetAxis("KeyboardRightVertical"));
        }

    }




}
