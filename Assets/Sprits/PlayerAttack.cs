using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 25;
    public float attackRange = 1f;
    public Transform attackPoint;
    public LayerMask enemyLayer;

    public void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            Life life = enemy.GetComponent<Life>();
            if (life != null)
                life.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
