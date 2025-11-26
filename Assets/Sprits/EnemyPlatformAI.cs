using UnityEngine;

public class EnemyPlatformAI : MonoBehaviour
{
    [Header("Config")]
    public float speed = 2f;
    public int damage = 20;
    public float attackRange = 0.7f;
    public float attackCooldown = 1.5f;

    private float cooldownTimer = 0;

    [Header("Floor Detection")]
    public Transform edgeCheck;         // empty object delante del enemigo
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;       // capa "Ground"

    private Transform player;
    private PlayerHealth playerLife;

    private Animator anim;
    private SpriteRenderer sr;

    private bool movingRight = true;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerLife = player.GetComponent<PlayerHealth>();

        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null) return;

        // --- DETECTAR BORDE ---
        bool noGround = !Physics2D.Raycast(edgeCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        if (noGround)
            FlipDirection();

        // --- MOVER ---
        float step = speed * Time.deltaTime;
        Vector3 dir = movingRight ? Vector3.right : Vector3.left;
        transform.Translate(dir * step);

        // --- ANIMACIÓN ---
        if (anim != null)
            anim.SetBool("isWalking", true);

        // --- VOLTEAR SPRITE ---
        sr.flipX = movingRight;

        // --- ATAQUE ---
        cooldownTimer -= Time.deltaTime;
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist < attackRange && cooldownTimer <= 0)
        {
            playerLife.TakeDamage(damage);
            cooldownTimer = attackCooldown;
        }
    }

    void FlipDirection()
    {
        movingRight = !movingRight;
    }

    private void OnDrawGizmosSelected()
    {
        if (edgeCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(edgeCheck.position,
            edgeCheck.position + Vector3.down * groundCheckDistance);
    }
}
