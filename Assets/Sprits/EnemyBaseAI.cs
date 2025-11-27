using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBaseAI : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 2f;
    public int damage = 20;
    public float attackRange = 0.7f;
    public float attackCooldown = 1.5f;

    [Header("Fury Mode")]
    public bool isFury = false;
    public float furyThreshold = 35f;
    public float furySpeedMultiplier = 1.8f;
    public float furyCooldown = 0.5f;

    [Header("Anti-Caída")]
    public Transform edgeCheckRight;
    public Transform edgeCheckLeft;
    public float edgeCheckDistance = 0.3f;
    public LayerMask groundLayer;

    private float cooldownTimer = 0f;
    private Transform player;

    private Animator anim;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private Life enemyLife;
    private PlayerHealth playerLife;

    private float moveDirection = 1f;
    private Color originalColor;


    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;

        if (player != null)
            playerLife = player.GetComponent<PlayerHealth>();

        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        enemyLife = GetComponent<Life>();

        rb.freezeRotation = true;

        originalColor = sr.color;
    }



    void Update()
    {
        if (player == null) return;

        HandleMovement();

        cooldownTimer -= Time.deltaTime;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < attackRange && cooldownTimer <= 0)
        {
            Attack();
        }
    }



    // ======================================
    // 🔥 FURIA — llamado desde Life.cs
    // ======================================
    public void OnDamageTaken(int newHealth)
    {
        if (!isFury && newHealth <= furyThreshold)
        {
            isFury = true;

            // Color de furia
            sr.color = new Color(1f, 0.3f, 0f);

            // Más velocidad
            speed *= furySpeedMultiplier;

            // Ataca más rápido
            attackCooldown = furyCooldown;

            Debug.Log("⚡ ENEMY ENTERED FURY MODE!");
        }
    }



    // ======================================
    // MOVIMIENTO Y PERSECUCIÓN
    // ======================================
    void HandleMovement()
    {
        moveDirection = Mathf.Sign(player.position.x - transform.position.x);

        // anti caída
        bool rightGround = Physics2D.Raycast(edgeCheckRight.position, Vector2.down, edgeCheckDistance, groundLayer);
        bool leftGround = Physics2D.Raycast(edgeCheckLeft.position, Vector2.down, edgeCheckDistance, groundLayer);

        if (moveDirection > 0 && !rightGround)
            moveDirection = -1;
        if (moveDirection < 0 && !leftGround)
            moveDirection = 1;

        sr.flipX = moveDirection < 0;
    }



    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection * speed, rb.linearVelocity.y);
    }



    // ======================================
    // ATAQUE + ANIMACIÓN AtackEnemy
    // ======================================
    void Attack()
    {
        anim.SetBool("AtackEnemy", true);
        StartCoroutine(StopAttackAnimation());

        if (playerLife != null)
        {
            playerLife.TakeDamage(damage);

            if (isFury)
            {
                float dir = moveDirection;
                Vector2 force = new Vector2(dir * 8f, 6f);
                playerLife.ApplyKnockback(force);
            }
        }

        cooldownTimer = attackCooldown;
    }

    IEnumerator StopAttackAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("AtackEnemy", false);
    }



    // ======================================
    // DEBUG RAYCASTS
    // ======================================
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        if (edgeCheckRight != null)
            Gizmos.DrawLine(edgeCheckRight.position, edgeCheckRight.position + Vector3.down * edgeCheckDistance);

        if (edgeCheckLeft != null)
            Gizmos.DrawLine(edgeCheckLeft.position, edgeCheckLeft.position + Vector3.down * edgeCheckDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
    