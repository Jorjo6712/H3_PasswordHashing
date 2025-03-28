using System.ComponentModel.DataAnnotations;

namespace H3_PasswordHashing.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public string Hash { get; set; }
    
    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }
}