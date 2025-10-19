using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TrayMovement : MonoBehaviour
{
    [Header("Referencias")]
    public Rigidbody playerRb;                 // Rigidbody del jugador
    public Transform[] cups;                   // Lista de copas sobre la bandeja

    [Header("Seguimiento")]
    public float yOffset = 2f;            // Altura sobre el jugador
    public float xOffset = 2f;                 // Desplazamiento lateral respecto al jugador
    public float zOffset = 1.25f;                 // Desplazamiento en Z respecto al jugador
    public float followSmooth = 10f;           // Suavizado del seguimiento lateral

    [Header("Inclinación")]
    public float maxTilt = 15f;                // Máxima inclinación en Z
    public float tiltSmooth = 5f;              // Suavizado de la rotación

    private Rigidbody rb;
    private Vector3 previousPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;                 // Bandeja controlada por script

        // Ignorar colisión entre bandeja y jugador
        Collider trayCollider = GetComponent<Collider>();
        Collider playerCollider = playerRb.GetComponent<Collider>();
        Physics.IgnoreCollision(trayCollider, playerCollider);

        previousPosition = transform.position;
    }

    void FixedUpdate()
    {
        Vector3 deltaMovement = FollowPlayer();
        MoveCupsWithTray(deltaMovement);
        TiltBasedOnVelocity();
    }

    Vector3 FollowPlayer()
    {
        // Posición objetivo lateral + altura + offset Z relativo al jugador
        Vector3 targetPos = new Vector3(
            playerRb.position.x + xOffset,
            playerRb.position.y + yOffset,
            playerRb.position.z + zOffset
        );

        // Suavizado
        Vector3 newPos = Vector3.Lerp(transform.position, targetPos, followSmooth * Time.fixedDeltaTime);

        // Delta de movimiento para mover las copas
        Vector3 delta = newPos - transform.position;

        // Aplicar posición
        transform.position = newPos;

        previousPosition = transform.position;

        return delta;
    }


    void MoveCupsWithTray(Vector3 delta)
    {
        foreach (Transform cup in cups)
        {
            Rigidbody cupRb = cup.GetComponent<Rigidbody>();
            if (cupRb != null && cupRb.isKinematic)
            {
                cup.position += delta; // mover copas kinematic con la bandeja
            }
        }
    }

    void TiltBasedOnVelocity()
    {
        float horizontalVel = playerRb.velocity.x;

        float targetZRotation = Mathf.Clamp(-horizontalVel * 3f, -maxTilt, maxTilt);
        Quaternion targetRot = Quaternion.Euler(0, 0, targetZRotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, tiltSmooth * Time.fixedDeltaTime);
    }
}
