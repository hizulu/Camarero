using UnityEngine;
using TMPro;

public class TrayMovement : MonoBehaviour
{
    [Header("Tilting Settings")]
    public float tiltSensitivity = 5f;
    public float tiltSmoothness = 5f;
    public bool usePlayerMaxTilt = true;
    [Range(0f, 90f)] public float customMaxTiltAngle = 20f;

    [Header("Tray Status UI")]
    public TMP_Text cupsCounterText;

    private PlayerMovement playerController;
    private Quaternion originalRotation;
    private float currentTiltAngle = 0f;
    private int cupsOnTray = 0;

    private bool gameEnded = false;
    private GameManager gameManager;

    void Start()
    {
        playerController = GetComponentInParent<PlayerMovement>();
        if (playerController == null)
            Debug.LogError("PlayerMovement not found in parent. Tray tilting will not work.");

        originalRotation = transform.localRotation;

        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
            Debug.LogError("GameManager not found in scene. End game will not trigger.");

        CountInitialCupsOnTray();
        UpdateCupsCounterUI();
    }

    void Update()
    {
        if (playerController == null || gameEnded) return;

        Vector3 velocity = playerController.GetVelocity();
        float maxTilt = usePlayerMaxTilt ? playerController.GetMaxTiltAngle() * 0.5f : customMaxTiltAngle;

        float targetTiltAngle = -velocity.x * tiltSensitivity;
        targetTiltAngle = Mathf.Clamp(targetTiltAngle, -maxTilt, maxTilt);

        currentTiltAngle = Mathf.Lerp(currentTiltAngle, targetTiltAngle, Time.deltaTime * tiltSmoothness);

        float clampedTilt = Mathf.Clamp(-90f + currentTiltAngle, -90f - maxTilt, -90f + maxTilt);
        transform.localRotation = Quaternion.Euler(clampedTilt, 90f, -90f);

        if (cupsOnTray == 0 && !gameEnded)
        {
            gameEnded = true;
            gameManager?.EndGame();
        }
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
                cupsOnTray = Mathf.Max(0, cupsOnTray - 1);
                UpdateCupsCounterUI();
            }
        }
    }

    private void UpdateCupsCounterUI()
    {
        if (cupsCounterText != null)
            cupsCounterText.text = "Copas en la bandeja: " + cupsOnTray;
    }

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

    // Public method to decrement count when a cup is destroyed
    public void DecrementCupCount()
    {
        cupsOnTray = Mathf.Max(0, cupsOnTray - 1);
        UpdateCupsCounterUI();
    }
}