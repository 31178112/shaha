using KFCConsoleApp.Features.Home.Screens;
using Commons;
using Commons.Services;

namespace KFCConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Initializer.Initialize();
                var homeScreen = new HomeScreen();
                homeScreen.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка запуска: {ex.Message}");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }
    }
}
