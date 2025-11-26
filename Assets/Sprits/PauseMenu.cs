using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuContainer;
    private bool isPaused = false;

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) Reanudar();
            else Pausar();
        }
    }

    public void Pausar()
    {
        menuContainer.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void Reanudar()
    {
        menuContainer.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
