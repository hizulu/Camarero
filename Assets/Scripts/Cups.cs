using UnityEngine;

public class Cups : MonoBehaviour
{
    [Header("Tray Reference")]
    public TrayMovement trayMovement;

    [Header("Destruction Effects")]
    public GameObject brokenCupPrefab;
    public float destructionDelay = 0.2f;

    private AudioSource cupAudio;
    private bool onTray = false;
    private Rigidbody rb;
    private bool hasBeenDestroyed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        cupAudio = GetComponent<AudioSource>();

        if (trayMovement == null)
        {
            trayMovement = FindObjectOfType<TrayMovement>();
            if (trayMovement == null)
            {
                Debug.LogWarning("No TrayMovement found in scene. Cup will not decrement tray count.");
            }
        }
    }

    public void SetOnTray(bool value)
    {
        onTray = value;
        if (!value)
        {
            Debug.Log("Cup fell off the tray!");
        }
    }

    public bool IsOnTray()
    {
        return onTray;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision detected with: {collision.gameObject.name}, tag: {collision.gameObject.tag}");

        if (hasBeenDestroyed) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            if (onTray && trayMovement != null)
                trayMovement.DecrementCupCount();

            Debug.Log("Cup collided with the ground and will be destroyed.");
            // Reproducir sonido (cortando cualquier anterior)
            if (cupAudio != null && cupAudio.clip != null)
            {
                if (cupAudio.isPlaying)
                    cupAudio.Stop();
                cupAudio.Play();
            }
            Debug.Log("Playing cup break sound.");

            //// Instanciar copa rota
            //if (brokenCupPrefab != null)
            //    Instantiate(brokenCupPrefab, transform.position, transform.rotation);

            hasBeenDestroyed = true;
            Debug.Log("Instantiating broken cup prefab and scheduling destruction.");
            Destroy(gameObject, destructionDelay);
        }
    }
}
