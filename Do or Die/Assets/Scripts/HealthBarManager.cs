using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{


    public GameObject healthBar;


    //Singleton
    public static HealthBarManager instance;
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


    /// <summary>
    /// Set up the health bar for an enemy when it's spawned
    /// Right now, this is only called for enemies
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public HealthBar SetupHealthBar (Enemy e)
    {
        GameObject hb = Instantiate(healthBar, e.transform.position, Quaternion.identity);
        hb.GetComponent<HealthBar>().AssignEnemy(e);

        return hb.GetComponent<HealthBar>();
    }

}
