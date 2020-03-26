using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumberManager : MonoBehaviour
{


    //Prefab reference of the damage number
    [SerializeField] protected GameObject damageNumber;

       
    //Offset of where the number is displayed
    public Vector3 offset;


    private List<GameObject> pool = new List<GameObject>();
    

    //Singleton
    public static DamageNumberManager instance;
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


    //Called when enemy takes damage to be displayed
    //Could possibly be used for the player as well?
    public void DisplayDamage (Transform pos, int damage)
    {

        DamageNumber number = RequestNumber().GetComponent<DamageNumber>();


        //Set the damage number, along with its size
        number.GetText().text = damage.ToString();
        number.SetTextSize(damage);

        
        number.transform.position = pos.position + offset;
        //number.GetComponent<RectTransform>().anchoredPosition = ViewportToScreenPosition(pos.position) + offset;

        //Finally play the fade animation
        number.PlayAnimation();

    }


    GameObject RequestNumber ()
    {

        GameObject n;
        if (pool.Count == 0)
        {
            //Create new object
            n = Instantiate(damageNumber, transform);
            //Destroy(n, 5);
        }
        else
        {
            //Reuse object
            n = pool[0];
            n.SetActive(true);
            pool.Remove(n);
        }

        StartCoroutine( n.GetComponent<DamageNumber>().ObjectDuration());
        return n;

    }
    public void AddToPool (GameObject obj)
    {

        obj.SetActive(false);
        pool.Add(obj);

    }


    Vector2 ViewportToScreenPosition (Vector3 pos)
    {

        RectTransform canvasRect = GetComponent<RectTransform>();

        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(pos);
        Vector2 canvasPosition = new Vector2(
        ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

        return canvasPosition;

    }

}
