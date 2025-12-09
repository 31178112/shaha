using Commons.Enums;
using Commons.Services;

namespace KFCConsoleApp.Features.Auth.Screens
{
    internal class RegisterScreen
    {
        public static void Show()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            
            Console.WriteLine("============================================");
            Console.WriteLine("==        🍗 KFC Регистрация 🍗          ==");
            Console.WriteLine("============================================");
            
            Console.Write("\nИмя: ");
            string name = Console.ReadLine() ?? "";
            
            Console.Write("Email: ");
            string email = Console.ReadLine() ?? "";
            
            Console.Write("Пароль: ");
            string password = Console.ReadLine() ?? "";
            
            // По умолчанию регистрируем как обычного пользователя
            if (AuthService.Register(name, email, password, UserRole.User))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✅ Регистрация успешна! Теперь войдите в систему.");
                Console.ResetColor();
                Thread.Sleep(2000);
                LoginScreen.Show();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n❌ Ошибка регистрации!");
                Console.ResetColor();
                Thread.Sleep(2000);
                Show();
            }
        }
    }
}
