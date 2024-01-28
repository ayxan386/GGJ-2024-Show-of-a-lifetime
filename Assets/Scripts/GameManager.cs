using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject LeaderBoard;

    // Start is called before the first frame update
    void Start()
    {
       
        if ( LeaderBoard != null)
        {
          KeyManager.OnRoundEnd += EnableLeaderBoardPanel;
        }
    }

    private void OnDestroy()
    {
        KeyManager.OnRoundEnd -= EnableLeaderBoardPanel;
    }

    private void EnableLeaderBoardPanel(int score)
    {
        //LeaderBoard.SetActive(true);
    }

  
}