using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumber : MonoBehaviour
{


    [SerializeField] protected int minSize;
    [SerializeField] protected int maxSize;

    [SerializeField] protected int minDamage;
    [SerializeField] protected int maxDamage;

    [SerializeField] protected Text damageNum;
    public Text GetText () { return damageNum; }


    public bool fading = false;


    // Start is called before the first frame update
    void Start()
    {

        //Reference call
        //damageNum = GetComponentInChildren<Text>();

    }


    public void PlayAnimation ()
    {

        GetComponent<Animator>().Play("NumberFade");

    }

    
    public void SetTextSize (int damage)
    {

        damageNum.fontSize = CalculateTextSize(damage);

    }
    private int CalculateTextSize (int damage)
    {

        float damageRatio = (float)(damage) / (float)(maxDamage - minDamage);
        int bonusFontSize = (int) ((maxSize - minSize) * damageRatio);

        return bonusFontSize + minSize;

    }


    public IEnumerator ObjectDuration ()
    {

        float timer = 0;
        fading = true;

        while (fading)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        Hide();

    }
    public void Hide ()
    {

        DamageNumberManager.instance.AddToPool(this.gameObject);

    }


}
