using UnityEngine;
using TMPro;

public class FilaRanking : MonoBehaviour
{
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI gamesPlayedText;
    public TextMeshProUGUI dateText;       

    public void SetData(int position, string name, int score)
    {
        positionText.text = position.ToString();
        playerNameText.text = name;
        scoreText.text = score.ToString();
    }
}
