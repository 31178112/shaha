using KFCConsoleApp.Src.Commons.Localization;

namespace KFCConsoleApp.Src.Features.Home.Screens
{
    internal class HomeScreen
    {
        public static void Build()
        {
        Menu:

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("============================================");
            Console.WriteLine($"==        🍗 {AppWords.Welcome.Translate()} 🍗");
            Console.WriteLine("============================================");
            Console.WriteLine($"==  1. {AppWords.Menu.Translate()}");
            Console.WriteLine($"==  2. {AppWords.ViewCart.Translate()}");
            Console.WriteLine($"==  3. {AppWords.Profile.Translate()}");
            Console.WriteLine($"==  0. {AppWords.Exit.Translate()}");
            Console.WriteLine("============================================");

            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine($"\n{AppWords.ChooseSection.Translate()}: ");
            Console.Write("--> ");

            Console.ResetColor();

            int section = Console.ReadLine().ParseStringToInt();

            switch (section)
            {
                case 0:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("==========================================");
                    Console.WriteLine("==     Thank you for visiting KFC!      ==");
                    Console.WriteLine("==          See you soon!               ==");
                    Console.WriteLine("==========================================");
                    Console.ResetColor();
                    break;
                case 1:
                    // TODO: Menu Screen
                    Console.WriteLine("Menu screen coming soon...");
                    Console.ReadLine();
                    Build();
                    break;
                case 2:
                    // TODO: Cart Screen  
                    Console.WriteLine("Cart screen coming soon...");
                    Console.ReadLine();
                    Build();
                    break;
                case 3:
                    // TODO: Profile Screen
                    Console.WriteLine("Profile screen coming soon...");
                    Console.ReadLine();
                    Build();
                    break;
                default:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("==========================================");
                    Console.WriteLine($"==     {AppWords.InvalidSection.Translate()}");
                    Console.WriteLine("==========================================");
                    Console.WriteLine($"==  0. {AppWords.Back.Translate()}");
                    Console.WriteLine("==========================================");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"\n{AppWords.ChooseSection.Translate()}: ");
                    Console.Write("--> ");
                    Console.ResetColor();
                    
                    section = Console.ReadLine().ParseStringToInt();
                    if (section == 0) goto Menu;
                    else goto default;
            }
        }
    }
}

