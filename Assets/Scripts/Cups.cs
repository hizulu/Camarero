using UnityEngine;

public class Cups : MonoBehaviour
{
    [Header("Scoring")]
    public int pointsPerSecond = 10; // Points earned per second while on tray

    private bool isOnTray = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Ensure gravity is on for falling
        rb.useGravity = true;
    }

    void Update()
    {
        if (isOnTray)
        {
            // Earn points while on tray (you can integrate with a global score manager)
            // For simplicity, log to console; replace with your scoring system
            Debug.Log("Earning points: " + pointsPerSecond * Time.deltaTime);
        }
    }

    // Called by TrayController when entering/exiting tray
    public void SetOnTray(bool onTray)
    {
        isOnTray = onTray;
        if (!onTray)
        {
            // Optional: Add effects when falling off, e.g., sound or particle
            Debug.Log("Cup fell off!");
        }
    }

    // Optional: Reset cup if it falls too far (e.g., respawn or destroy)
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // Assuming ground has this tag
        {
            // Destroy or respawn the cup
            Destroy(gameObject);
        }
    }
}