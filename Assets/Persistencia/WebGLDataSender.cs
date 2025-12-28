using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class WebGLPlayerData : MonoBehaviour
{
    public string baseUrl = "https://camarero-in0r.onrender.com/api/players";

    public void SendPlayerData(string username, int bestScore, int gamesPlayed)
    {
        string dateNow = System.DateTime.UtcNow.ToString("dd-MM-yyyy");
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

    public void GetRanking(System.Action<List<PlayerData>> callback)
    {
        StartCoroutine(GetRankingCoroutine(callback));
    }

    IEnumerator GetRankingCoroutine(System.Action<List<PlayerData>> callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(baseUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;

            // El JSON debe ser un array
            if (!json.StartsWith("["))
            {
                Debug.LogError("Error: JSON no es un array válido: " + json);
                yield break;
            }

            // Envolver el array en un objeto para JsonUtility
            string wrappedJson = "{\"players\":" + json + "}";

            try
            {
                PlayerDataList dataList = JsonUtility.FromJson<PlayerDataList>(wrappedJson);
                if (dataList != null && dataList.players != null)
                    callback?.Invoke(dataList.players);
                else
                    Debug.LogError("Lista de jugadores vacía o JSON mal formado");
            }
            catch (Exception e)
            {
                Debug.LogError("Error parseando JSON: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("Error al obtener ranking: " + request.error);
        }
    }
}