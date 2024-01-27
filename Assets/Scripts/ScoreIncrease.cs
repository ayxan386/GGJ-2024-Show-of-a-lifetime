using UnityEngine;

public class ScoreIncrease : MonoBehaviour
{
    int score = 0;
    [SerializeField] LeaderBoardManager leaderBoardManager;

    void Start()
    {
        KeyManager.OnRoundEnd += OnRoundEnd;
    }

    private void OnDestroy()
    {
        KeyManager.OnRoundEnd -= OnRoundEnd;
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