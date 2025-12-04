namespace KFCConsoleApp.Src.Commons.Service
{
    internal class LanguageService
    {
        public static string Language = "uz";

        public static void SetLanguage(string lang)
        {
            switch (lang)
            {
                case "uz":
                    Language = "uz";
                    break;
                case "ru":
                    Language = "ru";
                    break;
                case "en":
                    Language = "en";
                    break;
                default:
                    Language = "uz";
                    break;
            }
        }
    }
}
