using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{


    [SerializeField] protected Text health;
    //Health vial to fill
    [SerializeField] protected Image healthVial;


    //Ability bar UI components
    [SerializeField] protected Image leftIcon;
    [SerializeField] protected Image rightIcon;
    [SerializeField] protected Image leftBar;
    [SerializeField] protected Image rightBar;



    //THESE ARE NOT USED
    //The text that displays the player's current level
    [SerializeField] protected Text levelNumber;

    //Bar that displays EXP
    [SerializeField] protected Image expBar;
    //EXP bar smooth time
    [SerializeField] protected float barUpdateTime;

    [SerializeField] protected Text upgradeAText;
    [SerializeField] protected Text upgradeBText;
    [SerializeField] protected Text upgradeCText;
    //THESE ARE NOT USED



    //Wave timer
    [SerializeField] protected Text WaveTimer;
    //The current wave player is on
    [SerializeField] protected Text waveNumber;

    //Animation for surviving wave
    [SerializeField] protected Animator waveSurvived;
    //Animation for next wave
    [SerializeField] protected Animator nextWave;

    //Reference to the current score text
    [SerializeField] protected Text currentScore;
    //Reference to the high score text; current score will be managed by the Score Manager
    [SerializeField] protected Text highScore;


    [SerializeField] protected Animator displayMessage;
    //Reference to the message display text
    [SerializeField] protected Text messageText;


    //UI for pickups
    [SerializeField] protected Image scoreBuff;
    [SerializeField] protected Image attackBuff;
    [SerializeField] protected Image speedBuff;
    [SerializeField] protected Image rechargeBuff;
    [SerializeField] protected Image fireBuff;


    //Singleton
    public static PlayerHUD instance;
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {


        //Update high score when run starts; won't have to update again for the rest of the run
        UpdateHighScore();


    }

    // Update is called once per frame
    void Update()
    {

        //Ability bars are always changing, so we have to update every frame
        UpdateAbilityBars();

    }


    public void UpdateHealth ()
    {

        health.text = PlayerStats.instance.GetHealth().ToString();
        float percent = (float) PlayerStats.instance.GetHealth() / PlayerStats.instance.GetMaxHealth();
        StartCoroutine(UpdateBar(percent, healthVial));

    }
    
    public void SetAbilityIcons ()
    {

        if (leftIcon != null) {
            leftIcon.sprite = FindObjectOfType<LeftButtonAbility>().GetIcon();
        }
        if (rightIcon != null) {
            rightIcon.sprite = FindObjectOfType<RightButtonAbility>().GetIcon();
        }

    }
    //Must update ability bars in update
    private void UpdateAbilityBars ()
    {

        leftBar.fillAmount = FindObjectOfType<LeftButtonAbility>().GetMeterPercentage();
        rightBar.fillAmount = FindObjectOfType<RightButtonAbility>().GetMeterPercentage();

    }



    public void UpdateLevel (int level)
    {
        levelNumber.text = level.ToString();
    }

    //Call this to change the EXP bar fill
    public void UpdateExperienceBar (float percent)
    {
        StartCoroutine(UpdateBar(percent, expBar));
    }
    public IEnumerator UpdateBar(float percent, Image bar)
    {

        float origFill = bar.fillAmount;
        float timer = 0f;


        while (timer < barUpdateTime)
        {
            timer += Time.deltaTime;
            bar.fillAmount = Mathf.Lerp(origFill, percent, timer / barUpdateTime);
            yield return null;
        }

        bar.fillAmount = percent;

    }


    public void LoadUpgradeUI(string a, string b, string c)
    {
        upgradeAText.text = a;
        upgradeAText.gameObject.SetActive(true);
        upgradeBText.text = b;
        upgradeBText.gameObject.SetActive(true);
        upgradeCText.text = c;
        upgradeCText.gameObject.SetActive(true);
    }
    public void ResetUpgrades ()
    {

        upgradeAText.gameObject.SetActive(false);
        upgradeBText.gameObject.SetActive(false);
        upgradeCText.gameObject.SetActive(false);

    }



    public void UpdateCurrentScore (float scoreNum)
    {

        int scoreNumberLength = 10;

        string s = ((int)(scoreNum)).ToString();
        string prefix = "";

        for (int i = s.Length; i < scoreNumberLength; i++)
        {
            prefix += "0";
        }

        currentScore.text = prefix + s;


        //Also update the highscore if the current score passes the past high score
        if ((int) scoreNum > ProgressManager.instance.GetHighScore())
        {

            highScore.text = prefix + s;
            //SpeedRTPCControl.instance.PlayNewSkill();  //Soudn for new high score
        }


    }
    private void UpdateHighScore ()
    {

        int scoreNumberLength = 10;

        string s;
        if (ProgressManager.instance != null) {
            s = ProgressManager.instance.GetHighScore().ToString();
        }
        else { s = ""; }

        string prefix = "";

        for (int i = s.Length; i < scoreNumberLength; i++)
        {
            prefix += "0";
        }

        highScore.text = prefix + s;

    }


    public void UpdateWaveTimer (int t)
    {
        WaveTimer.text = t.ToString();
    }
    public void UpdateWaveNumber ()
    {

        waveNumber.text = EnemyWaveManager.instance.GetCurrentWaveNumber().ToString();

    }


    public void PlayWaveSurvivedAnimation()
    {

        waveSurvived.Play("TextSlide");

    }
    public void PlayNextWaveAnimation()
    {

        nextWave.Play("TextSlide");

    }


    //Call this when there's information to be displayed to the player
    public void DisplayMessage (string message)
    {
        messageText.text = message;
        displayMessage.Play("TextSlide");
    }



    //Stuff for displaying buffs
    Coroutine lastScoreBuff;
    Coroutine lastAttackBuff;
    Coroutine lastSpeedBuff;
    Coroutine lastRechargeBuff;
    Coroutine lastFireBuff;
    public void DisplayScoreBuff(float duration)
    {

        if (lastScoreBuff != null)
        {
            StopCoroutine(lastScoreBuff);
        }
        lastScoreBuff = StartCoroutine(StartBuff(scoreBuff, duration));

    }
    public void DisplayAttackBuff(float duration)
    {

        if (lastAttackBuff != null)
        {
            StopCoroutine(lastAttackBuff);
        }
        lastAttackBuff = StartCoroutine(StartBuff(attackBuff, duration));

    }
    public void DisplaySpeedBuff(float duration)
    {

        if (lastSpeedBuff != null)
        {
            StopCoroutine(lastSpeedBuff);
        }
        lastSpeedBuff = StartCoroutine(StartBuff(speedBuff, duration));

    }
    public void DisplayRechargeBuff(float duration)
    {

        if (lastRechargeBuff != null)
        {
            StopCoroutine(lastRechargeBuff);
        }
        lastRechargeBuff = StartCoroutine(StartBuff(rechargeBuff, duration));

    }
    public void DisplayFireBuff(float duration)
    {

        if (lastFireBuff != null)
        {
            StopCoroutine(lastFireBuff);
        }
        lastFireBuff = StartCoroutine(StartBuff(fireBuff, duration));

    }
    IEnumerator StartBuff (Image buffIcon, float duration)
    {



        float timer = 0f;

        //Whether the icon is fading yet
        bool fading = false;

        //Turn on indicator here
        //buffIcon.enabled = true;
        buffIcon.GetComponent<Animator>().Play("Idle");
        Debug.Log("buff");

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (timer >= duration * 0.66f && !fading)
            {
                fading = true;
                buffIcon.GetComponent<Animator>().Play("Icon Fade");
            }

            yield return null;
        }

        //Turn off indicator here
        //buffIcon.enabled = false;
        buffIcon.GetComponent<Animator>().Play("Off");



    }

}
