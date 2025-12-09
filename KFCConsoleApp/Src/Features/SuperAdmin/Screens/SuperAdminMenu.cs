using Commons.Enums;
using Commons.Services;

namespace KFCConsoleApp.Features.SuperAdmin.Screens
{
    internal class SuperAdminMenu
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
                Console.WriteLine("==       👑 Режим: Супер-Администратор   ==");
                Console.WriteLine("============================================");
                Console.WriteLine("==  1. 🏷️  Управление категориями        ==");
                Console.WriteLine("==  2. 📋 Управление продуктами          ==");
                Console.WriteLine("==  3. 👥 Управление пользователями      ==");
                Console.WriteLine("==  4. 👑 Назначить администратора       ==");
                Console.WriteLine("==  5. 📊 Вся статистика                 ==");
                Console.WriteLine("==  6. ⚙️  Настройки системы              ==");
                Console.WriteLine("==  7. 👤 Мой профиль                    ==");
                Console.WriteLine("==  0. ↩️  Выйти из аккаунта              ==");
                Console.WriteLine("============================================");
                
                Console.Write("\nВыберите действие: ");
                string choice = Console.ReadLine() ?? "";
                
                switch (choice)
                {
                    case "1":
                        CategoryManagementScreen.Show();
                        break;
                    case "2":
                        ProductManagementScreen.Show();
                        break;
                    case "3":
                        ManageUsers();
                        break;
                    case "4":
                        AssignAdmin();
                        break;
                    case "5":
                        ShowAllStatistics();
                        break;
                    case "6":
                        SystemSettings();
                        break;
                    case "7":
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
        
        private static void ManageUsers()
        {
            Console.Clear();
            var users = AuthService.GetUsers();
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("============================================");
            Console.WriteLine("==        👥 Управление пользователями   ==");
            Console.WriteLine("============================================");
            
            foreach (var user in users)
            {
                string status = user.IsActive ? "✅" : "❌";
                Console.WriteLine($"  {user.Id}. {user.Name} - {user.Email} ({user.Role}) {status}");
            }
            
            Console.WriteLine("============================================");
            Console.WriteLine("\nОпции: [B] Заблокировать, [A] Активировать, [R] Изменить роль");
            Console.Write("\nВыберите действие или [0] Назад: ");
            string choice = Console.ReadLine()?.ToUpper() ?? "";
            
            Console.WriteLine("\nФункционал управления пользователями в разработке...");
            Console.ReadKey();
        }
        
        private static void AssignAdmin()
        {
            Console.Clear();
            Console.WriteLine("👑 Назначить администратора");
            Console.WriteLine("\nВведите email пользователя для назначения администратором:");
            string email = Console.ReadLine() ?? "";
            
            Console.WriteLine("\nВыберите роль:");
            Console.WriteLine("  1. Администратор");
            Console.WriteLine("  2. Супер-Администратор");
            Console.Write("Выбор: ");
            string roleChoice = Console.ReadLine() ?? "";
            
            Console.WriteLine("\nФункционал назначения ролей в разработке...");
            Console.ReadKey();
        }
        
        private static void ShowAllStatistics()
        {
            Console.Clear();
            Console.WriteLine("📊 Вся статистика системы (скоро будет)...");
            Console.ReadKey();
        }
        
        private static void SystemSettings()
        {
            Console.Clear();
            Console.WriteLine("⚙️  Настройки системы (скоро будет)...");
            Console.ReadKey();
        }
        
        private static void ShowProfile()
        {
            Console.Clear();
            var user = AuthService.CurrentUser;
            
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("============================================");
            Console.WriteLine("==        👑 Профиль SuperAdmin          ==");
            Console.WriteLine("============================================");
            Console.WriteLine($"==  Имя: {user?.Name}");
            Console.WriteLine($"==  Email: {user?.Email}");
            Console.WriteLine($"==  Роль: {user?.Role} 👑");
            Console.WriteLine($"==  Дата регистрации: {user?.CreatedAt:dd.MM.yyyy}");
            Console.WriteLine("============================================");
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}
