using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreIncrease : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreTxt;
    int score = 0;
    [SerializeField] LeaderBoardManager leaderBoardManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            score++;
        }
        scoreTxt.text = score.ToString();
    }

    public void UpdateScore()
    {
        leaderBoardManager.LoadLeaderboard();
        leaderBoardManager.AddScoreToLeaderboard(score);
    }
}
