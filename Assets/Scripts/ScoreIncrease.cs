using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreIncrease : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreTxt;
    int score = 0;
    [SerializeField] LeaderBoardManager leaderBoardManager;
    // Start is called before the first frame update
    void Start()
    {
        KeyManager.OnRoundEnd += OnRoundEnd;
    }
    void OnRoundEnd(int score)
    {
        this.score = score;
    }

    public void UpdateScore()
    {
        leaderBoardManager.AddScoreToLeaderboard(score);
    }
}
