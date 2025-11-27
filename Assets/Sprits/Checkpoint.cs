using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.UpdateRespawnPoint(transform.position);
                Debug.Log("Nuevo checkpoint alcanzado: " + transform.position);
            }
        }
    }
}
