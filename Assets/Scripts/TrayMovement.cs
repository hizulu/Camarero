using UnityEngine;

public class TrayMovement : MonoBehaviour
{
    public Rigidbody playerRb;          // Rigidbody del personaje
    public Movement playerMovement;     // Referencia al script de movimiento
    public float inclinacionMax = 15f;  // grados m�ximo de inclinaci�n
    public float suavizado = 5f;        // velocidad de suavizado
    public float factorVelocidad = 2f;  // cu�nto afecta la velocidad a la inclinaci�n
    public float factorAceleracion = 0.5f; // cu�nto afecta la aceleraci�n a la inclinaci�n

    void Update()
    {
        if (playerRb == null || playerMovement == null)
        {
            Debug.LogError("Debes asignar Rigidbody y Movement en Bandeja.");
            return;
        }

        // Velocidad y aceleraci�n actuales
        float velocidadX = playerRb.velocity.x;
        float aceleracionX = playerMovement.CurrentAcceleration; // necesitamos exponer currentAcceleration como propiedad

        // Calcula la inclinaci�n seg�n velocidad y aceleraci�n
        float inclinacionX = Mathf.Clamp(
            -90f + (-velocidadX * factorVelocidad - aceleracionX * factorAceleracion),
            -90f - inclinacionMax,
            -90f + inclinacionMax
        );

        // Rotaci�n manteniendo Y = 90 y Z = -90
        Quaternion rotacionDeseada = Quaternion.Euler(inclinacionX, 90f, -90f);

        // Suavizado de la rotaci�n
        transform.localRotation = Quaternion.Lerp(transform.localRotation, rotacionDeseada, Time.deltaTime * suavizado);
    }
}
