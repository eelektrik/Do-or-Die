using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldFollow : MonoBehaviour
{
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>().transform;
            if (!player.TryGetComponent<Shield>(out Shield shield))
            {
                gameObject.SetActive(false);
            }
        }

        transform.position = player.position;
    }
}
