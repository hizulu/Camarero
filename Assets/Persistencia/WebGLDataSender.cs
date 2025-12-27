using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class WebGLPlayerData : MonoBehaviour
{
    private string baseUrl = "http://localhost:3000/api/players"; // Cambia si tu backend está en otro host

    [System.Serializable]
    public class PlayerData
    {
        public string username;
        public int best_score;
        public int games_played;
        public string last_game_date;
    }

    public void SendPlayerData(string username, int bestScore, int gamesPlayed)
    {
        string dateNow = System.DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        PlayerData player = new PlayerData
        {
            username = username,
            best_score = bestScore,
            games_played = gamesPlayed,
            last_game_date = dateNow
        };
        StartCoroutine(PostPlayerData(player));
    }

    IEnumerator PostPlayerData(PlayerData player)
    {
        string json = JsonUtility.ToJson(player);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(baseUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("Datos enviados correctamente al backend.");
        else
            Debug.LogError("Error al enviar datos: " + request.error);
    }
}
