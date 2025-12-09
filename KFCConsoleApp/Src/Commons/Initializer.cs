using Commons.Services;
using System;

namespace Commons
{
    public static class Initializer
    {
        public static IAuthService AuthService { get; private set; }

        static Initializer()
        {
            AuthService = new AuthService();
        }

        public static void Initialize()
        {
            Console.WriteLine("Система инициализирована...");
        }
    }
}
