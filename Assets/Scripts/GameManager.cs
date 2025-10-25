using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class GameManager : MonoBehaviour
{
    public Canvas pausaCanvas;
    public TMPro.TextMeshProUGUI distanceText;
    SectionMovement sectionMovement;

    public void Start()
    {        
        pausaCanvas.enabled = false;
        sectionMovement = FindObjectOfType<SectionMovement>();
    }

    public void Jugar()
    {
        //Cargar escena 1
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void SalirMenuInicial()
    {
        //SalirMenuInicial de la aplicacion
        Application.Quit();
        Debug.Log("SalirMenuInicial");
    }

    public void Pausa()
    {
        //Pausar el juego
        Time.timeScale = 0;
        pausaCanvas.enabled = true;

    }

    public void Reanudar()
    {
        //Reanudar el juego
        Time.timeScale = 1;
        pausaCanvas.enabled = false;
    }

    public void SalirPausa()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    //Detectar ESC para pausar el juego
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pausaCanvas!=null)
        {
            Pausa();
        }
    }

    private void FixedUpdate()
    {
        if (distanceText != null && sectionMovement != null)
        {
            distanceText.text = "Distancia Recorrida: " + sectionMovement.distanceRecord.ToString() + " m";
        }
    }
}
