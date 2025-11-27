using UnityEngine;
using TMPro; // Necesitas este namespace para usar TextMeshPro

public class PlayerScoreManager : MonoBehaviour
{
    private int puntuacionActual = 0;

    // Referencia al componente de texto de la UI (Asignar en Unity)
    public TextMeshProUGUI textoPuntuacionUI;

    void Start()
    {
        // ... (El resto del Start) ...
        ActualizarUI();
    }

    public void AñadirPuntos(int cantidad)
    {
        puntuacionActual += cantidad;
        Debug.Log("Puntos obtenidos: " + cantidad + ". Total: " + puntuacionActual);
        ActualizarUI();
    }

    private void ActualizarUI()
    {
        if (textoPuntuacionUI != null)
        {
            // Actualiza el texto con el nuevo valor de la puntuación
            textoPuntuacionUI.text = puntuacionActual.ToString();
        }
    }
}