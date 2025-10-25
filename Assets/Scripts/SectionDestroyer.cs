using UnityEngine;

public class SectionDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DestroyerTrigger"))
        {
            Destroy(gameObject);
        }
    }
}