using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playVideo : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] CinematicPlayer player;
    void Start()
    {
        StartCoroutine(player.PlayVideo());
    }

    // Update is called once per frame
    void Update()
    {
        player.LoadSceneOnloopPointReached("MainMenu");
    }
}
