using System;
using UnityEngine;

public class PlayerStats
{
    // Debe coincidir con el campo JSON enviado a la API
    public string playerId;
    public int score;
    public int deaths;
    public string zone;

    // Constructor para la prueba de guardado
    public PlayerStats(string id, int newScore, int newDeaths, string newZone)
    {
        playerId = id;
        score = newScore;
        deaths = newDeaths;
        zone = newZone;
    }

    // Constructor vacío
    public PlayerStats() { }
}