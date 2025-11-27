using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public GameObject[] walls;     // tus muros que deben activarse
    public GameObject boss;        // tu jefe

    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated) return;

        if (collision.CompareTag("Player"))
        {
            activated = true;

            // Activar muros
            foreach (GameObject w in walls)
            {
                w.SetActive(true);
            }

            // Activar jefe
            boss.SetActive(true);

            Debug.Log("¡Boss activado y muros activados!");
        }
    }
}
