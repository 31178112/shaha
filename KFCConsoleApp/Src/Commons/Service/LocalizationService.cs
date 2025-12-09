using System.Text.Json;

namespace Commons.Service
{
    /// <summary>
    /// Tillarni boshqarish va tarjima xizmati
    /// </summary>
    internal class LocalizationService
    {
        private static Dictionary<string, string>? _dictionary;
        private static readonly object _lock = new object();
        
        /// <summary>
        /// Hozirgi til lug'ati
        /// </summary>
        public static Dictionary<string, string> Dictionary
        {
            get
            {
                lock (_lock)
                {
                    return _dictionary ?? new Dictionary<string, string>();
                }
            }
            private set
            {
                lock (_lock)
                {
                    _dictionary = value;
                }
            }
        }

        /// <summary>
        /// Tilni yuklash (Async)
        /// </summary>
        public static async Task LoadLanguageAsync(string lang)
        {
            try
            {
                var dict = await LoadLanguageDictionaryAsync(lang);
                Dictionary = dict;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Tilni yuklashda xatolik: {ex.Message}");
                Dictionary = GetFallbackDictionary();
            }
        }

        /// <summary>
        /// Tilni yuklash (Sync - backward compatibility)
        /// </summary>
        public static void LoadLanguage(string lang)
        {
            try
            {
                var dict = LoadLanguageDictionarySync(lang);
                Dictionary = dict;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Tilni yuklashda xatolik: {ex.Message}");
                Dictionary = GetFallbackDictionary();
            }
        }

        /// <summary>
        /// Async til lug'atini yuklash
        /// </summary>
        private static async Task<Dictionary<string, string>> LoadLanguageDictionaryAsync(string lang)
        {
            if (string.IsNullOrWhiteSpace(lang))
            {
                throw new ArgumentException("Til parametri bo'sh bo'lishi mumkin emas", nameof(lang));
            }

            string localizationDir = GetLocalizationDirectory();
            string dictionaryFilePath = Path.Combine(localizationDir, $"{lang}.json");

            // Fayl yo'q bo'lsa, standart yaratamiz
            if (!File.Exists(dictionaryFilePath))
            {
                var fallbackDict = GetFallbackDictionary();
                await SaveDictionaryAsync(dictionaryFilePath, fallbackDict);
                return fallbackDict;
            }

            try
            {
                string jsonData = await File.ReadAllTextAsync(dictionaryFilePath);
                
                if (string.IsNullOrWhiteSpace(jsonData))
                {
                    throw new InvalidOperationException("JSON fayl bo'sh");
                }

                var dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonData);
                
                if (dictionary == null || dictionary.Count == 0)
                {
                    throw new InvalidOperationException("Lug'at bo'sh yoki noto'g'ri format");
                }

                return dictionary;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON parse xatolik: {ex.Message}");
                return GetFallbackDictionary();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fayl o'qish xatolik: {ex.Message}");
                return GetFallbackDictionary();
            }
        }

        /// <summary>
        /// Sync til lug'atini yuklash (backward compatibility)
        /// </summary>
        private static Dictionary<string, string> LoadLanguageDictionarySync(string lang)
        {
            if (string.IsNullOrWhiteSpace(lang))
            {
                throw new ArgumentException("Til parametri bo'sh bo'lishi mumkin emas", nameof(lang));
            }

            string localizationDir = GetLocalizationDirectory();
            string dictionaryFilePath = Path.Combine(localizationDir, $"{lang}.json");

            if (!File.Exists(dictionaryFilePath))
            {
                var fallbackDict = GetFallbackDictionary();
                SaveDictionarySync(dictionaryFilePath, fallbackDict);
                return fallbackDict;
            }

            try
            {
                string jsonData = File.ReadAllText(dictionaryFilePath);
                
                if (string.IsNullOrWhiteSpace(jsonData))
                {
                    throw new InvalidOperationException("JSON fayl bo'sh");
                }

                var dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonData);
                
                if (dictionary == null || dictionary.Count == 0)
                {
                    throw new InvalidOperationException("Lug'at bo'sh yoki noto'g'ri format");
                }

                return dictionary;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON parse xatolik: {ex.Message}");
                return GetFallbackDictionary();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Fayl o'qish xatolik: {ex.Message}");
                return GetFallbackDictionary();
            }
        }

        /// <summary>
        /// Localization papkasini olish va yaratish
        /// </summary>
        private static string GetLocalizationDirectory()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectDir = baseDir;

            // bin/Debug yoki bin/Release'dan chiqish
            if (baseDir.Contains("bin"))
            {
                var binIndex = baseDir.IndexOf("bin", StringComparison.OrdinalIgnoreCase);
                if (binIndex >= 0)
                {
                    projectDir = baseDir.Substring(0, binIndex);
                }
            }

            string localizationDir = Path.Combine(projectDir, "Src", "Commons", "Localization");

            // Papka yo'q bo'lsa yaratamiz
            if (!Directory.Exists(localizationDir))
            {
                try
                {
                    Directory.CreateDirectory(localizationDir);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Papka yaratishda xatolik: {ex.Message}");
                    // Agar yarata olmasak, joriy papkani ishlatamiz
                    localizationDir = Path.Combine(Directory.GetCurrentDirectory(), "Localization");
                    Directory.CreateDirectory(localizationDir);
                }
            }

            return localizationDir;
        }

        /// <summary>
        /// Standart lug'atni olish
        /// </summary>
        private static Dictionary<string, string> GetFallbackDictionary()
        {
            return new Dictionary<string, string>
            {
                ["choose_language"] = "Tilni tanlang / Choose language / Выберите язык",
                ["language_not_available"] = "Til mavjud emas",
                ["welcome"] = "KFC'ga xush kelibsiz!",
                ["home"] = "Bosh sahifa",
                ["back"] = "Orqaga",
                ["exit"] = "Chiqish",
                ["menu"] = "Menyu",
                ["products"] = "Mahsulotlar",
                ["cart"] = "Savat",
                ["profile"] = "Profil",
                ["invalid_section"] = "Noto'g'ri bo'lim",
                ["choose_section"] = "Bo'limni tanlang"
            };
        }

        /// <summary>
        /// Lug'atni faylga saqlash (Async)
        /// </summary>
        private static async Task SaveDictionaryAsync(string filePath, Dictionary<string, string> dictionary)
        {
            try
            {
                var options = new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                
                string json = JsonSerializer.Serialize(dictionary, options);
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Faylga yozishda xatolik: {ex.Message}");
            }
        }

        /// <summary>
        /// Lug'atni faylga saqlash (Sync)
        /// </summary>
        private static void SaveDictionarySync(string filePath, Dictionary<string, string> dictionary)
        {
            try
            {
                var options = new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                
                string json = JsonSerializer.Serialize(dictionary, options);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Faylga yozishda xatolik: {ex.Message}");
            }
        }

        /// <summary>
        /// Tarjima olish
        /// </summary>
        public static string Translate(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return key ?? string.Empty;
            }

            return Dictionary.GetValueOrDefault(key, key);
        }
    }
}
  
          
