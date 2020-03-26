using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceManager : MonoBehaviour
{

    [SerializeField] protected int maxLevel;
    private int level = 1;  public int GetLevel() { return level; }

    private int experience = 0;
    private int previousLevel = 10;
    private int toNextLevel = 25;
    private int counter = 0;


    //Bar that displays EXP
    [SerializeField] protected Image expBar;
    //Smooth time
    [SerializeField] protected float barUpdateTime;
    //The text that displays the player's current level
    [SerializeField] protected Text levelNumber;

    [SerializeField] protected float explosionRadius;


    //Singleton
    public static ExperienceManager instance;
    private void Awake()
    {
        instance = this;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GameObject.FindGameObjectWithTag("Player").transform.position, explosionRadius);
    }


    // Start is called before the first frame update
    void Start()
    {
        //Initialize the level display
        PlayerHUD.instance.UpdateLevel(level);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void LevelUp ()
    {

        //Load upgrades for the player
        if (level < maxLevel)
        {
            level++;
            experience -= toNextLevel;
            PlayerHUD.instance.UpdateLevel(level);


            UpgradeManager.instance.OnLevelUp();
            

            //Calculate XP to next level
            int temp = toNextLevel;
            if (level < 10)
            {
                toNextLevel = Mathf.RoundToInt((toNextLevel + previousLevel) *.075f) *10; //Faster leveling for testing
                //toNextLevel = Mathf.RoundToInt((toNextLevel + previousLevel) *.085f) *10;
            }
            else
            {
                toNextLevel = Mathf.RoundToInt((toNextLevel + previousLevel) * (.07f - .001f * counter)) * 10; //Faster leveling for testing
                //toNextLevel = Mathf.RoundToInt((toNextLevel + previousLevel) * (.08f - .001f * counter)) * 10;
                counter++;
            }
            previousLevel = temp;
            //Debug.Log(toNextLevel);


            //Explode
            Explosion();
            LevelAnimation.instance.PlayAnimation();

            //PlayPopSound
            //SpeedRTPCControl.instance.PlayPopSound();
        } 
    }


    private void Explosion ()
    {
        //LAYER MASK MUST INCLUDE 9 (enemies) AND 11 (enemy projectiles)
        int layerMask = 1 << 9 | 1 << 11;

        Collider[] enemies = Physics.OverlapSphere(GameObject.FindGameObjectWithTag("Player").transform.position, explosionRadius, layerMask);
        foreach (Collider e in enemies)
        {
            if (e.gameObject.GetComponent<Enemy>() != null)
            {

                //Push enemies away from the player
                //Find direction to push them in
                Vector3 pushDirection = e.transform.position - GameObject.FindGameObjectWithTag("Player").transform.position;
                pushDirection = new Vector3(pushDirection.x, 0, pushDirection.z).normalized;

                //Find how far to push them
                float pushDistance = explosionRadius - Vector3.Distance(e.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);

                //e.transform.Translate(pushDirection * pushDistance);
                //e.gameObject.GetComponent<Rigidbody>().AddForce(pushDirection, ForceMode.Impulse);
                e.gameObject.GetComponent<Enemy>().Knockback(GameObject.FindGameObjectWithTag("Player").transform.position, pushDistance);

            }
            if (e.gameObject.GetComponent<EnemyProjectile>() != null)
            {
                Destroy(e.gameObject);
            }
        }
    }


    public void GainExperience (int exp)
    {

        //Add exp
        experience += exp;

        //Check for level up
        if (experience >= toNextLevel)
        {
            LevelUp();
        }

        float percent = (float) experience / toNextLevel;
        PlayerHUD.instance.UpdateExperienceBar(percent);

    }




}
