using System;

[System.Serializable]
public class UserDTO
{
    public string Name { get; set; }
    public string Email { get; set; }

    // Constructor sin parámetros (necesario para JsonUtility)
    public UserDTO() { }

    // Constructor con parámetros
    public UserDTO(string name, string email)
    {
        Name = name;
        Email = email;
    }
}