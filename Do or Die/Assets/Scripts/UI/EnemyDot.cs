using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDot : MonoBehaviour
{


    private Transform targetEnemy;

    //Scale for minimap coords
    float levelScale;
    public void SetLevelScale (float s) { levelScale = s; }
    float minimapScale;
    public void SetMapScale(float s) { minimapScale = s; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForDeath();
        UpdateDotPosition();
    }

    void UpdateDotPosition ()
    {

        //Calculate scaled player position
        Vector2 pos = ConvertToMinimapCoords(targetEnemy);

        GetComponent<RectTransform>().localPosition = new Vector3 (pos.x, pos.y, 0);

    }


    public void SetTarget(Transform t)
    {

        targetEnemy = t;
        UpdateDotPosition();
        GetComponent<Image>().enabled = true;

    }


    Vector2 ConvertToMinimapCoords(Transform obj)
    {

        if (obj == null) return new Vector2(0,0);

        Vector2 converted = new Vector2(obj.position.x, obj.position.z);

        converted /= levelScale;
        converted *= minimapScale;

        return converted;

    }


    void CheckForDeath ()
    {

        //If the target is null, enemy has died
        if (targetEnemy == null)
        {
            Destroy(gameObject);
            Destroy(this);
        }

    }

}
