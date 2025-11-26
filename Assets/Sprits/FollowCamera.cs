using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float smooth = 5f;

    [Header("Offset en Y (más negativo = personaje más abajo)")]
    public float yOffset = -5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 newPos = new Vector3(
            target.position.x,
            target.position.y + yOffset,
            transform.position.z
        );

        transform.position = Vector3.Lerp(transform.position, newPos, smooth * Time.deltaTime);
    }
}
