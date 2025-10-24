using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    public GameObject[] sections;

    private void OnTriggerEnter(Collider other)
    {
        //No puede ser la tag player porque es el jugador el que detecta la colision
        if (other.gameObject.CompareTag("SectionTrigger"))
        {
            Instantiate(sections[Random.Range(0, sections.Length)], new Vector3(0, 0, 50), Quaternion.identity);
        }
    }
}