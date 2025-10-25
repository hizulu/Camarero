using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public void Jugar()
    {
        //Cargar escena 1
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void Salir()
    {
        //Salir de la aplicacion
        Application.Quit();
    }
}
