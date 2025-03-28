using H3_PasswordHashing.Data;
using H3_PasswordHashing.Models;
using H3_PasswordHashing.Utils;
using Microsoft.EntityFrameworkCore;

namespace H3_PasswordHashing.Controllers;

public class HashController
{
    private readonly PasswordHashingDbContext _context;

    public HashController(PasswordHashingDbContext context)
    {
        _context = context;
    }
    
    public async Task<string> CreateUserAsync(string username, string password)
    {
        if (!HashUtils.PasswordUtils.IsValid(password))
        {
            return "Password does not meet strength requirements.";
        }

        var (hashedPassword, salt) = HashUtils.PasswordUtils.HashPassword(password);

        User user = new User(username, password)
        {
            Password = hashedPassword,
            Salt = salt,
            Hash = hashedPassword
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();


        return $"User successfully created with hashed password.\nHash stored in the database: {hashedPassword}";
    }
    
    public async Task<string> VerifyPasswordAsync(string username, string password)
    {
        User user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            return "User not found.";
        }

        bool isPasswordValid = HashUtils.PasswordUtils.VerifyPassword(password, user.Password, user.Salt);
        
        return isPasswordValid
            ? $"Password verification successful.\nHash stored in the database: {user.Hash}"
            : "Invalid password.";
    }
}