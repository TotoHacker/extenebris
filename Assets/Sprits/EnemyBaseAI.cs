using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBaseAI : MonoBehaviour
{
    [Header("Config")]
    public float speed = 2f;
    public int damage = 20;
    public float attackRange = 0.7f;
    public float attackCooldown = 1.5f;

    [Header("Anti-Caída")]
    public Transform edgeCheckRight;
    public Transform edgeCheckLeft;
    public float edgeCheckDistance = 0.25f;
    public LayerMask groundLayer;

    private float cooldownTimer = 0f;
    private Transform player;

    private Animator anim;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private PlayerHealth playerLife;

    // dirección (1 derecha, -1 izquierda)
    private float moveDirection = 1f;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerLife = player.GetComponent<PlayerHealth>();

        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        rb.freezeRotation = true;
        rb.gravityScale = 3f;
    }

    void Update()
    {
        bool rightGround = Physics2D.Raycast(edgeCheckRight.position, Vector2.down, edgeCheckDistance, groundLayer);
        bool leftGround = Physics2D.Raycast(edgeCheckLeft.position, Vector2.down, edgeCheckDistance, groundLayer);

        // Si voy hacia la derecha pero no hay suelo → girar
        if (moveDirection > 0 && !rightGround)
            moveDirection = -1;

        // Si voy hacia la izquierda pero no hay suelo → girar
        if (moveDirection < 0 && !leftGround)
            moveDirection = 1;

        // Animación
        if (anim != null)
            anim.SetBool("isWalking", true);

        // Voltear sprite
        sr.flipX = moveDirection < 0;

        // Ataque
        cooldownTimer -= Time.deltaTime;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < attackRange && cooldownTimer <= 0)
        {
            playerLife.TakeDamage(damage);
            cooldownTimer = attackCooldown;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection * speed, rb.linearVelocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        if (edgeCheckRight != null)
            Gizmos.DrawLine(edgeCheckRight.position, edgeCheckRight.position + Vector3.down * edgeCheckDistance);

        if (edgeCheckLeft != null)
            Gizmos.DrawLine(edgeCheckLeft.position, edgeCheckLeft.position + Vector3.down * edgeCheckDistance);
    }
}
