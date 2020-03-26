using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{


    //How long the pick up stays to be picked up
    [SerializeField] protected float lifeDuration = 30;
    //Message displayed when picked up
    [SerializeField] protected string message;


    private void Start()
    {

        StartCoroutine(PickUpDuration());

    }


    protected void OnTriggerEnter(Collider other)
    {

        //Only can be picked up by the player
        if (other.tag != "Player") { return; }

        OnPickUp();

    }


    protected virtual void OnPickUp ()
    {

        DisplayMessage();

    }


    void DisplayMessage ()
    {
        PlayerHUD.instance.DisplayMessage(message);
    }


    protected IEnumerator LateDestroy ()
    {

        float timer = 0f;

        while (timer < 1)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(transform.parent.gameObject);

    }

    IEnumerator PickUpDuration ()
    {

        bool flashing = false;
        float timer = 0f;

        while (timer < lifeDuration)
        {
            timer += Time.deltaTime;

            //Flash when the pickup will die soon
            if (timer > lifeDuration * 0.75f && !flashing)
            {
                Debug.Log(timer);
                Debug.Log(lifeDuration * 0.8f);
                GetComponent<Animator>().Play("Flash", 1);
                flashing = true;
            }

            yield return null;
        }


        Destroy(this.gameObject);

    }

}
