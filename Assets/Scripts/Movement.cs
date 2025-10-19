using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float acceleration = 10f;
    public float maxXRange = 5f;

    Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        //Bloquea la rotacion y el movimiento en Z
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    }

    private void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector3 targetVelocity = new Vector3(moveInput * speed, rb.velocity.y, 0);

        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, acceleration * Time.deltaTime);
        // Hace que el objeto no se salga del rango en X
        Vector3 clampedPosition = rb.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -maxXRange, maxXRange);
        rb.position = clampedPosition;
    }
}
