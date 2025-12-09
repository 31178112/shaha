using System;
using System.Security.Cryptography;
using System.Text;

public class HashTest
{
    public static void RunTest()
    {
        Console.WriteLine("=== ТЕСТ ХЭШИРОВАНИЯ ===");
        
        string testPass1 = "admin123";
        string testPass2 = "superadmin123";
        
        Console.WriteLine($"Пароль 1: '{testPass1}'");
        Console.WriteLine($"Пароль 2: '{testPass2}'");
        Console.WriteLine();
        
        // Хэш методом из UserModel
        Console.WriteLine("Хэш методом UserModel.HashPassword():");
        Console.WriteLine($"admin123: {HashPasswordMethod(testPass1)}");
        Console.WriteLine($"superadmin123: {HashPasswordMethod(testPass2)}");
        Console.WriteLine();
        
        // Что сейчас в users.json
        Console.WriteLine("=== ТЕКУЩИЕ ХЭШИ В users.json ===");
        try
        {
            string jsonPath = "Data/users.json";
            if (File.Exists(jsonPath))
            {
                string json = File.ReadAllText(jsonPath);
                Console.WriteLine(json);
            }
            else
            {
                Console.WriteLine("Файл users.json не существует!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка чтения файла: {ex.Message}");
        }
    }
    
    // Копия метода из UserModel
    private static string HashPasswordMethod(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
