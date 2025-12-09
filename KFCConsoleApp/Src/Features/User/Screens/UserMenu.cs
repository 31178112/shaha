using Commons.Services;

namespace KFCConsoleApp.Features.User.Screens
{
    internal class UserMenu
    {
        public static void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                
                var user = AuthService.CurrentUser;
                
                Console.WriteLine("============================================");
                Console.WriteLine($"==   🍗 Добро пожаловать, {user?.Name}! 🍗  ==");
                Console.WriteLine("==          👤 Режим: Пользователь        ==");
                Console.WriteLine("============================================");
                Console.WriteLine("==  1. 🍗 Меню KFC                        ==");
                Console.WriteLine("==  2. 🛒 Моя корзина                     ==");
                Console.WriteLine("==  3. 📦 Мои заказы                      ==");
                Console.WriteLine("==  4. 👤 Мой профиль                     ==");
                Console.WriteLine("==  0. ↩️  Выйти из аккаунта               ==");
                Console.WriteLine("============================================");
                
                Console.Write("\nВыберите действие: ");
                string choice = Console.ReadLine() ?? "";
                
                switch (choice)
                {
                    case "1":
                        KfcMenuScreen.Show();
                        break;
                    case "2":
                        CartScreen.Show();
                        break;
                    case "3":
                        // TODO: Мои заказы
                        Console.WriteLine("\n📦 Мои заказы (скоро будет)...");
                        Console.ReadKey();
                        break;
                    case "4":
                        ShowProfile();
                        break;
                    case "0":
                        AuthService.Logout();
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n❌ Неверный выбор!");
                        Console.ResetColor();
                        Thread.Sleep(1000);
                        break;
                }
            }
        }
        
        private static void ShowProfile()
        {
            Console.Clear();
            var user = AuthService.CurrentUser;
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("============================================");
            Console.WriteLine("==             👤 Мой профиль            ==");
            Console.WriteLine("============================================");
            Console.WriteLine($"==  Имя: {user?.Name}");
            Console.WriteLine($"==  Email: {user?.Email}");
            Console.WriteLine($"==  Роль: {user?.Role}");
            Console.WriteLine($"==  Дата регистрации: {user?.CreatedAt:dd.MM.yyyy}");
            Console.WriteLine("============================================");
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}
