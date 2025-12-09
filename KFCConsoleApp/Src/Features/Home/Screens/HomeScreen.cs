using KFCConsoleApp.Features.Auth.Screens;

namespace KFCConsoleApp.Features.Home.Screens
{
    public class HomeScreen
    {
        public void Show()
        {
            Console.Clear();
            Console.WriteLine("Добро пожаловать в KFC Console App!");
            Console.WriteLine("===============================");
            Console.WriteLine("1. Вход");
            Console.WriteLine("2. Регистрация");
            Console.WriteLine("3. Выход");
            Console.Write("Выберите опцию: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var loginScreen = new LoginScreen();
                    loginScreen.Show();
                    break;
                case "2":
                    var registerScreen = new RegisterScreen();
                    registerScreen.Show();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Неверный выбор!");
                    Console.ReadKey();
                    Show();
                    break;
            }
        }
    }
}
