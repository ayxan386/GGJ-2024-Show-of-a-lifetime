using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   [SerializeField] GameObject LeaderBoard;
   
    // Start is called before the first frame update
    void Start()
    {
        KeyManager.OnRoundEnd += EnableLeaderBoardPanel;
    }

    private void EnableLeaderBoardPanel(int score)
    {
        LeaderBoard.SetActive(true);
    }
    // Update is called once per frame

}
