using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginUI : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public GameObject loginPanel;

    public static string playerUsername;

    private void Start()
    {
        Time.timeScale = 0f;
    }

    public void OnStartButton()
    {
        string username = usernameInput.text.Trim();

        if (string.IsNullOrEmpty(username))
        {
            Debug.LogWarning("Debes introducir un nombre de usuario.");
            return;
        }

        playerUsername = username;
        loginPanel.SetActive(false);
        Time.timeScale = 1f;

        Debug.Log("Jugador registrado: " + playerUsername);
        //gameManager.playerUsername = playerUsername;
        //gameManager.gamesPlayed += 1;
    }
}
