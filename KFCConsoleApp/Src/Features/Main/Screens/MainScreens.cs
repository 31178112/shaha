using KFCConsoleApp.Src.Commons.Localization;
using KFCConsoleApp.Src.Commons.Service;
using KFCConsoleApp.Src.Features.Home.Screens;

namespace KFCConsoleApp.Src.Features.Main.Screens
{
    internal class MainScreens
    {
        public static void Build()
        {
            
            int count = 0;
        ChooseLanguage:

            Console.Clear();

            Console.ForegroundColor = count == 0 ? ConsoleColor.Cyan : ConsoleColor.Red;

            Console.WriteLine("======================================================================================");
            LanguageService.SetLanguage("en");
            Console.WriteLine($"==   {(count == 0 ? AppWords.ChooseLanguage : AppWords.LanguageNotAvailable).Translate()}");
            LanguageService.SetLanguage("ru");
            Console.WriteLine($"==   {(count == 0 ? AppWords.ChooseLanguage : AppWords.LanguageNotAvailable).Translate()}");
            LanguageService.SetLanguage("uz");
            Console.WriteLine($"==   {(count == 0 ? AppWords.ChooseLanguage : AppWords.LanguageNotAvailable).Translate()}");
            Console.WriteLine("======================================================================================");
            Console.WriteLine($"==  1. English");
            Console.WriteLine($"==  2. Русский");
            Console.WriteLine($"==  3. O'zbek");
            Console.WriteLine("======================================================================================");

            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write("\n--> ");

            Console.ResetColor();

            int section = Console.ReadLine().ParseStringToInt();

            switch (section)
            {
                case 1:
                    LanguageService.SetLanguage("en");
                    break;
                case 2:
                    LanguageService.SetLanguage("ru");
                    break;
                case 3:
                    LanguageService.SetLanguage("uz");
                    break;
                default:
                    count++;
                    goto ChooseLanguage;
            }

            // После выбора языка переходим на главный экран KFC
            HomeScreen.Build();
        }
    }
}
