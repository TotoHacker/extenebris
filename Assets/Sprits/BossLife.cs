using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossLife : MonoBehaviour
{
    [Header("Vida del Jefe")]
    public int maxHealth = 200;
    public int currentHealth;

    [Header("Barra de vida (opcional)")]
    public Slider healthBar;

    [Header("Daño visual")]
    public SpriteRenderer sr;
    private Color originalColor;

    [Header("Muros que deben desaparecer al morir")]
    public GameObject[] wallsToDisable;

    [Header("Animador del jefe (opcional)")]
    public Animator anim;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
        }

        if (sr == null)
            sr = GetComponentInChildren<SpriteRenderer>();

        originalColor = sr.color;

        if (anim == null)
            anim = GetComponentInChildren<Animator>();
    }

    // ==========================
    //    RECIBIR DAÑO
    // ==========================
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (healthBar != null)
            healthBar.value = currentHealth;

        StartCoroutine(DamageEffect());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator DamageEffect()
    {
        // jefe se pone BLANCO cuando recibe daño
        sr.color = Color.white;

        yield return new WaitForSeconds(0.1f);

        sr.color = originalColor;
    }

    // ==========================
    //    MUERTE DEL JEFE
    // ==========================
    void Die()
    {
        Debug.Log("JEFE MUERTO");

        // animación opcional
        if (anim != null)
            anim.SetTrigger("Die");

        // apagar muros
        foreach (GameObject g in wallsToDisable)
        {
            g.SetActive(false);
        }

        // destruir jefe después de animación
        Destroy(gameObject, 1.2f);
    }
}
