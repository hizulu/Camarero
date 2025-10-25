using UnityEngine;
using TMPro;

public class TrayMovement : MonoBehaviour
{
    [Header("Tilting Settings")]
    public float tiltSensitivity = 5f;
    public float tiltSmoothness = 5f;
    public bool usePlayerMaxTilt = true;
    [Range(0f, 90f)]
    public float customMaxTiltAngle = 20f;

    [Header("End Game Settings")]
    public GameObject endPanel; // Panel de fin de juego
    public TMP_Text cupsCounterText;

    private PlayerMovement playerController;
    private Quaternion originalRotation;
    private float currentTiltAngle = 0f;
    private int cupsOnTray = 0; // Cantidad de copas en la bandeja

    void Start()
    {
        playerController = GetComponentInParent<PlayerMovement>();
        originalRotation = transform.localRotation;

        if (endPanel != null)
            endPanel.SetActive(false);

        // Contar copas ya en la bandeja al inicio (arreglo para triggers que no detectan objetos ya superpuestos)
        CountInitialCupsOnTray();

        UpdateCupsCounterUI();
    }

    void Update()
    {
        Vector3 velocity = playerController.GetVelocity();

        float maxTilt = usePlayerMaxTilt && playerController != null
            ? playerController.GetMaxTiltAngle()* 0.5f
            : customMaxTiltAngle;

        float targetTiltAngle = -velocity.x * tiltSensitivity;
        targetTiltAngle = Mathf.Clamp(targetTiltAngle, -maxTilt, maxTilt);

        currentTiltAngle = Mathf.Lerp(currentTiltAngle, targetTiltAngle, Time.deltaTime * tiltSmoothness);

        float inclinacionX = Mathf.Clamp(-90f + currentTiltAngle, -90f - maxTilt, -90f + maxTilt);
        transform.localRotation = Quaternion.Euler(inclinacionX, 90f, -90f);

        CheckEndGame();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Copa"))
        {
            Cups cup = other.GetComponent<Cups>();
            if (cup != null && cup.gameObject.activeInHierarchy)
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
                cupsOnTray--;
                cupsOnTray = Mathf.Max(0, cupsOnTray);
                UpdateCupsCounterUI();
            }
        }
    }

    private void CheckEndGame()
    {
        if (cupsOnTray == 0 && endPanel != null && !endPanel.activeSelf)
        {
            endPanel.SetActive(true);
            Time.timeScale = 0f; // Pausa el juego
        }
    }

    private void UpdateCupsCounterUI()
    {
        if (cupsCounterText != null)
            cupsCounterText.text = "Cups: " + cupsOnTray;
    }

    // Método para contar copas ya en la bandeja al inicio (evita que el juego termine inmediatamente si hay copas colocadas)
    private void CountInitialCupsOnTray()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2f, transform.rotation);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Copa"))
            {
                Cups cup = col.GetComponent<Cups>();
                if (cup != null && cup.gameObject.activeInHierarchy)
                {
                    cup.SetOnTray(true);
                    cupsOnTray++;
                }
            }
        }
    }

    // Método público para decrementar el contador cuando una copa se destruye mientras está en la bandeja
    public void DecrementCupCount()
    {
        cupsOnTray--;
        cupsOnTray = Mathf.Max(0, cupsOnTray);
        UpdateCupsCounterUI();
    }
}