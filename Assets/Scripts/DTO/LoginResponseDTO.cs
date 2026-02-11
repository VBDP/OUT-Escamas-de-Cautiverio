[System.Serializable]
public class LoginResponseDTO
{
    public string Name;
    public string Email;
    public string Token;

    public bool Success;   // <-- Esto indica si el login fue exitoso
    public string Message; // <-- Opcional, puede contener error o info del servidor

    public LoginResponseDTO() { }

    public LoginResponseDTO(string name, string email, string token, bool success, string message = "")
    {
        Name = name;
        Email = email;
        Token = token;
        Success = success;
        Message = message;
    }
}