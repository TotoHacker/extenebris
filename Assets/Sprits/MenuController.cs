using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    public GameObject menuContainer;

    // INICIAR JUEGO
    public void IniciarJuego()
    {
        SceneManager.LoadScene("prologo");
        Time.timeScale = 1f;
    }

    // REANUDAR
    public void Reanudar()
    {
        menuContainer.SetActive(false);
        Time.timeScale = 1f;
    }

    // AJUSTES (no hace nada por ahora)
    public void Ajustes()
    {
        Debug.Log("Ajustes aún no implementados");
    }

    // SALIR → Volver al menú principal
    public void Salir()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
