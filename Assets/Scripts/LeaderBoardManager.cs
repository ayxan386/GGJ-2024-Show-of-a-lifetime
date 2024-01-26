using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

    [System.Serializable]
    public class LeaderboardEntry
    {
        public string playerName;
        public int score;
    }
public class LeaderBoardManager : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public TextMeshProUGUI leaderboardText;

    private List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();
    // Start is called before the first frame update
    private void Start()
    {
        // Load leaderboard data
        LoadLeaderboard();
        UpdateLeaderboardUI();
    }

    public void AddScoreToLeaderboard(int score)
    {
        // Create a new entry with the player's name and score
        LeaderboardEntry entry = new LeaderboardEntry();
        entry.playerName = playerNameInput.text;
        entry.score = score;

        // Add the entry to the leaderboard
        leaderboardEntries.Add(entry);

        // Sort the leaderboard by score (descending)
        leaderboardEntries.Sort((a, b) => b.score.CompareTo(a.score));

        // Update the UI
        UpdateLeaderboardUI();

        // Save the updated leaderboard data
        SaveLeaderboard();
    }

    private void UpdateLeaderboardUI()
    {
        leaderboardText.text = "Leaderboard:\n";

        // Display only the top 10 entries or less if there are fewer entries
        int numEntriesToShow = Mathf.Min(leaderboardEntries.Count, 10);
        for (int i = 0; i < numEntriesToShow; i++)
        {
            leaderboardText.text += leaderboardEntries[i].playerName + ": " + leaderboardEntries[i].score + "\n";
        }
    }

    public void LoadLeaderboard()
    {
        string json = PlayerPrefs.GetString("Leaderboard", "");
        if (!string.IsNullOrEmpty(json))
        {
            leaderboardEntries = JsonUtility.FromJson<List<LeaderboardEntry>>(json);
        }
    }

    private void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(leaderboardEntries);
        PlayerPrefs.SetString("Leaderboard", json);
        PlayerPrefs.Save();
    }
}
