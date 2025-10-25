using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Speed of side-to-side movement
    public float maxTiltAngle = 30f; // Max tilt angle for the tray (passed to TrayController)

    private float horizontalInput;
    private Vector3 velocity; // Track velocity for tilting

    public SectionsManager sectionManager;

    void Update()
    {
        // Get input (A/D or Left/Right arrows)
        horizontalInput = Input.GetAxis("Horizontal");

        // Move the player
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, 0f);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Calculate velocity (for tilting)
        velocity = moveDirection * moveSpeed;
    }

    // Public getter for velocity (used by TrayController)
    public Vector3 GetVelocity()
    {
        return velocity;
    }

    // Public getter for max tilt (used by TrayController)
    public float GetMaxTiltAngle()
    {
        return maxTiltAngle;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SectionTrigger"))
        {
            sectionManager.SpawnSection(other.transform);
        }
    }
}