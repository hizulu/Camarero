using UnityEngine;

public class TrayMovement : MonoBehaviour
{
    public Rigidbody playerRb;          // Rigidbody del personaje
    public Movement playerMovement;     // Referencia al script de movimiento
    public float inclinacionMax = 15f;  // grados máximo de inclinación
    public float suavizado = 5f;        // velocidad de suavizado
    public float factorVelocidad = 2f;  // cuánto afecta la velocidad a la inclinación
    public float factorAceleracion = 0.5f; // cuánto afecta la aceleración a la inclinación

    void Update()
    {
        if (playerRb == null || playerMovement == null)
        {
            Debug.LogError("Debes asignar Rigidbody y Movement en Bandeja.");
            return;
        }

        // Velocidad y aceleración actuales
        float velocidadX = playerRb.velocity.x;
        float aceleracionX = playerMovement.CurrentAcceleration; // necesitamos exponer currentAcceleration como propiedad

        // Calcula la inclinación según velocidad y aceleración
        float inclinacionX = Mathf.Clamp(
            -90f + (-velocidadX * factorVelocidad - aceleracionX * factorAceleracion),
            -90f - inclinacionMax,
            -90f + inclinacionMax
        );

        // Rotación manteniendo Y = 90 y Z = -90
        Quaternion rotacionDeseada = Quaternion.Euler(inclinacionX, 90f, -90f);

        // Suavizado de la rotación
        transform.localRotation = Quaternion.Lerp(transform.localRotation, rotacionDeseada, Time.deltaTime * suavizado);
    }
}
