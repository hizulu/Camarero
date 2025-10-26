using UnityEngine;

public class Cups : MonoBehaviour
{
    [Header("Tray Reference")]
    public TrayMovement trayMovement;

    private bool isOnTray = false;
    private Rigidbody rb;
    private bool hasBeenDestroyed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        if (trayMovement == null)
        {
            trayMovement = FindObjectOfType<TrayMovement>();
            if (trayMovement == null)
            {
                Debug.LogWarning("No TrayMovement found in scene. Cup will not decrement tray count.");
            }
        }
    }

    public void SetOnTray(bool onTray)
    {
        isOnTray = onTray;
        if (!onTray)
        {
            Debug.Log("Cup fell off the tray!");
        }
    }

    public bool IsOnTray()
    {
        return isOnTray;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasBeenDestroyed) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            if (isOnTray && trayMovement != null)
            {
                trayMovement.DecrementCupCount();
                Debug.Log("Cup hit the ground while on tray - decrementing count.");
            }
            else
            {
                Debug.Log("Cup hit the ground (not on tray).");
            }

            //Destruirlas con un poco de Delay
            hasBeenDestroyed = true;
            Destroy(gameObject, 0.2f);
        }
    }
}
