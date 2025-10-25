using UnityEngine;

public class TrayMovement : MonoBehaviour
{
    [Header("Tilting Settings")]
    [Tooltip("Sensibilidad de inclinaci�n seg�n la velocidad del jugador")]
    public float tiltSensitivity = 5f;

    [Tooltip("Suavidad de la interpolaci�n de inclinaci�n")]
    public float tiltSmoothness = 5f;

    [Tooltip("�ngulo m�ximo de inclinaci�n (editable desde el editor)")]
    [Range(0f, 90f)]
    public float maxTiltAngle = 20f; // ahora editable en el inspector

    private PlayerMovement playerController;
    private Quaternion originalRotation;
    private float currentTiltAngle = 0f;

    void Start()
    {
        playerController = GetComponentInParent<PlayerMovement>();
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        Vector3 velocity = playerController.GetVelocity();

        // Calcula el �ngulo deseado seg�n la velocidad
        float targetTiltAngle = -velocity.x * tiltSensitivity;
        targetTiltAngle = Mathf.Clamp(targetTiltAngle, -maxTiltAngle, maxTiltAngle);

        // Interpolaci�n suave
        currentTiltAngle = Mathf.Lerp(currentTiltAngle, targetTiltAngle, Time.deltaTime * tiltSmoothness);

        // Aplica la rotaci�n
        float inclinacionX = Mathf.Clamp(-90f + currentTiltAngle, -90f - maxTiltAngle, -90f + maxTiltAngle);
        transform.localRotation = Quaternion.Euler(inclinacionX, 90f, -90f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Copa"))
        {
            Cups cup = other.GetComponent<Cups>();
            if (cup != null)
                cup.SetOnTray(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Copa"))
        {
            Cups cup = other.GetComponent<Cups>();
            if (cup != null)
                cup.SetOnTray(false);
        }
    }
}
