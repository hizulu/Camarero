using UnityEngine;

public class TrayMovement : MonoBehaviour
{
    public Rigidbody playerRb; // referencia al Rigidbody del personaje
    public float inclinacionMax = 15f; // grados máximo de inclinación lateral
    public float suavizado = 5f; // cuánto tarda en volver a la posición neutral
    public float factorVelocidad = 2f; // cuánto afecta la velocidad
    public float factorAceleracion = 1f; // cuánto afecta la aceleración
    private float lastXVelocity;

    void Start()
    {
        if (playerRb == null)
        {
            Debug.LogError("Debes asignar el Rigidbody del personaje en Bandeja.");
        }
        lastXVelocity = playerRb.velocity.x;
    }

    void Update()
    {
        // Velocidad horizontal actual
        float velocidadX = playerRb.velocity.x;

        // Calcula aceleración para efectos extra al cambio brusco
        float aceleracionX = (velocidadX - lastXVelocity) / Time.deltaTime;

        // Inclina la bandeja sobre X sumando a -90 según velocidad + aceleración
        float inclinacionX = Mathf.Clamp(-90f + (-velocidadX * factorVelocidad - aceleracionX * factorAceleracion), -90f - inclinacionMax, -90f + inclinacionMax);

        Quaternion rotacionDeseada = Quaternion.Euler(inclinacionX, 90f, -90f);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, rotacionDeseada, Time.deltaTime * suavizado);

        lastXVelocity = velocidadX;
    }
}
