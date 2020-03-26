using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapManager : MonoBehaviour
{


    //Transform of the minimap GO
    [SerializeField] protected Transform minimap;
    //Image of the minimap, for scaling
    [SerializeField] protected RectTransform squareImage;

    //Level area the minimap will display
    [SerializeField] protected Transform level;

    //Transform of the player GO
    //[SerializeField] protected Transform player;
    //Dot that represents the player on the minimap
    [SerializeField] protected RectTransform playerDot;

    //Enemy dot prefab
    [SerializeField] protected RectTransform enemyDot;


    //Scale for minimap coords
    float levelScale;
    float minimapScale;

    List<GameObject> unusedEnemyDots = new List<GameObject>();
    List<GameObject> usedEnemyDots = new List<GameObject>();



    //Singleton
    public static MiniMapManager instance;
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {

        //Calculate the scale of the level for converting to minimap coords
        //Level should be a square, so just taking x should be okay
        levelScale = level.localScale.x / 2;
        minimapScale = squareImage.rect.width / 2;

    }

    // Update is called once per frame
    void Update()
    {



        //UpdatePlayerPosition();
        //UpdateEnemyPositions();
    }

    void UpdatePlayerPosition ()
    {

        //Calculate scaled player position
        Vector2 playerPos = ConvertToMinimapCoords(GameObject.FindGameObjectWithTag("Player").transform);

        playerDot.localPosition = playerPos;

    }

    public void AddEnemyDot (Transform e)
    {

        EnemyDot dot = Instantiate(enemyDot, minimap).GetComponent<EnemyDot>();

        //Set sibling index
        //Set as 1 (index 0 is the minimap itself)
        dot.transform.SetSiblingIndex(1);

        //Set values
        dot.SetLevelScale(levelScale);
        dot.SetMapScale(minimapScale);
        dot.SetTarget(e);

    }

    void UpdateEnemyPositions ()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        ResetEnemyDots();

        foreach (Enemy e in enemies)
        {

            GameObject dot = RequestEnemyDot();
            usedEnemyDots.Add(dot);
            Vector2 enemyPos = ConvertToMinimapCoords(e.transform);
            dot.GetComponent<RectTransform>().localPosition = enemyPos;

        }

    }

    GameObject RequestEnemyDot ()
    {
        
        if (unusedEnemyDots.Count != 0)
        {
            GameObject dot = unusedEnemyDots[0];
            dot.GetComponent<Image>().enabled = true;
            unusedEnemyDots.RemoveAt(0);
            return dot;
        }
        else
        {
            return Instantiate(enemyDot, minimap).gameObject;
        }

    }

    void ResetEnemyDots ()
    {
        Debug.Log("reset!");
        foreach (GameObject e in usedEnemyDots)
        {
            e.GetComponent<Image>().enabled = false;
            unusedEnemyDots.Add(e);
            usedEnemyDots.Remove(e);
        }

    }

    Vector2 ConvertToMinimapCoords (Transform obj)
    {

        Vector2 converted = new Vector2(obj.position.x, obj.position.z);

        converted /= levelScale;
        converted *= minimapScale;

        return converted;

    }

}
