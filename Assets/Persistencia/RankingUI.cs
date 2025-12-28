using UnityEngine;
using TMPro;

public class RankingUI : MonoBehaviour
{
    public WebGLPlayerData webGLPlayerData;
    public Transform contentPanel;
    public GameObject filaPrefab;

    void Start()
    {
        LoadRanking();
    }

    public void LoadRanking()
    {
        webGLPlayerData.GetRanking(players =>
        {
            players.Sort((a, b) => b.best_score.CompareTo(a.best_score));

            foreach (Transform child in contentPanel)
                Destroy(child.gameObject);

            int pos = 1;

            foreach (var p in players)
            {
                GameObject fila = Instantiate(filaPrefab, contentPanel);
                FilaRanking filaRanking = fila.GetComponent<FilaRanking>();

                // Formatear fecha
                string fechaFormateada = "";
                try
                {
                    fechaFormateada = System.DateTime.Parse(p.last_game_date)
                                     .ToString("dd-MM-yyyy");
                }
                catch
                {
                    fechaFormateada = p.last_game_date;
                }

                filaRanking.positionText.text = pos.ToString();
                filaRanking.playerNameText.text = p.username;
                filaRanking.scoreText.text = p.best_score.ToString();
                filaRanking.gamesPlayedText.text = p.games_played.ToString();
                filaRanking.dateText.text = fechaFormateada;

                pos++;
            }
        });
    }

    public void RefreshRanking()
    {
        LoadRanking();
    }
}
