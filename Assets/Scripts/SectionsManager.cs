using UnityEngine;
using TMPro;

public class SectionsManager : MonoBehaviour
{
    [Header("Prefabs de secciones")]
    public GameObject[] sections;

    [Header("Largo de cada sección (en unidades)")]
    public float sectionLength = 100f;

    [Header("Movement Settings")]
    public float speed = 10f;
    public float acceleration = 1f;

    [Header("Distancia Recorrida")]
    public int distanceRecord = 0;
    private float startZ;
    public TMP_Text distanceText;

    void Start()
    {
        startZ = transform.position.z;
    }

    void Update()
    {
        UpdateMovement();
        acceleration += 0.0001f;  // Increase acceleration over time
    }

    // Call this method when the player hits a "SectionTrigger" (e.g., from a collider on the player or container)
    public void SpawnSection(Transform triggerTransform)
    {
        // Find the highest z-position among existing sections (to spawn ahead without overlap)
        float maxZ = float.NegativeInfinity;
        Quaternion lastRotation = Quaternion.identity;  // Default rotation

        foreach (Transform child in transform)
        {
            if (child.position.z > maxZ)
            {
                maxZ = child.position.z;
                lastRotation = child.rotation;  // Match the rotation of the last section
            }
        }

        // If no sections exist, start from the container's position
        if (maxZ == float.NegativeInfinity)
        {
            maxZ = transform.position.z;
        }

        // Calculate spawn position ahead of the last section
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, maxZ + sectionLength);

        // Instantiate a random section as a child of the container
        int index = Random.Range(0, sections.Length);
        Instantiate(sections[index], spawnPos, lastRotation, transform);
    }

    private void UpdateMovement()
    {
        // Move the container backward with accelerating speed
        transform.position += Vector3.back * speed * acceleration * Time.deltaTime;
        UpdateDistanceRecord();
    }

    private void UpdateDistanceRecord()
    {
        // Distance traveled from the start
        float distance = Mathf.Abs(transform.position.z - startZ);
        int distanceInt = Mathf.FloorToInt(distance * 0.2f);

        if (distanceInt > distanceRecord)
        {
            distanceRecord = distanceInt;
            // Update UI only when the record changes
            distanceText.text = "Distancia Recorrida: " + distanceRecord.ToString() + " m";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DestroyerTrigger"))
        {
            // Destroy the section that entered the destroyer trigger
            Destroy(other.transform.parent.gameObject);
        }
    }
}