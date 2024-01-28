using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class CinematicPlayer : MonoBehaviour
{
    [SerializeField] private string fileName;
    [SerializeField] private VideoPlayer playerRef;

    private bool videoEnded = false;

    public IEnumerator PlayVideo()
    {
        gameObject.SetActive(true);
        var videoUrl = Path.Combine(Application.streamingAssetsPath, fileName);
        playerRef.url = videoUrl;
        playerRef.Play();
        playerRef.loopPointReached += OnloopPointReached;
        yield return new WaitUntil(() => videoEnded);
    }

    private void OnloopPointReached(VideoPlayer source)
    {
        videoEnded = true;
        gameObject.SetActive(false);
    }
}