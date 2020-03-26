using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreResult : MonoBehaviour
{

    public int points;

    public ScoreResult (ScoreManager s)
    {
        points = s.GetScore();
    }

}
