namespace Commons.Config
{
    internal static class AdminConfig
    {
        // Пароли по умолчанию (в реальном приложении хранить в зашифрованном виде!)
        public const string AdminPassword = "admin123";
        public const string SuperAdminPassword = "superadmin123";
        
        // Можно добавить больше настроек
        public const int MaxLoginAttempts = 3;
        public const bool RequireStrongPassword = true;
    }
}
