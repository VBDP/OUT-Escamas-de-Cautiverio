using System;

[System.Serializable]
public class ScoreDTO
{
    public string Name { get; set; }
    public int Score { get; set; }

    public ScoreDTO() { }

    public ScoreDTO(string name, int score)
    {
        Name = name;
        Score = score;
    }
}