using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int score;
}

[Serializable]
public class LeaderboardJSon
{
    public List<LeaderboardEntry> leaderboardEntries;
}

public class LeaderBoardManager : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public TextMeshProUGUI leaderboardText;
    LeaderboardJSon leaderboardJSon = new LeaderboardJSon();

    private void Awake()
    {
        LoadLeaderboard();
    }

    public void AddScoreToLeaderboard(int score)
    {
        // Create a new entry with the player's name and score
        LeaderboardEntry entry = new LeaderboardEntry();
        entry.playerName = playerNameInput.text;
        entry.score = score;

        // Add the entry to the leaderboard
        leaderboardJSon.leaderboardEntries.Add(entry);

        // Sort the leaderboard by score (descending)
        leaderboardJSon.leaderboardEntries.Sort((a, b) => b.score.CompareTo(a.score));

        // Update the UI
        UpdateLeaderboardUI();

        // Save the updated leaderboard data
        SaveLeaderboard();
    }

    private void UpdateLeaderboardUI()
    {
        // Display only the top 10 entries or less if there are fewer entries
        leaderboardText.text = "";
        int numEntriesToShow = Mathf.Min(leaderboardJSon.leaderboardEntries.Count, 10);
        for (int i = 0; i < numEntriesToShow; i++)
        {
            leaderboardText.text += leaderboardJSon.leaderboardEntries[i].playerName + ": " +
                                    leaderboardJSon.leaderboardEntries[i].score + "\n";
        }
    }

    private void LoadLeaderboard()
    {
        string json = PlayerPrefs.GetString("Leaderboard", "");
        if (!string.IsNullOrEmpty(json))
        {
            leaderboardJSon = JsonUtility.FromJson<LeaderboardJSon>(json);
        }
        else
        {
            leaderboardJSon.leaderboardEntries = new List<LeaderboardEntry>();
        }
    }

    private void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(leaderboardJSon);
        PlayerPrefs.SetString("Leaderboard", json);
        PlayerPrefs.Save();
    }
}