using UnityEngine;
using UnityEngine.Video;

public class TriggerCinematic : MonoBehaviour
{
    public GameObject cinematicUI;     // El canvas donde está el video
    public VideoPlayer videoPlayer;    // El Video Player

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag("Player"))
        {
            triggered = true;

            // Activar Cinematica
            cinematicUI.SetActive(true);

            // Pausar juego (si quieres)
            Time.timeScale = 0f;

            videoPlayer.Play();

            // Cuando termine el video → llamar método
            videoPlayer.loopPointReached += EndCinematic;
        }
    }

    void EndCinematic(VideoPlayer vp)
    {
        cinematicUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
