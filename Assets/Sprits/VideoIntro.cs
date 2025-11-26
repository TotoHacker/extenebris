using UnityEngine;
using UnityEngine.Video;

public class VideoIntro : MonoBehaviour
{
    private VideoPlayer video;

    void Start()
    {
        video = GetComponent<VideoPlayer>();
        video.loopPointReached += OnVideoFinished;

        // Congelar el juego hasta que termine el video
        Time.timeScale = 0f;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Ocultar el video
        gameObject.SetActive(false);

        // Reanudar el juego
        Time.timeScale = 1f;
    }
}
