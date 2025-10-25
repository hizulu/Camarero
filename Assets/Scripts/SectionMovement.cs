using UnityEngine;

public class SectionMovement : MonoBehaviour
{
    public int distanceRecord = 0;
    public float speed = 10f;

    private float startZ;

    void Start()
    {
        startZ = transform.position.z;
    }

    void Update()
    {
        UpdateMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DestroyerTrigger"))
        {
            Destroy(gameObject);
        }
    }

    private void UpdateMovement()
    {
        // Movimiento constante hacia atrás
        transform.position += Vector3.back * speed * Time.deltaTime;

        // Distancia recorrida desde el punto inicial
        float distance = Mathf.Abs(transform.position.z - startZ);
        int distanceInt = Mathf.FloorToInt(distance*0.2f);

        if (distanceInt > distanceRecord)
        {
            distanceRecord = distanceInt;
        }
    }
}
