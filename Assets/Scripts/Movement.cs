using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxSpeed = 10f;
    public float baseAcceleration = 20f;
    public float initialAcceleration = 50f; // Aceleración inicial para que arranque rápido
    public float maxAccumulatedAcceleration = 50f;
    public float maxXRange = 5f;

    private float currentAcceleration = 0f;
    private float moveInput;
    private Rigidbody rb;
    private float targetSpeed = 0f;
    private bool justStartedMoving = false;

    public float CurrentAcceleration => currentAcceleration;


    public TextMeshProUGUI accelText;
    public TextMeshProUGUI velText;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    }

    private void FixedUpdate()
    {
        ReadInput();
        UpdateTargetSpeed();
        HandleMovement();
        ClampPosition();
        UpdateText();
    }

    private void ReadInput()
    {
        moveInput = Input.GetAxis("Horizontal");
    }

    private void UpdateTargetSpeed()
    {
        targetSpeed = moveInput * maxSpeed;
    }

    private void HandleMovement()
    {
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            ApplyAcceleration();
            MoveCharacter();
        }
        else
        {
            StopCharacter();
        }
    }

    private void ApplyAcceleration()
    {
        if (Mathf.Abs(rb.velocity.x) < 0.1f && !justStartedMoving)
        {
            // Primera pulsación: aceleración fuerte para arrancar
            currentAcceleration = initialAcceleration;
            justStartedMoving = true;
        }
        else
        {
            // Acumulación normal
            currentAcceleration += baseAcceleration * Time.fixedDeltaTime;
            currentAcceleration = Mathf.Min(currentAcceleration, maxAccumulatedAcceleration);
        }
    }

    private void MoveCharacter()
    {
        float speedDifference = targetSpeed - rb.velocity.x;
        float movement = Mathf.Clamp(speedDifference, -currentAcceleration * Time.fixedDeltaTime, currentAcceleration * Time.fixedDeltaTime);
        rb.velocity = new Vector3(rb.velocity.x + movement, rb.velocity.y, 0f);
    }

    private void StopCharacter()
    {
        rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        currentAcceleration = 0f;
        justStartedMoving = false;
    }

    private void ClampPosition()
    {
        Vector3 clampedPosition = rb.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -maxXRange, maxXRange);
        rb.position = clampedPosition;
    }

    private void UpdateText()
    {
        accelText.text = currentAcceleration.ToString("F2");
        velText.text = rb.velocity.x.ToString("F2");
    }
}
