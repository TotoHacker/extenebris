using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;
    private int currentLives;

    public int touchDamage = 1;
    public Transform respawnPoint;

    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        currentLives = maxLives;

        // Obtener el SpriteRenderer del jugador
        sr = GetComponentInChildren<SpriteRenderer>();
        originalColor = sr.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage(touchDamage);
        }
    }

    public void TakeDamage(int amount)
    {
        currentLives -= amount;

        // Activar efecto visual
        StartCoroutine(HitEffect());

        if (currentLives <= 0)
        {
            DieAndRespawn();
        }
    }

    IEnumerator HitEffect()
    {
        // 1) Cambia a rojo
        sr.color = Color.red;

        // 2) Retroceso pequeño (knockback visual)
        Vector3 originalPos = transform.localPosition;
        transform.localPosition += new Vector3(0.1f, 0, 0);

        yield return new WaitForSeconds(0.1f);

        // 3) Restaurar color y posición
        sr.color = originalColor;
        transform.localPosition = originalPos;
    }

    void DieAndRespawn()
    {
        currentLives = maxLives;

        if (respawnPoint != null)
            transform.position = respawnPoint.position;
        else
            Debug.LogWarning("PlayerHealth: respawnPoint NO asignado.");
    }
}
