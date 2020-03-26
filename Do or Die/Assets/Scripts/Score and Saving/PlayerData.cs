using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{

    public int highScore;

    public int highestWavesCleared;

    public int runs;

    public PlayerData()
    {
        highScore = 0;
        highestWavesCleared = 0;
        runs = 0;
    }
    public PlayerData (ProgressManager p)
    {
        highScore = p.GetHighScore();
        highestWavesCleared = p.GetHighestWavesCleared();
        runs = p.GetRuns();
    }


    //Singleton
    public static PlayerData instance;
    private void Awake()
    {
        if (instance == null)
        {
            //CopyData(SaveManager.LoadData());
            instance = this;
        }
    }


    public void InputResult (ScoreManager s)
    {

        highScore = Mathf.Max(highScore, s.GetScore());
        highestWavesCleared = s.GetClearedWaves();
        runs++;

    }


    private void CopyData (PlayerData p)
    {
        highScore = p.highScore;
        runs = p.runs;
    }

}
