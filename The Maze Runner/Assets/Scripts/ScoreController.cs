using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{

    public List<Score> scores = new List<Score>();

    private void Start() {
        
    }


    public void AddScore(string name, float score)
    {
        scores.Add(new Score { playerName = name, playerScore = score });
        scores.Sort((Score x, Score y) => y.playerScore.CompareTo(x.playerScore));
    }

    public string[] GetNames()
    {
        string[] namesList = new string[scores.Count];

        for (int i = 0; i < scores.Count; i++)
        {
            namesList[i] = scores[i].playerName;
        }

        return namesList;
    }

    public float[] GetScores()
    {
        float[] scoresList = new float[scores.Count];

        for (int i = 0; i < scores.Count; i++)
        {
            scoresList[i] = scores[i].playerScore;
        }

        return scoresList;
    }

}
