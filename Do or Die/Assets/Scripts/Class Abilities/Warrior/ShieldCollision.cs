using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollision : MonoBehaviour
{
    private float shieldKnockback; public float getKnockback() { return shieldKnockback; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shieldKnockback == 0)
        {
            shieldKnockback = FindObjectOfType<PlayerCollision>().GetComponent<Shield>().getKnockback();
            Debug.Log(shieldKnockback);
        }
    }
}
