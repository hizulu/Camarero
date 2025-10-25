using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cup : MonoBehaviour
{
    [Header("Ajustes de caída")]
    public float dropAngleThreshold = 20f;       // grados de inclinación de la bandeja para que la copa caiga
    public float dropAccelerationThreshold = 40f;// si la aceleración acumulada supera esto, cae
    public float additionalDropVelocity = 2f;    // empujón adicional al caer
    public float collisionDropImpact = 2f;       // impacto mínimo para caer al chocar con un obstáculo

    [Header("Referencias (se asignan en Start si no se ponen)")]
    public Rigidbody rb;
    public Collider col;
    public TrayInventory ownerTray;              // será asignado al ser hija de la bandeja

    bool isOnTray = true;

    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (col == null) col = GetComponent<Collider>();

        // Si la copa es hija de la bandeja en escena, registrarla automáticamente
        Transform p = transform.parent;
        if (p != null)
        {
            ownerTray = p.GetComponentInParent<TrayInventory>();
            if (ownerTray != null)
            {
                ownerTray.AddCup(this);
            }
        }

        // Mientras está sobre la bandeja, usar kinematic para "pegarla" al transform.
        rb.isKinematic = isOnTray;
    }

    private void Update()
    {
        if (!isOnTray) return;
        if (ownerTray == null) return;

        // Comprobación por inclinación: ángulo entre el up de la bandeja y el up global
        float trayAngle = Vector3.Angle(ownerTray.trayTransform.up, Vector3.up);
        if (trayAngle > dropAngleThreshold)
        {
            Drop();
            return;
        }

        // Comprobación por aceleración (usa la propiedad exposada en PlayerMovement)
        if (ownerTray.playerMovement != null && ownerTray.playerMovement.CurrentAcceleration > dropAccelerationThreshold)
        {
            Drop();
            return;
        }
    }

    public void Drop()
    {
        if (!isOnTray) return;
        isOnTray = false;

        // deshijar para física independiente
        transform.parent = null;
        rb.isKinematic = false;

        // dar velocidad inicial: la del jugador + componente por la inclinación de la bandeja
        Vector3 baseVel = Vector3.zero;
        if (ownerTray != null && ownerTray.playerRb != null) baseVel = ownerTray.playerRb.velocity;

        // calcular empujón según la normal de la bandeja (hacia abajo de la bandeja)
        Vector3 trayDown = ownerTray != null ? -ownerTray.trayTransform.up : Vector3.down;
        Vector3 added = trayDown * additionalDropVelocity;

        rb.velocity = baseVel + added;

        // avisar al inventario
        if (ownerTray != null) ownerTray.RemoveCup(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Si estaba sobre la bandeja no se considera; si no: detectar colisión con suelo/obstáculo
        if (isOnTray) return;

        // Si choca con un obstáculo (tag "Obstacle") con una fuerza suficiente, hacer algo (opcional)
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            float impact = collision.relativeVelocity.magnitude;
            if (impact > collisionDropImpact)
            {
                // por ejemplo, podemos desactivar física o marcarla como "rota", etc.
                // aquí no hacemos nada extra, pero lo dejo para que personalices.
            }
        }
    }
}
