using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUpgrade : MonoBehaviour
{


    [SerializeField] protected string upgradeName;
    public string GetName () { return upgradeName; }
    [SerializeField] protected Sprite icon;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public virtual void UpgradePlayer ()
    {

    }

}
