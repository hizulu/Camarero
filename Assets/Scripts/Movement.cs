using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SmoothMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxSpeed = 10f;           // velocidad máxima
    public float acceleration = 20f;       // cuánto acelera al mantener la tecla
    public float deceleration = 30f;       // cuánto frena al soltar o cambiar dirección
    public float maxXRange = 5f;

    private Rigidbody rb;
    private float targetSpeed = 0f;        // velocidad objetivo según input

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    }

    private void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Horizontal");
        targetSpeed = moveInput * maxSpeed;

        float speedDifference = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        // Calcula la velocidad incremental según aceleración o desaceleración
        float movement = Mathf.Clamp(speedDifference, -accelRate * Time.fixedDeltaTime, accelRate * Time.fixedDeltaTime);
        float newVelocityX = rb.velocity.x + movement;

        // Aplica la nueva velocidad
        rb.velocity = new Vector3(newVelocityX, rb.velocity.y, 0f);

        // Limita la posición X
        Vector3 clampedPosition = rb.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -maxXRange, maxXRange);
        rb.position = clampedPosition;
    }
}
