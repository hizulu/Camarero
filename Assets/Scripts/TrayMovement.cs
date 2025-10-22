using UnityEngine;

public class TrayMovement : MonoBehaviour
{
    public Rigidbody playerRb; // referencia al Rigidbody del personaje
    public float inclinacionMax = 15f; // grados m�ximo de inclinaci�n lateral
    public float suavizado = 5f; // cu�nto tarda en volver a la posici�n neutral
    public float factorVelocidad = 2f; // cu�nto afecta la velocidad
    public float factorAceleracion = 1f; // cu�nto afecta la aceleraci�n
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

        // Calcula aceleraci�n para efectos extra al cambio brusco
        float aceleracionX = (velocidadX - lastXVelocity) / Time.deltaTime;

        // Inclina la bandeja sobre X sumando a -90 seg�n velocidad + aceleraci�n
        float inclinacionX = Mathf.Clamp(-90f + (-velocidadX * factorVelocidad - aceleracionX * factorAceleracion), -90f - inclinacionMax, -90f + inclinacionMax);

        Quaternion rotacionDeseada = Quaternion.Euler(inclinacionX, 90f, -90f);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, rotacionDeseada, Time.deltaTime * suavizado);

        lastXVelocity = velocidadX;
    }
}
