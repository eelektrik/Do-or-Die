using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningManager : MonoBehaviour
{


    [SerializeField] protected float duration = 5;

    //First button manager to enable after the opening
    [SerializeField] protected ButtonManager b;


    //Singleton
    public static OpeningManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            b.Recalibrate();
            b.enabled = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CutsceneDuration());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Fades out the opening after the duration has passed
    IEnumerator CutsceneDuration ()
    {

        float timer = 0.0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        GetComponent<Animator>().Play("CutsceneEnd");

    }


    public void AllowInputs ()
    {
        b.enabled = true;
    }

}
