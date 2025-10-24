using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    [Header("Prefabs de secciones")]
    public GameObject[] sections;

    [Header("Largo de cada secci�n (en unidades)")]
    public float sectionLength = 50f;

    private void OnTriggerEnter(Collider other)
    {
        // Este script est� en el jugador, por eso miramos si colisiona con el final del pasillo
        if (other.CompareTag("SectionTrigger"))
        {
            // Obtenemos el transform del trigger que tocamos
            Transform trigger = other.transform;

            // El padre del trigger es la secci�n que se est� terminando
            Transform sectionParent = trigger.root;

            // Calculamos la posici�n del nuevo pasillo
            // Se crea justo "adelante" del pasillo actual
            Vector3 spawnPos = sectionParent.position + sectionParent.forward * sectionLength;

            // Instanciamos una nueva secci�n aleatoria con la misma rotaci�n
            int index = Random.Range(0, sections.Length);
            Instantiate(sections[index], spawnPos, sectionParent.rotation);
        }
    }
}
