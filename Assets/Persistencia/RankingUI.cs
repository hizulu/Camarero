using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RankingUI : MonoBehaviour
{
    public WebGLPlayerData webGLPlayerData;
    public Transform contentPanel; // Content del ScrollView
    public GameObject rowPrefab;   // Prefab para cada fila

    void Start()
    {
        LoadRanking();
    }

    public void LoadRanking()
    {
        webGLPlayerData.GetRanking(players =>
        {
            // Ordenar por mejor puntuación
            players.Sort((a, b) => b.best_score.CompareTo(a.best_score));

            // Limpiar filas anteriores
            foreach (Transform child in contentPanel)
                Destroy(child.gameObject);

            // Crear filas
            foreach (var p in players)
            {
                GameObject row = Instantiate(rowPrefab, contentPanel);
                row.transform.Find("Username").GetComponent<Text>().text = p.username;
                row.transform.Find("BestScore").GetComponent<Text>().text = p.best_score.ToString();
                row.transform.Find("GamesPlayed").GetComponent<Text>().text = p.games_played.ToString();
                row.transform.Find("LastGameDate").GetComponent<Text>().text = p.last_game_date;
            }
        });
    }
}
