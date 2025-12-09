namespace Commons.Service
{
    /// <summary>
    /// Til sozlamalarini boshqarish xizmati
    /// </summary>
    internal class LanguageService
    {
        private static readonly HashSet<string> SupportedLanguages = new HashSet<string> { "uz", "ru", "en" };
        private static string _language = "uz";
        private static readonly object _lock = new object();

        /// <summary>
        /// Joriy til
        /// </summary>
        public static string Language
        {
            get
            {
                lock (_lock)
                {
                    return _language;
                }
            }
            private set
            {
                lock (_lock)
                {
                    _language = value;
                }
            }
        }

        /// <summary>
        /// Tilni o'rnatish
        /// </summary>
        /// <param name="lang">Til kodi (uz, ru, en)</param>
        /// <returns>Til muvaffaqiyatli o'rnatilganmi</returns>
        public static bool SetLanguage(string lang)
        {
            if (string.IsNullOrWhiteSpace(lang))
            {
                Console.WriteLine("Xatolik: Til parametri bo'sh");
                return false;
            }

            lang = lang.ToLowerInvariant().Trim();

            if (!IsLanguageSupported(lang))
            {
                Console.WriteLine($"Xatolik: '{lang}' tili qo'llab-quvvatlanmaydi");
                return false;
            }

            Language = lang;
            
            // Tilni o'zgartirgandan keyin lug'atni yangilash
            LocalizationService.LoadLanguage(lang);
            
            return true;
        }

        /// <summary>
        /// Tilni o'rnatish (Async)
        /// </summary>
        public static async Task<bool> SetLanguageAsync(string lang)
        {
            if (string.IsNullOrWhiteSpace(lang))
            {
                Console.WriteLine("Xatolik: Til parametri bo'sh");
                return false;
            }

            lang = lang.ToLowerInvariant().Trim();

            if (!IsLanguageSupported(lang))
            {
                Console.WriteLine($"Xatolik: '{lang}' tili qo'llab-quvvatlanmaydi");
                return false;
            }

            Language = lang;
            
            // Tilni o'zgartirgandan keyin lug'atni yangilash
            await LocalizationService.LoadLanguageAsync(lang);
            
            return true;
        }

        /// <summary>
        /// Til qo'llab-quvvatlanadimi
        /// </summary>
        public static bool IsLanguageSupported(string lang)
        {
            if (string.IsNullOrWhiteSpace(lang))
                return false;

            return SupportedLanguages.Contains(lang.ToLowerInvariant().Trim());
        }

        /// <summary>
        /// Qo'llab-quvvatlanadigan tillar ro'yxati
        /// </summary>
        public static IReadOnlyCollection<string> GetSupportedLanguages()
        {
            return SupportedLanguages;
        }

        /// <summary>
        /// Standart tilni tiklash
        /// </summary>
        public static void ResetToDefault()
        {
            SetLanguage("uz");
        }
    }
}