using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public void StartGame()
    {
        KeyManager.Instance.RoundEnded = false;
        gameObject.SetActive(false);
    }
}