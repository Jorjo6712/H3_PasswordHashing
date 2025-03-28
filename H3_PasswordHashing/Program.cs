using System;
using System.Threading.Tasks;
using H3_PasswordHashing.Controllers;
using H3_PasswordHashing.Data;
using Microsoft.EntityFrameworkCore;

namespace H3_PasswordHashing
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Instantiate DbContext and HashController
            using var dbContext = new PasswordHashingDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<PasswordHashingDbContext>());
            dbContext.Database.OpenConnection(); // Open the in-memory SQLite DB
            dbContext.Database.EnsureCreated();  // Set up the schema
            var controller = new HashController(dbContext);

            Console.WriteLine("Welcome to the password hashing app!");

            // Keep the program running
            while (true)
            {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1 - Create a new user");
                Console.WriteLine("2 - Verify a password");
                Console.WriteLine("3 - Exit");
                Console.Write("Enter your choice: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("What is your username?");
                        string username = Console.ReadLine();
                        Console.WriteLine("What is your password?");
                        string password = Console.ReadLine();

                        // Call CreateUserAsync from the controller
                        string createUserMessage = await controller.CreateUserAsync(username, password);
                        Console.WriteLine(createUserMessage);
                        break;

                    case "2":
                        Console.WriteLine("What is your username?");
                        string verifyUsername = Console.ReadLine();
                        Console.WriteLine("What is your password?");
                        string verifyPassword = Console.ReadLine();

                        // Call VerifyPasswordAsync from the controller
                        string verifyPasswordMessage = await controller.VerifyPasswordAsync(verifyUsername, verifyPassword);
                        Console.WriteLine(verifyPasswordMessage);
                        break;

                    case "3":
                        Console.WriteLine("Exiting the application. Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please select a valid option.");
                        break;
                }
            }
        }
    }
}