using UnityEngine;
using TMPro;

public class TrayMovement : MonoBehaviour
{
    [Header("Tilting Settings")]
    [Tooltip("How sensitive the tray is to player movement.")]
    public float tiltSensitivity = 5f;

    [Tooltip("How smoothly the tray tilts (higher = smoother).")]
    public float tiltSmoothness = 5f;

    [Tooltip("Use the player's max tilt angle instead of a custom one.")]
    public bool usePlayerMaxTilt = true;

    [Range(0f, 90f)]
    [Tooltip("Custom max tilt angle if not using player's.")]
    public float customMaxTiltAngle = 20f;

    [Header("End Game Settings")]
    [Tooltip("Panel to show when the game ends.")]
    public GameObject endPanel;
    public GameObject pantallaPanel;

    [Tooltip("Text to display the cup count.")]
    public TMP_Text cupsCounterText;

    private PlayerMovement playerController;
    private Quaternion originalRotation;
    private float currentTiltAngle = 0f;
    private int cupsOnTray = 0;
    private bool gameEnded = false;  // Prevent multiple end-game triggers

    void Start()
    {
        // Cache the player controller reference
        playerController = GetComponentInParent<PlayerMovement>();
        if (playerController == null)
        {
            Debug.LogError("PlayerMovement not found in parent. Tray tilting will not work.");
        }

        originalRotation = transform.localRotation;

        if (endPanel != null)
            endPanel.SetActive(false);

        // Count cups already on the tray at start (fixes issues with pre-placed cups)
        CountInitialCupsOnTray();

        UpdateCupsCounterUI();
    }

    void Update()
    {
        if (playerController == null) return;  // Safety check

        Vector3 velocity = playerController.GetVelocity();

        // Determine max tilt angle
        float maxTilt = usePlayerMaxTilt ? playerController.GetMaxTiltAngle() * 0.5f : customMaxTiltAngle;

        // Calculate target tilt based on velocity
        float targetTiltAngle = -velocity.x * tiltSensitivity;
        targetTiltAngle = Mathf.Clamp(targetTiltAngle, -maxTilt, maxTilt);

        // Smoothly interpolate to target tilt
        currentTiltAngle = Mathf.Lerp(currentTiltAngle, targetTiltAngle, Time.deltaTime * tiltSmoothness);

        // Apply tilt (clamped to prevent over-rotation)
        float clampedTilt = Mathf.Clamp(-90f + currentTiltAngle, -90f - maxTilt, -90f + maxTilt);
        transform.localRotation = Quaternion.Euler(clampedTilt, 90f, -90f);

        CheckEndGame();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Copa"))
        {
            Cups cup = other.GetComponent<Cups>();
            if (cup != null && cup.gameObject.activeInHierarchy && !cup.IsOnTray())
            {
                cup.SetOnTray(true);
                cupsOnTray++;
                UpdateCupsCounterUI();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Copa"))
        {
            Cups cup = other.GetComponent<Cups>();
            if (cup != null && cup.gameObject.activeInHierarchy && cup.IsOnTray())
            {
                cup.SetOnTray(false);
                cupsOnTray = Mathf.Max(0, cupsOnTray - 1);  // Ensure non-negative
                UpdateCupsCounterUI();
            }
        }
    }

    private void CheckEndGame()
    {
        if (!gameEnded && cupsOnTray == 0 && endPanel != null && pantallaPanel!=null)
        {
            endPanel.SetActive(true);
            pantallaPanel.SetActive(false);
            Time.timeScale = 0f;  // Pause the game
            gameEnded = true;
            Debug.Log("Game ended: No cups left on tray.");
        }
    }

    private void UpdateCupsCounterUI()
    {
        if (cupsCounterText != null)
        {
            cupsCounterText.text = "Copas en la bandeja: " + cupsOnTray;
        }
    }

    // Counts cups already overlapping the tray at start (avoids instant game over)
    private void CountInitialCupsOnTray()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2f, transform.rotation);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Copa"))
            {
                Cups cup = col.GetComponent<Cups>();
                if (cup != null && cup.gameObject.activeInHierarchy && !cup.IsOnTray())
                {
                    cup.SetOnTray(true);
                    cupsOnTray++;
                }
            }
        }
    }

    // Public method to decrement count when a cup is destroyed while on the tray
    public void DecrementCupCount()
    {
        cupsOnTray = Mathf.Max(0, cupsOnTray - 1);
        UpdateCupsCounterUI();
    }
}