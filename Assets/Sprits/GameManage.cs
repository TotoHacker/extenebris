using UnityEngine;

public class GameManage : MonoBehaviour
{
    [Header("Fondo")]
    public Renderer fondo;               // tu Quad del fondo
    public Transform cameraTarget;       // referencia a la cámara
    public float parallaxSpeed = 0.02f;  // velocidad del parallax

    void Update()
    {
        if (fondo == null || cameraTarget == null) return;

        // El fondo sigue SIEMPRE la cámara
        Vector2 offset = fondo.material.mainTextureOffset;

        offset.x = cameraTarget.position.x * parallaxSpeed;

        fondo.material.mainTextureOffset = offset;
    }
}
