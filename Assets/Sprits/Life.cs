using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Life : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar;

    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.maxValue = maxHealth;

        sr = GetComponentInChildren<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (healthBar != null)
            healthBar.value = currentHealth;

        StartCoroutine(HitEffect());

        // 🔥 AVISAR AL ENEMIGO QUE FUE DAÑADO
        EnemyBaseAI ai = GetComponent<EnemyBaseAI>();
        if (ai != null)
            ai.OnDamageTaken(currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    IEnumerator HitEffect()
    {
        // 1) Cambiar color a rojo
        sr.color = Color.red;

        // 2) Pequeña vibración
        Vector3 originalPos = transform.localPosition;
        transform.localPosition += new Vector3(0.1f, 0, 0);

        yield return new WaitForSeconds(0.1f);

        // 3) Restaurar color y posición
        sr.color = originalColor;
        transform.localPosition = originalPos;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
