using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{


    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;

    //WHEN PLAYER FIRES
    // How long the object should shake for.
    [SerializeField] protected float fireShakeDuration = 1.0f;
    // Amplitude of the shake. A larger value shakes the camera harder.
    [SerializeField] protected float fireShakeAmount = 0.7f;

    //WHEN THE PLAYER GETS HIT
    // How long the object should shake for.
    [SerializeField] protected float damagedShakeDuration = 1.0f;
    // Amplitude of the shake. A larger value shakes the camera harder.
    [SerializeField] protected float damagedShakeAmount = 2f;
    [SerializeField] protected Animator damageFlash;

    Vector3 originalPos;


    //Singleton
    public static CameraShake instance;
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        //FireShake();
        originalPos = camTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Shake to be done when the player fires
    public void FireShake ()
    {

        originalPos = camTransform.localPosition;
        StartCoroutine(Shake(fireShakeDuration, fireShakeAmount));

    }
    public void OnDamageShake ()
    {

        originalPos = camTransform.localPosition;
        StartCoroutine(Shake(damagedShakeDuration, damagedShakeAmount));

        //Damage flash
        damageFlash.Play("PlayerDamageFlash");

    }


    public IEnumerator Shake (float shakeDuration, float shakeAmount)
    {

        float counter = 0f;

        while (counter < shakeDuration)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            counter += Time.deltaTime;
            yield return null;
        }

        camTransform.localPosition = originalPos;

    }


    public void StopShake ()
    {

        StopAllCoroutines();
        //StopCoroutine(Shake());
        Camera.main.transform.localPosition = originalPos;

    }

}
