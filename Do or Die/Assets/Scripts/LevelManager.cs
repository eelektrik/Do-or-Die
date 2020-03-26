using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{


    //CLASS PREFABS
    //Warrior
    [SerializeField] protected GameObject warrior;
    [SerializeField] protected GameObject warriorManager;
    //Mage
    [SerializeField] protected GameObject mage;
    [SerializeField] protected GameObject mageManager;
    //Ranger
    [SerializeField] protected GameObject ranger;
    [SerializeField] protected GameObject rangerManager;

    //Where to load the player in the scene at the beginning of the game
    [SerializeField] protected Transform startingPos;


    //How long each wave should last
    private float waveDuration;
    //How long breaks between waves should last
    [SerializeField] protected float breakDuration;

    //Reference to the timer text object
    public Text timerNumber;

    //What level follows this level
    public Object nextLevel;

    //Win screen that's displayed after surviving the level
    public GameObject winScreen;
    //Lose screen displayed when the player's health hits 0
    public GameObject loseScreen;


    //Singleton
    public static LevelManager instance;
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {

        //Load player based on class they chose
        LoadPlayer();
        //Load the player's active skills
        PlayerHUD.instance.SetAbilityIcons();
        //Start the game
        StartNextWave();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void WinLevel()
    {
        Time.timeScale = 0;
        winScreen.SetActive(true);
    }
    public void LoseLevel()
    {
        Time.timeScale = 0;
        loseScreen.SetActive(true);
        loseScreen.GetComponent<LoseScreenManager>().StartLoseScreen();
    }


    //Now that we have different classes, at the start of the game we load a different character prefab based on the selected class
    void LoadPlayer ()
    {
        
        ClassManager c = FindObjectOfType<ClassManager>();

        //If we load the level from the editor, just spawn the warrior as default
        if (c == null)
        {
            Instantiate(warrior, startingPos.position, Quaternion.identity);
            Instantiate(warriorManager);
            return;
        }

        //Find the class manager and get the chosen class
        ClassManager.PlayerClass chosenClass = FindObjectOfType<ClassManager>().GetClass();

        if (chosenClass == ClassManager.PlayerClass.WARRIOR)
        {
            Instantiate(warrior, startingPos.position, Quaternion.identity);
            Instantiate(warriorManager);
        }
        else if (chosenClass == ClassManager.PlayerClass.MAGE)
        {
            Instantiate(mage, startingPos.position, Quaternion.identity);
            Instantiate(mageManager);
        }
        else if (chosenClass == ClassManager.PlayerClass.RANGER)
        {
            Instantiate(ranger, startingPos.position, Quaternion.identity);
            Instantiate(rangerManager);
        }

    }


    IEnumerator WaveTimer ()
    {

        float timer = 0.0f;

        while (timer <= waveDuration)
        {
            timer += Time.deltaTime;
            PlayerHUD.instance.UpdateWaveTimer((int)(waveDuration - timer));
            yield return null;
        }

        //Player survived the wave HERE, so play this animation
        PlayerHUD.instance.PlayWaveSurvivedAnimation();

        StartCoroutine(BreakTimer());

    }


    private void StartNextWave ()
    {
        
        EnemyWaveManager.instance.IncrementWave();

        //Update the UI for the new wave
        PlayerHUD.instance.UpdateWaveNumber();

        //Play next wave animation
        if (EnemyWaveManager.instance.GetCurrentWaveNumber() == 1)
        {
            //On the first wave (when the game starts) do something different
            PlayerHUD.instance.DisplayMessage("Game Start!");
        }
        else
        {
            PlayerHUD.instance.PlayNextWaveAnimation();
        }

        //Update wave duration, and start the timer
        waveDuration = EnemyWaveManager.instance.GetWave().GetDuration();
        StartCoroutine(WaveTimer());

        //SpeedRTPCControl.instance.NextLevelSound();  //Play Next_Level Sound.
    }

   

    IEnumerator BreakTimer ()
    {
        //PickUpAudio.instance.PlayWaveComplete();
        float timer = 0.0f;

        //Pause enemy spawning during the break
        EnemyWaveManager.instance.ToggleSpawning();
        //Now clear the old enemies from the last wave
        EnemyWaveManager.instance.ClearActiveEnemies();

        //Let's disable the timer during the break
        timerNumber.enabled = false;

        while (timer <= breakDuration)
        {
            timer += Time.deltaTime;
            //timerNumber.text = ((int)(breakDuration - timer)).ToString();
            yield return null;
        }

        //Unpause enemy spawning after the break
        EnemyWaveManager.instance.ToggleSpawning();

        //Enable timer after break
        timerNumber.enabled = true;

        StartNextWave();

    }


    public void EndGame ()
    {
        Time.timeScale = 1;
        UpdatePlayerProgress();
        //0 should be the index of the main menu
        SceneManager.LoadSceneAsync(0);
    }

    public void RestartLevel ()
    {
        Time.timeScale = 1;
        UpdatePlayerProgress();
        SceneManager.LoadSceneAsync(1);
    }

    private void UpdatePlayerProgress ()
    {

        ProgressManager.instance.InputResult(ScoreManager.instance);
        SaveManager.SaveData();

    }


}
