using System;

[System.Serializable]
public class PostScoreDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Score { get; set; }
    public string Token { get; set; }

    public PostScoreDTO() { }

    public PostScoreDTO(string name, string email, int score, string token)
    {
        Name = name;
        Email = email;
        Score = score;
        Token = token;
    }
}