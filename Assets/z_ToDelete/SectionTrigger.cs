using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    public GameObject[] sections;
    public float sectionLength = 50f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SectionTrigger"))
        {
             Transform currentSection = other.transform.root; 
             float newZ = currentSection.position.z + sectionLength;

            GameObject newSection = Instantiate(
                sections[Random.Range(0, sections.Length)],
                new Vector3(0, 0, newZ),
                Quaternion.identity
            );
        }
    }
}
