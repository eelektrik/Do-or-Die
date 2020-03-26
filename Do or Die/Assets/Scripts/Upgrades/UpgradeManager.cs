using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    //TODO: SUPPORT 4 BUTTONS IN UI FOR UPGRADE CHOICES
    private Queue<BaseUpgrade> upgradeQueue = new Queue<BaseUpgrade>();
    //Bool of whether there are available upgrades for the player
    private bool canUpgrade = false;

    //Pool of upgrades that can still be chosen from
    [SerializeField] protected List<BaseUpgrade> tier_1_upgrades = new List<BaseUpgrade>();
    [SerializeField] protected List<BaseUpgrade> tier_2_upgrades = new List<BaseUpgrade>();
    [SerializeField] protected List<BaseUpgrade> tier_3_upgrades = new List<BaseUpgrade>();
    [SerializeField] protected List<BaseUpgrade> attributes = new List<BaseUpgrade>();
    [SerializeField] protected List<BaseUpgrade> movement = new List<BaseUpgrade>();


    private static List<string>[] level_chart = { new List<string> { "Tier 1" },                //Level 2
                                                  new List<string> { "Movement" },              //Level 3
                                                  new List<string> { "Attribute" },             //Level 4
                                                  new List<string> { "Tier 1",      "Add d8" }, //Level 5
                                                  new List<string> { "Tier 1" },                //Level 6
                                                  new List<string> { "Tier 1" },                //Level 7
                                                  new List<string> { "Attribute" },             //Level 8
                                                  new List<string> { "Tier 2" },                //Level 9
                                                  new List<string> { "Tier 2",      "Add d10" },//Level 10
                                                  new List<string> { "Tier 2" },                //Level 11
                                                  new List<string> { "Attribute" },             //Level 12
                                                  new List<string> { "Tier 2" },                //Level 13
                                                  new List<string> { "Tier 3" },                //Level 14
                                                  new List<string> { "Attribute",   "Add d12" },//Level 15
                                                  new List<string> { "Tier 3" },                //Level 16
                                                  new List<string> { "Tier 3" },                //Level 17
                                                  new List<string> { "Attribute" },             //Level 18
                                                  new List<string> { "Tier 3" },                //Level 19
                                                  new List<string> { "Tier 3" },                //Level 20
    };
    //Level chart stores strings for what upgrades are available at what levels, stored as lists for levels where you earn multiple things

    //List of upgrades the player has used
    //private List<BaseUpgrade> usedUpgrades = new List<BaseUpgrade>();
    //List of upgrades the player has chosen to not use
    private List<BaseUpgrade> recycledUpgrades = new List<BaseUpgrade>();


    //The 3 different upgrades the player can choose from after leveling up
    private BaseUpgrade upgradeA;
    private BaseUpgrade upgradeB;
    private BaseUpgrade upgradeC;
    //private BaseUpgrade upgradeD;



    //Singleton
    public static UpgradeManager instance;
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
        CheckForInput();
    }


    private void CheckForInput ()
    {

        if (!canUpgrade) { return; }

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetButtonDown("X")) { /*SpeedRTPCControl.instance.PlayUpgrade(); /*sound*/ ChooseUpgradeA(); }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetButtonDown("Y")) { /*SpeedRTPCControl.instance.PlayUpgrade(); /*sound*/ ChooseUpgradeB(); } 
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetButtonDown("B")) { /*SpeedRTPCControl.instance.PlayUpgrade(); /*sound*/ ChooseUpgradeC(); }
        //if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetButtonDown("A")) { ChooseUpgradeD(); }

    }
    private void UpgradePlayer (BaseUpgrade upgrade)
    {

        //Store the upgrade as used
        //usedUpgrades.Add(upgrade);

        //Use the upgrade
        Debug.Log(upgrade.GetName());
        upgrade.UpgradePlayer();

        //Reset upgrades
        upgradeA = null;
        upgradeB = null;
        upgradeC = null;
        //upgradeD = null;
        canUpgrade = false;
        PlayerHUD.instance.ResetUpgrades();

        //Check to see if there are more upgrades queue'd up
        LoadUpgrades();

    }


    public void OnLevelUp ()
    {
        if(ExperienceManager.instance.GetLevel() % PlayerStats.instance.GetDice()[0] == 0)
        {
            PlayerStats.instance.AddDie(PlayerStats.instance.GetDice()[0]);
        }
        //Need to choose 3 different upgrades for the player to choose from
        //Right now, upgrades will be RANDOM, and if not chosen, won't be loaded until upgrades are recycled
        /*foreach (string reward in level_chart[ExperienceManager.instance.GetLevel()-2]) //First level upgrade is at level 2, stored in index 0
        {
            switch (reward)
            {
                case "Tier 1":
                    upgradeQueue.Enqueue(ChooseRandomUpgrade(tier_1_upgrades));
                    upgradeQueue.Enqueue(ChooseRandomUpgrade(tier_1_upgrades));
                    upgradeQueue.Enqueue(ChooseRandomUpgrade(tier_1_upgrades));
                    //upgradeQueue.Enqueue(ChooseRandomUpgrade(tier_1_upgrades));
                    RecycleUpgrades(tier_1_upgrades);
                    break;
                case "Tier 2":
                    upgradeQueue.Enqueue(ChooseRandomUpgrade(tier_2_upgrades));
                    upgradeQueue.Enqueue(ChooseRandomUpgrade(tier_2_upgrades));
                    upgradeQueue.Enqueue(ChooseRandomUpgrade(tier_2_upgrades));
                    //upgradeQueue.Enqueue(ChooseRandomUpgrade(tier_2_upgrades));
                    RecycleUpgrades(tier_2_upgrades);
                    break;
                case "Tier 3":
                    upgradeQueue.Enqueue(ChooseRandomUpgrade(tier_3_upgrades));
                    upgradeQueue.Enqueue(ChooseRandomUpgrade(tier_3_upgrades));
                    upgradeQueue.Enqueue(ChooseRandomUpgrade(tier_3_upgrades));
                    //upgradeQueue.Enqueue(ChooseRandomUpgrade(tier_3_upgrades));
                    RecycleUpgrades(tier_3_upgrades);
                    break;
                case "Attribute":
                    //TODO Make not random, load in order listed on the side once 4 button upgrade choices is implemented
                    upgradeQueue.Enqueue(ChooseRandomUpgrade(attributes));      //Focus
                    upgradeQueue.Enqueue(ChooseRandomUpgrade(attributes));      //Alacrity
                    upgradeQueue.Enqueue(ChooseRandomUpgrade(attributes));      //Strength
                    //upgradeQueue.Enqueue(ChooseRandomUpgrade(attributes));    //Endurance
                    RecycleUpgrades(attributes); //Remove after randomness fixed
                    break;
                case "Add d6":
                    PlayerStats.instance.AddDie(6);
                    break;
                case "Add d8":
                    PlayerStats.instance.AddDie(8);
                    break;
                case "Add d10":
                    PlayerStats.instance.AddDie(10);
                    break;
                case "Add d12":
                    PlayerStats.instance.AddDie(12);
                    break;
                case "Movement":
                    upgradeQueue.Enqueue(movement[0]);
                    upgradeQueue.Enqueue(movement[1]);
                    upgradeQueue.Enqueue(movement[2]);
                    RecycleUpgrades(movement);  //Only needed to clean out the recycled upgrades list, movement abilities are never chosen again
                    break;
            
        }
        
        
        if (!canUpgrade)
        {
            LoadUpgrades();
        }
        */
    }

    private BaseUpgrade ChooseRandomUpgrade (List<BaseUpgrade> availableUpgrades)
    {

        //Before choosing, if the upgrade pool is empty, recycle declined upgrades
        //if (availableUpgrades.Count == 0)   availableUpgrades = recycledUpgrades;

        //Choose a random upgrade from those available, and remove it from the list
        int rng = Random.Range(0, availableUpgrades.Count);
        BaseUpgrade upgrade = availableUpgrades[rng];
        recycledUpgrades.Add(upgrade);
        availableUpgrades.Remove(upgrade);
        return upgrade;

    }

    private void RecycleUpgrades(List<BaseUpgrade> availableUpgrades)
    {
        foreach(BaseUpgrade upgrade in recycledUpgrades)
        {
            availableUpgrades.Add(upgrade);
        }
        recycledUpgrades.Clear();
    }

    private void LoadUpgrades ()
    {

        if (upgradeQueue.Count == 0) { return; }

        canUpgrade = true;

  


        upgradeA = upgradeQueue.Dequeue();
        upgradeB = upgradeQueue.Dequeue();
        upgradeC = upgradeQueue.Dequeue();
        //upgradeD = upgradeQueue.Dequeue();

        PlayerHUD.instance.LoadUpgradeUI(upgradeA.GetName(), upgradeB.GetName(), upgradeC.GetName());
        //PlayerHUD.instance.LoadUpgradeUI(upgradeA.GetName(), upgradeB.GetName(), upgradeC.GetName(), upgradeD.GetName());

        //Play New_Skills Sound.
        //SpeedRTPCControl.instance.PlayNewSkill();
    }

    private void ChooseUpgradeA()
    {
        //recycledUpgrades.Add(upgradeB);
        //recycledUpgrades.Add(upgradeC);
        UpgradePlayer(upgradeA);
    }
    private void ChooseUpgradeB()
    {
        //recycledUpgrades.Add(upgradeA);
        //recycledUpgrades.Add(upgradeC);
        UpgradePlayer(upgradeB);
    }
    private void ChooseUpgradeC()
    {
        //recycledUpgrades.Add(upgradeA);
        //recycledUpgrades.Add(upgradeB);
        UpgradePlayer(upgradeC);
    }

    /*private void ChooseUpgradeD()
    {
        UpgradePlayer(upgradeD);
    }*/
}
