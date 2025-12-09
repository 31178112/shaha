using Commons.Services;

namespace KFCConsoleApp.Features.Admin.Screens
{
    internal class AdminMenu
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
                Console.WriteLine("==        👨‍💼 Режим: Администратор        ==");
                Console.WriteLine("============================================");
                Console.WriteLine("==  1. 📊 Статистика заказов            ==");
                Console.WriteLine("==  2. 📦 Управление заказами           ==");
                Console.WriteLine("==  3. 📋 Управление меню               ==");
                Console.WriteLine("==  4. 👥 Просмотр пользователей        ==");
                Console.WriteLine("==  5. 👤 Мой профиль                   ==");
                Console.WriteLine("==  0. ↩️  Выйти из аккаунта              ==");
                Console.WriteLine("============================================");
                
                Console.Write("\nВыберите действие: ");
                string choice = Console.ReadLine() ?? "";
                
                switch (choice)
                {
                    case "1":
                        ShowStatistics();
                        break;
                    case "2":
                        ManageOrders();
                        break;
                    case "3":
                        ManageMenu();
                        break;
                    case "4":
                        ViewUsers();
                        break;
                    case "5":
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
        
        private static void ShowStatistics()
        {
            Console.Clear();
            Console.WriteLine("📊 Статистика заказов (скоро будет)...");
            Console.ReadKey();
        }
        
        private static void ManageOrders()
        {
            Console.Clear();
            Console.WriteLine("📦 Управление заказами (скоро будет)...");
            Console.ReadKey();
        }
        
        private static void ManageMenu()
        {
            Console.Clear();
            Console.WriteLine("📋 Управление меню (только просмотр для Admin)...");
            Console.ReadKey();
        }
        
        private static void ViewUsers()
        {
            Console.Clear();
            var users = AuthService.GetUsers();
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("============================================");
            Console.WriteLine("==           👥 Список пользователей     ==");
            Console.WriteLine("============================================");
            
            foreach (var user in users)
            {
                Console.WriteLine($"  {user.Id}. {user.Name} - {user.Email} ({user.Role})");
            }
            
            Console.WriteLine("============================================");
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
        
        private static void ShowProfile()
        {
            // Такой же как у User
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

