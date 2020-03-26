using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIManager : MonoBehaviour
{


    [SerializeField] protected Text upgradeAText;
    [SerializeField] protected Text upgradeBText;
    [SerializeField] protected Text upgradeCText;


    //Singleton
    public static UpgradeUIManager instance;
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


    public void LoadUpgradeUI (string a, string b, string c)
    {
        upgradeAText.text = a;
        upgradeBText.text = b;
        upgradeCText.text = c;
    }

}
