using System;

[Serializable]
public class PlayerData
{
    public string playerName;
    public int level;
    public int score;
    
    // Posição 2D do jogador
    public float posX;
    public float posY;

    // Valores padrão ao iniciar um jogo novo
    public PlayerData()
    {
        playerName = "Jogador 2D";
        level = 1;
        score = 0;
        posX = 0f;
        posY = 0f;
    }
}