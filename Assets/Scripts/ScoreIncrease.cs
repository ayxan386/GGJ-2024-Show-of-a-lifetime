using UnityEngine;

public class ScoreIncrease : MonoBehaviour
{
    int score = 0;
    [SerializeField] LeaderBoardManager leaderBoardManager;
    [SerializeField]  MenuNavigator menuNavigator;

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
        menuNavigator.OpenNameInputPanel();
    }

    public void UpdateScore()
    {
        leaderBoardManager.AddScoreToLeaderboard(score);
        menuNavigator.OpenLeaderBoardPanel();
    }
}