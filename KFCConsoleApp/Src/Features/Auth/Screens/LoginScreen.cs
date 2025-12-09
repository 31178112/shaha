using Commons.Services;
using KFCConsoleApp.Features.SuperAdmin.Screens;
using KFCConsoleApp.Features.Admin.Screens;
using KFCConsoleApp.Features.User.Screens;

namespace KFCConsoleApp.Features.Auth.Screens
{
    public class LoginScreen
    {
        private readonly IAuthService _authService;

        public LoginScreen()
        {
            _authService = Initializer.AuthService;
        }

        public void Show()
        {
            Console.Clear();
            Console.WriteLine("Вход в систему");
            Console.WriteLine("==============");

            Console.Write("Имя пользователя: ");
            var username = Console.ReadLine();

            Console.Write("Пароль: ");
            var password = Console.ReadLine();

            if (_authService.Login(username, password))
            {
                Console.WriteLine("Успешный вход!");
                Console.ReadKey();

                var user = _authService.GetUser(username);
                
                if (_authService.IsSuperAdmin(username))
                {
                    var superAdminMenu = new SuperAdminMenu();
                    superAdminMenu.Show(user);
                }
                else if (_authService.IsAdmin(username))
                {
                    var adminMenu = new AdminMenu();
                    adminMenu.Show(user);
                }
                else
                {
                    var userMenu = new UserMenu();
                    userMenu.Show(user);
                }
            }
            else
            {
                Console.WriteLine("Неверное имя пользователя или пароль!");
                Console.ReadKey();
                Show();
            }
        }
    }
}
