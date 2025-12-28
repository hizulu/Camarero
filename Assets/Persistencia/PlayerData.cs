using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public string username;
    public int best_score;
    public int games_played;
    public string last_game_date;
}

// Wrapper para listas JSON
[Serializable]
public class PlayerDataList
{
    public List<PlayerData> players;
}
