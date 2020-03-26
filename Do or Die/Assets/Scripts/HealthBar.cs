using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{


    private Transform target;
    private Enemy enemy;

    public float updateTime;
    public Image bar;

    public Vector3 offset;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy)
        {
            transform.position = target.position + offset;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public IEnumerator UpdateHealthBar (float perc)
    {

        float origFill = bar.fillAmount;
        float timer = 0f;


        while (timer < updateTime)
        {
            timer += Time.deltaTime;
            if (bar == null) { yield break; }
            bar.fillAmount = Mathf.Lerp(origFill, perc, timer / updateTime);
            yield return null;
        }

        bar.fillAmount = perc;

    }


    public void AssignEnemy (Enemy e)
    {
        target = e.transform;
        enemy = e;
    }

}
