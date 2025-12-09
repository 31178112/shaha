using Commons.Models;
using Commons.Repositories;
using Commons.Services;

namespace KFCConsoleApp.Features.User.Screens
{
    internal class KfcMenuScreen
    {
        public static void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                
                var user = AuthService.CurrentUser;
                
                Console.WriteLine("============================================");
                Console.WriteLine("==           🍗 МЕНЮ KFC 🍗              ==");
                Console.WriteLine("============================================");
                
                var categories = CategoryRepository.GetAll();
                var products = ProductRepository.GetAll();
                var cart = user != null ? CartRepository.GetUserCart(user.Id) : null;
                int cartItemsCount = cart?.GetItemCount() ?? 0;
                
                Console.WriteLine($"📦 Товаров в корзине: {cartItemsCount}");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                
                if (categories.Count == 0)
                {
                    Console.WriteLine("\n📭 Категорий пока нет");
                }
                else
                {
                    Console.WriteLine("\n🎯 Выберите категорию:");
                    for (int i = 0; i < categories.Count; i++)
                    {
                        var category = categories[i];
                        var categoryProducts = products.Where(p => p.CategoryId == category.Id && p.IsAvailable).ToList();
                        Console.WriteLine($"  {i + 1}. {category.Name} ({categoryProducts.Count} товаров)");
                    }
                }
                
                Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("\nОпции:");
                Console.WriteLine("  [1-{0}] - Выбрать категорию", categories.Count);
                
                if (user != null)
                {
                    Console.WriteLine("  C - 🛒 Моя корзина");
                }
                else
                {
                    Console.WriteLine("  L - 🔐 Войти для заказа");
                }
                
                Console.WriteLine("  S - 🔍 Поиск товаров");
                Console.WriteLine("  0 - ↩️  Назад");
                
                Console.Write("\nВыберите действие: ");
                string choice = Console.ReadLine()?.ToUpper() ?? "";
                
                // Выбор категории
                if (int.TryParse(choice, out int categoryIndex) && categoryIndex >= 1 && categoryIndex <= categories.Count)
                {
                    ShowCategoryProducts(categories[categoryIndex - 1]);
                    continue;
                }
                
                switch (choice)
                {
                    case "C" when user != null:
                        CartScreen.Show();
                        break;
                    case "L" when user == null:
                        Auth.Screens.LoginScreen.Show();
                        if (AuthService.IsAuthenticated)
                        {
                            Show();
                        }
                        return;
                    case "S":
                        SearchProducts();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\n❌ Неверный выбор!");
                        Thread.Sleep(1000);
                        break;
                }
            }
        }
        
        private static void ShowCategoryProducts(CategoryModel category)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                
                Console.WriteLine("============================================");
                Console.WriteLine($"==        📂 {category.Name.ToUpper()}         ==");
                Console.WriteLine($"==        📝 {category.Description}        ==");
                Console.WriteLine("============================================");
                
                var products = ProductRepository.GetByCategory(category.Id)
                    .Where(p => p.IsAvailable)
                    .ToList();
                
                if (products.Count == 0)
                {
                    Console.WriteLine("\n📭 В этой категории пока нет товаров");
                }
                else
                {
                    Console.WriteLine("\n📦 Доступные товары:");
                    Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                    
                    for (int i = 0; i < products.Count; i++)
                    {
                        var product = products[i];
                        Console.WriteLine($"\n  {i + 1}. {product.Name}");
                        Console.WriteLine($"     💰 Цена: {product.Price}₸");
                        Console.WriteLine($"     📝 {product.Description}");
                        
                        if (AuthService.IsAuthenticated)
                        {
                            Console.WriteLine($"     [A{i + 1}] ➕ Добавить в корзину");
                        }
                    }
                }
                
                Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("\nОпции:");
                
                if (AuthService.IsAuthenticated)
                {
                    Console.WriteLine("  [A1-A{0}] - Добавить товар в корзину", products.Count);
                }
                else
                {
                    Console.WriteLine("  L - 🔐 Войти для добавления в корзину");
                }
                
                Console.WriteLine("  0 - ↩️  Назад к категориям");
                
                Console.Write("\nВыберите действие: ");
                string choice = Console.ReadLine()?.ToUpper() ?? "";
                
                // Добавление в корзину (A1, A2, ...)
                if (choice.StartsWith("A") && int.TryParse(choice.Substring(1), out int productIndex) && 
                    productIndex >= 1 && productIndex <= products.Count)
                {
                    if (!AuthService.IsAuthenticated)
                    {
                        Console.WriteLine("\n❌ Для добавления в корзину нужно войти в систему!");
                        Console.ReadKey();
                        continue;
                    }
                    
                    var selectedProduct = products[productIndex - 1];
                    Console.Write($"\nКоличество '{selectedProduct.Name}' (по умолчанию 1): ");
                    string qtyInput = Console.ReadLine()?.Trim() ?? "1";
                    
                    if (!int.TryParse(qtyInput, out int quantity) || quantity <= 0)
                    {
                        quantity = 1;
                    }
                    
                    var user = AuthService.CurrentUser;
                    if (user != null && CartRepository.AddToCart(user.Id, selectedProduct, quantity))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n✅ Добавлено {quantity} x '{selectedProduct.Name}' в корзину!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n❌ Ошибка при добавлении в корзину!");
                        Console.ResetColor();
                    }
                    
                    Console.WriteLine("\nНажмите любую клавишу...");
                    Console.ReadKey();
                    continue;
                }
                
                switch (choice)
                {
                    case "L" when !AuthService.IsAuthenticated:
                        Auth.Screens.LoginScreen.Show();
                        if (AuthService.IsAuthenticated)
                        {
                            ShowCategoryProducts(category);
                        }
                        return;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("\n❌ Неверный выбор!");
                        Thread.Sleep(1000);
                        break;
                }
            }
        }
        
        private static void SearchProducts()
        {
            Console.Clear();
            Console.WriteLine("🔍 Поиск товаров");
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            
            Console.Write("\nВведите название или описание: ");
            string keyword = Console.ReadLine()?.Trim() ?? "";
            
            var results = ProductRepository.Search(keyword)
                .Where(p => p.IsAvailable)
                .ToList();
            
            Console.WriteLine($"\n📊 Найдено товаров: {results.Count}");
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            
            if (results.Count == 0)
            {
                Console.WriteLine("   Ничего не найдено");
            }
            else
            {
                for (int i = 0; i < results.Count; i++)
                {
                    var product = results[i];
                    var category = CategoryRepository.GetById(product.CategoryId);
                    string categoryName = category?.Name ?? "Неизвестно";
                    
                    Console.WriteLine($"\n  {i + 1}. {product.Name}");
                    Console.WriteLine($"     📂 Категория: {categoryName}");
                    Console.WriteLine($"     💰 Цена: {product.Price}₸");
                    Console.WriteLine($"     📝 {product.Description}");
                    
                    if (AuthService.IsAuthenticated)
                    {
                        Console.WriteLine($"     [S{i + 1}] ➕ Добавить в корзину");
                    }
                }
            }
            
            Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            
            if (AuthService.IsAuthenticated && results.Count > 0)
            {
                Console.WriteLine("\nДля добавления в корзину введите S1, S2, ...");
                Console.Write("Или 0 для возврата: ");
                string choice = Console.ReadLine()?.ToUpper() ?? "";
                
                if (choice.StartsWith("S") && int.TryParse(choice.Substring(1), out int productIndex) && 
                    productIndex >= 1 && productIndex <= results.Count)
                {
                    var selectedProduct = results[productIndex - 1];
                    Console.Write($"\nКоличество '{selectedProduct.Name}' (по умолчанию 1): ");
                    string qtyInput = Console.ReadLine()?.Trim() ?? "1";
                    
                    if (!int.TryParse(qtyInput, out int quantity) || quantity <= 0)
                    {
                        quantity = 1;
                    }
                    
                    var user = AuthService.CurrentUser;
                    if (user != null && CartRepository.AddToCart(user.Id, selectedProduct, quantity))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n✅ Добавлено {quantity} x '{selectedProduct.Name}' в корзину!");
                        Console.ResetColor();
                    }
                }
            }
            else
            {
                Console.WriteLine("\nНажмите любую клавишу для возврата...");
            }
            
            Console.ReadKey();
        }
    }
}
