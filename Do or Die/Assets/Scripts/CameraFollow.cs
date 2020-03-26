using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{


    //public Transform target;

    public float springArmLength;
    public float defaultCameraTilt;

    private Vector3 originalRotation;
    public float maxTilt;
    private float smooth;
    public void SetSmooth (float s) { smooth = s; }


    //Singleton
    public static CameraFollow instance;
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Camera>().transform.position = new Vector3(0, 0, -springArmLength);
        transform.rotation = Quaternion.Euler(defaultCameraTilt, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        originalRotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFollow();
        UpdateTilt();
    }


    void UpdateFollow ()
    {
        this.transform.position = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, this.transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
    }


    void UpdateTilt ()
    {
        //Get input to calculate camera tilt
        Vector2 input = InputManager.instance.GetLeftJoystickInputs() * maxTilt;

        //Quaternion q = Quaternion.Euler(originalRotation.x - input.y, originalRotation.y, originalRotation.z + input.x);
        Quaternion q = Quaternion.Euler(originalRotation.x - 1.5f*input.y, originalRotation.y - .5f*input.y + .5f*input.x, originalRotation.z + 1.5f*input.x);
        Quaternion r = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * smooth);
        transform.rotation = r;
    }

}
