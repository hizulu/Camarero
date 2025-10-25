using UnityEngine;

public class Cups : MonoBehaviour
{
    [Header("Scoring")]
    public int pointsPerSecond = 10; // Puntos ganados por segundo mientras est� en la bandeja

    private bool isOnTray = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Aseg�rate de que la gravedad est� activada para que caiga
        rb.useGravity = true;
    }

    void Update()
    {
        if (isOnTray)
        {
            // Gana puntos mientras est� en la bandeja (puedes integrar con un gestor de puntuaci�n global)
            // Por simplicidad, registra en consola; reemplaza con tu sistema de puntuaci�n
            Debug.Log("Ganando puntos: " + pointsPerSecond * Time.deltaTime);
        }
    }

    // Llamado por TrayMovement cuando entra/sale de la bandeja
    public void SetOnTray(bool onTray)
    {
        isOnTray = onTray;
        if (!onTray)
        {
            // Opcional: Agrega efectos al caer, e.g., sonido o part�culas
            Debug.Log("�Copa cay�!");
        }
    }

    // Opcional: Reinicia la copa si cae demasiado lejos (e.g., respawnea o destruye)
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // Asumiendo que el suelo tiene este tag
        {
            // Si est� en la bandeja, notifica a TrayMovement para decrementar el contador antes de destruirse
            if (isOnTray)
            {
                TrayMovement tray = FindObjectOfType<TrayMovement>();
                if (tray != null)
                {
                    tray.DecrementCupCount();
                }
            }
            // Destruye o respawnea la copa
            Destroy(gameObject);
        }
    }

    public bool IsOnTray()
    {
        return isOnTray;
    }

}