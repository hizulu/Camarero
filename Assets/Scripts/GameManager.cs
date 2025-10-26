using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pausaPanel;
    public GameObject endPanel;
    public GameObject pantallaPanel;

    [Header("UI Texts")]
    public TMP_Text distanceText;
    public TMP_Text finalDistanceText;

    [Header("Game References")]
    public SectionsManager sectionsManager;

    private bool gameEnded = false;

    void Start()
    {
        if (pausaPanel != null)
            pausaPanel.SetActive(false);

        if (endPanel != null)
            endPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pausaPanel != null)
            Pausa();
    }

    public void UpdateDistance(int distance)
    {
        if (distanceText != null)
            distanceText.text = "Distancia: " + distance + " m";
    }

    public void EndGame()
    {
        if (gameEnded) return;

        if (endPanel != null) endPanel.SetActive(true);
        if (pantallaPanel != null) pantallaPanel.SetActive(false);

        if (sectionsManager != null && finalDistanceText != null)
            finalDistanceText.text = "Distancia Recorrida: " + sectionsManager.distanceRecord.ToString("F0") + " m";

        Time.timeScale = 0f;
        gameEnded = true;
        Debug.Log("Game ended: No cups left on tray.");
    }

    #region Pause Menu

    public void Pausa()
    {
        Time.timeScale = 0;
        if (pausaPanel != null)
            pausaPanel.SetActive(true);
    }

    public void Reanudar()
    {
        Time.timeScale = 1;
        if (pausaPanel != null)
            pausaPanel.SetActive(false);
    }

    public void SalirPausa()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    #endregion

    public void Jugar()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void SalirMenuInicial()
    {
        Application.Quit();
        Debug.Log("SalirMenuInicial");
    }
}
