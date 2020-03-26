using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAnimation : MonoBehaviour
{

    [SerializeField] protected Transform target;
    [SerializeField] protected float rotationSpeed;



    private void Start()
    {
        rotationSpeed = Random.Range(rotationSpeed * 0.5f, rotationSpeed * 1.5f);
    }

    // Update is called once per frame
    void Update()
    {

        target.Rotate(0, rotationSpeed * 2, rotationSpeed);

    }
}
