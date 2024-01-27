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
        UpdateScore();
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore()
    {
        
        LeaderboardEntry entry = new LeaderboardEntry();
        entry.score = score;
       
        leaderBoardManager.AddScoreToLeaderboard(score);
    }
}
