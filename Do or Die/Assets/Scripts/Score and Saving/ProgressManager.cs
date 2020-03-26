using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{


    private int highScore;
    public int GetHighScore() { return highScore; }

    private int highestWavesCleared;
    public int GetHighestWavesCleared() { return highestWavesCleared; }

    private int runs;
    public int GetRuns () { return runs; }


    //Singleton
    public static ProgressManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            //CopyData(SaveManager.LoadData());
            instance = this;
            ImportPlayerData(SaveManager.LoadData());
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void InputResult(ScoreManager s)
    {

        highScore = Mathf.Max(highScore, s.GetScore());
        highestWavesCleared = s.GetClearedWaves();
        runs++;

    }


    public void ImportPlayerData (PlayerData data)
    {

        highScore = data.highScore;
        highestWavesCleared = data.highestWavesCleared;
        runs = data.runs;

    }
    public PlayerData ExportPlayerData ()
    {
        return new PlayerData(this);
    }


}
