using System.Text.Json;

namespace KFCConsoleApp.Src.Commons.Service
{
    internal class LocalizationService
    {
        private static Dictionary<string, string>? uz;
        private static Dictionary<string, string>? ru;
        private static Dictionary<string, string>? en;

        public static Dictionary<string, string>? dictionary
        {
            get
            {
                var lang = LanguageService.Language.ToString().ToLower();
                
                switch (lang)
                {       
                    case "uz":
                        if (uz == null) uz = getDictionaryInFile("uz");
                        return uz;
                    case "ru":
                        if (ru == null) ru = getDictionaryInFile("ru");
                        return ru;
                    case "en":
                        if (en == null) en = getDictionaryInFile("en");
                        return en;
                    default:
                        if (uz == null) uz = getDictionaryInFile("uz");
                        return uz;
                }
            }
        }

public static Dictionary<string, string> getDictionaryInFile(string lang)
{
    // Простой и надежный способ: используем относительный путь от текущей директории
    string baseDir = AppDomain.CurrentDomain.BaseDirectory;

    Console.WriteLine($"Base Directory: {baseDir}");
    
    // Если мы в bin/Debug/net8.0/, поднимаемся на 3 уровня вверх
    string projectDir = baseDir;
    
    // Проверяем разные возможные расположения
    if (baseDir.Contains("bin/Debug/") || baseDir.Contains("bin/Release/"))
    {
        projectDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", ".."));
    }
    
    string localizationDir = Path.Combine( "Src", "Commons", "Localization");

    // Создаем директорию если не существует
    if (!Directory.Exists(localizationDir))
    {
        Directory.CreateDirectory(localizationDir);
    }

    string dictionaryFilePath = Path.Combine(localizationDir, $"{lang}.json");

    // Если файл не существует, создаем его с базовым содержимым
    if (!File.Exists(dictionaryFilePath))
    {
        // Используем запасной вариант - создаем базовый словарь
        var fallbackDict = new Dictionary<string, string>
        {
            ["choose_language"] = "Please choose language",
            ["language_not_available"] = "Language not available",
            ["welcome"] = "Welcome to KFC"
        };
        
        string json = JsonSerializer.Serialize(fallbackDict, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(dictionaryFilePath, json);
        return fallbackDict;
    }

    try
    {
        string data = File.ReadAllText(dictionaryFilePath);
        return JsonSerializer.Deserialize<Dictionary<string, string>>(data) ?? new Dictionary<string, string>();
    }
    catch
    {
        return new Dictionary<string, string>();
    }
}
    }
}

  
          
