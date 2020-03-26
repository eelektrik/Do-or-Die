using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{


    private int score;
    public int GetScore () { return score; }


    private float scoreMultiplier = 1;


    public Text scoreText;
    public float scoreUpdateTime;


    //Singleton
    public static ScoreManager instance;
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



    public void IncreaseScore (int s)
    {
        score += (int)(s * scoreMultiplier);
        StartCoroutine(UpdateScore(score - s, score));
    }


    public IEnumerator UpdateScore(float origScore, float newScore)
    {

        float timer = 0f;


        while (timer < scoreUpdateTime)
        {
            timer += Time.deltaTime;
            PlayerHUD.instance.UpdateCurrentScore((Mathf.Lerp(origScore, newScore, timer / scoreUpdateTime)));
            yield return null;
        }

        score = (int) newScore;

    }


    public int GetClearedWaves ()
    {
        return EnemyWaveManager.instance.GetClearedWaves();
    }


    void UpdateScoreNumber (float scoreNum)
    {

        int scoreNumberLength = 10;

        string s = ((int)(scoreNum)).ToString();
        string prefix = "";

        for (int i = s.Length; i < scoreNumberLength; i++)
        {
            prefix += "0";
        }

        scoreText.text = prefix + s;

    }


    //Call this when a score-pickup is picked up
    public void IncreaseMultiplier (float duration)
    {
        StartCoroutine(MultiplierDuration(duration));
    }
    public IEnumerator MultiplierDuration (float duration)
    {

        //This is what we'll multiply by
        float mult = 1.5f;

        float timer = 0f;
        scoreMultiplier *= mult;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            Debug.Log(timer);
            yield return null;
        }

        scoreMultiplier = scoreMultiplier / mult;

    }


}
