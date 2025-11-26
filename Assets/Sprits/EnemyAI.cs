using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;
    public int damage = 20;
    public float attackRange = 0.7f;
    public float attackCooldown = 1.5f;

    private float cooldownTimer = 0f;
    private Transform player;

    private Animator anim;
    private SpriteRenderer sr;

    private PlayerHealth playerLife;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerLife = player.GetComponent<PlayerHealth>();

        anim = GetComponent<Animator>();
        if (anim == null) anim = GetComponentInChildren<Animator>();

        sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();

    }

    void Update()
    {
        if (player == null) return;

        cooldownTimer -= Time.deltaTime;

        // perseguir
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );

        // animación caminar (solo si tiene animator)
        if (anim != null)
            anim.SetBool("isWalking", true);

        // voltear sprite hacia el jugador
        if (player.position.x > transform.position.x)
            sr.flipX = true;
        else
            sr.flipX = false;

        // atacar
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < attackRange && cooldownTimer <= 0)
        {
            playerLife.TakeDamage(damage);
            cooldownTimer = attackCooldown;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
