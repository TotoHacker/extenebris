using UnityEngine;

public class ElevatorPlatform : MonoBehaviour
{
    public Transform endPoint;
    public float speed = 2f;

    private bool moveUp = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            moveUp = true;

            // 👉 Hacer que el jugador suba con la plataforma
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
        }
    }

    void Update()
    {
        if (moveUp)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                endPoint.position,
                speed * Time.deltaTime
            );

            if (transform.position == endPoint.position)
            {
                moveUp = false;
            }
        }
    }
}
