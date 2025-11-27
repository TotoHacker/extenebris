using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida del jugador")]
    public int maxLives = 100;
    private int currentLives;

    [Header("Daño recibido al tocar un enemigo")]
    public int touchDamage = 20;

    [Header("Punto de respawn actual")]
    public Vector3 currentRespawn;

    [Header("Efecto visual de daño")]
    private SpriteRenderer sr;
    private Color originalColor;

    private bool isInvulnerable = false;
    private bool isDead = false;   // 🔥 NUEVO

    private Rigidbody2D rb;


    void Start()
    {
        currentLives = maxLives;

        sr = GetComponentInChildren<SpriteRenderer>();
        originalColor = sr.color;

        rb = GetComponent<Rigidbody2D>();

        currentRespawn = transform.position;
    }


    void Update()
    {
        DetectFallDeath();
    }


    void DetectFallDeath()
    {
        if (isDead) return; // 🔥 Evita muerte doble

        Camera cam = Camera.main;
        float bottom = cam.ViewportToWorldPoint(new Vector3(0, -0.2f, 0)).y;

        if (transform.position.y < bottom)
            KillPlayer();
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            TakeDamage(touchDamage);
        }
    }


    public void TakeDamage(int amount)
    {
        if (isInvulnerable || isDead) return;  // 🔥 BLOQUEO TOTAL

        currentLives -= amount;

        StartCoroutine(HitEffect());
        StartCoroutine(TempInvulnerability());

        if (currentLives <= 0)
        {
            KillPlayer();
        }
    }


    // 🔥 NUEVO MÉTODO SEGURO
    void KillPlayer()
    {
        if (isDead) return;
        isDead = true;                    // 🔥 Se bloquea TODO daño
        isInvulnerable = true;            // 🔥 Evita ser golpeado antes del respawn

        DieAndRespawn();
    }


    IEnumerator HitEffect()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }


    IEnumerator TempInvulnerability()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(0.7f);
        isInvulnerable = false;
    }


    void DieAndRespawn()
    {
        currentLives = maxLives;

        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        // 🔥 AQUI YA NO PUEDES RECIBIR DAÑO JAMÁS
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        transform.position = currentRespawn;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        StartCoroutine(RespawnInvulnerability());
    }


    IEnumerator RespawnInvulnerability()
    {
        float duration = 1.2f;
        float blinkSpeed = 0.1f;

        for (float t = 0; t < duration; t += blinkSpeed)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(blinkSpeed);
        }

        sr.enabled = true;

        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);

        isInvulnerable = false;
        isDead = false;       // 🔥 YA PUEDE RECIBIR DAÑO OTRA VEZ
    }


    public void UpdateRespawnPoint(Vector3 newPoint)
    {
        currentRespawn = newPoint;
        Debug.Log("Nuevo checkpoint guardado: " + newPoint);
    }
    public void ApplyKnockback(Vector2 force)
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // resetear primero
            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }

}
