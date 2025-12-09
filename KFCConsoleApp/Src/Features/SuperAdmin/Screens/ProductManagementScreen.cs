using Commons.Models;
using Commons.Repositories;
using Commons.Services;

namespace KFCConsoleApp.Features.SuperAdmin.Screens
{
    internal class ProductManagementScreen
    {
        public static void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                
                Console.WriteLine("============================================");
                Console.WriteLine("==        📋 Управление продуктами       ==");
                Console.WriteLine("============================================");
                
                var products = ProductRepository.GetAll();
                var categories = CategoryRepository.GetAll();
                
                Console.WriteLine("\n📦 Список продуктов:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                
                if (products.Count == 0)
                {
                    Console.WriteLine("   Продуктов нет");
                }
                else
                {
                    foreach (var category in categories)
                    {
                        var categoryProducts = products.Where(p => p.CategoryId == category.Id).ToList();
                        if (categoryProducts.Count > 0)
                        {
                            Console.WriteLine($"\n   📂 {category.Name}:");
                            foreach (var product in categoryProducts)
                            {
                                string status = product.IsAvailable ? "✅" : "❌";
                                Console.WriteLine($"      {product.Id}. {product.Name} - {product.Price}₸ {status}");
                            }
                        }
                    }
                }
                
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("\nОпции:");
                Console.WriteLine("  1. ➕ Добавить продукт");
                Console.WriteLine("  2. ✏️  Редактировать продукт");
                Console.WriteLine("  3. ❌ Удалить продукт");
                Console.WriteLine("  4. 🔄 Изменить доступность");
                Console.WriteLine("  5. 🔍 Поиск продуктов");
                Console.WriteLine("  0. ↩️  Назад");
                
                Console.Write("\nВыберите действие: ");
                string choice = Console.ReadLine() ?? "";
                
                switch (choice)
                {
                    case "1":
                        AddProduct();
                        break;
                    case "2":
                        EditProduct();
                        break;
                    case "3":
                        DeleteProduct();
                        break;
                    case "4":
                        ToggleAvailability();
                        break;
                    case "5":
                        SearchProducts();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        Thread.Sleep(1000);
                        break;
                }
            }
        }
        
        private static void AddProduct()
        {
            Console.Clear();
            Console.WriteLine("➕ Добавление нового продукта");
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            
            // Выбор категории
            var categories = CategoryRepository.GetAll();
            if (categories.Count == 0)
            {
                Console.WriteLine("\n❌ Нет категорий! Сначала создайте категорию.");
                Console.ReadKey();
                return;
            }
            
            Console.WriteLine("\nВыберите категорию:");
            foreach (var category in categories)
            {
                Console.WriteLine($"  {category.Id}. {category.Name}");
            }
            
            Console.Write("\nID категории: ");
            if (!int.TryParse(Console.ReadLine(), out int categoryId) || categories.All(c => c.Id != categoryId))
            {
                Console.WriteLine("\n❌ Неверный ID категории!");
                Console.ReadKey();
                return;
            }
            
            Console.Write("\nНазвание продукта: ");
            string name = Console.ReadLine()?.Trim() ?? "";
            
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("\n❌ Название не может быть пустым!");
                Console.ReadKey();
                return;
            }
            
            Console.Write("Описание: ");
            string description = Console.ReadLine()?.Trim() ?? "";
            
            Console.Write("Цена: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
            {
                Console.WriteLine("\n❌ Неверная цена!");
                Console.ReadKey();
                return;
            }
            
            Console.Write("Доступен? (да/нет): ");
            string availableInput = Console.ReadLine()?.ToLower() ?? "";
            bool isAvailable = availableInput == "да" || availableInput == "д" || availableInput == "y" || availableInput == "yes";
            
            var product = new ProductModel
            {
                Name = name,
                Description = description,
                Price = price,
                CategoryId = categoryId,
                IsAvailable = isAvailable,
                CreatedBy = AuthService.CurrentUser?.Id ?? 2
            };
            
            if (ProductRepository.Add(product))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n✅ Продукт '{name}' добавлен!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n❌ Ошибка при добавлении продукта!");
                Console.ResetColor();
            }
            
            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }
        
        private static void EditProduct()
        {
            Console.Clear();
            Console.WriteLine("✏️  Редактирование продукта");
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            
            var products = ProductRepository.GetAll();
            if (products.Count == 0)
            {
                Console.WriteLine("\n📭 Продуктов нет для редактирования");
                Console.ReadKey();
                return;
            }
            
            Console.WriteLine("\nВыберите продукт для редактирования:");
            foreach (var prod in products)
            {
                string status = prod.IsAvailable ? "✅" : "❌";
                Console.WriteLine($"  {prod.Id}. {prod.Name} - {prod.Price}₸ {status}");
            }
            
            Console.Write("\nID продукта: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\n❌ Неверный ID!");
                Console.ReadKey();
                return;
            }
            
            var productToEdit = ProductRepository.GetById(id);
            if (productToEdit == null)
            {
                Console.WriteLine("\n❌ Продукт не найден!");
                Console.ReadKey();
                return;
            }
            
            // Выбор категории
            var categories = CategoryRepository.GetAll();
            Console.WriteLine($"\nТекущая категория ID: {productToEdit.CategoryId}");
            Console.WriteLine("Выберите новую категорию (0 - оставить текущую):");
            foreach (var category in categories)
            {
                Console.WriteLine($"  {category.Id}. {category.Name}");
            }
            
            Console.Write("\nID новой категории: ");
            if (int.TryParse(Console.ReadLine(), out int newCategoryId) && newCategoryId > 0)
            {
                if (categories.Any(c => c.Id == newCategoryId))
                {
                    productToEdit.CategoryId = newCategoryId;
                }
            }
            
            Console.WriteLine($"\nТекущее название: {productToEdit.Name}");
            Console.Write("Новое название (Enter - оставить): ");
            string newName = Console.ReadLine()?.Trim() ?? "";
            if (!string.IsNullOrWhiteSpace(newName))
                productToEdit.Name = newName;
            
            Console.WriteLine($"\nТекущее описание: {productToEdit.Description}");
            Console.Write("Новое описание (Enter - оставить): ");
            string newDescription = Console.ReadLine()?.Trim() ?? "";
            if (!string.IsNullOrWhiteSpace(newDescription))
                productToEdit.Description = newDescription;
            
            Console.WriteLine($"\nТекущая цена: {productToEdit.Price}₸");
            Console.Write("Новая цена (Enter - оставить): ");
            string priceInput = Console.ReadLine()?.Trim() ?? "";
            if (!string.IsNullOrWhiteSpace(priceInput) && decimal.TryParse(priceInput, out decimal newPrice) && newPrice > 0)
            {
                productToEdit.Price = newPrice;
            }
            
            Console.WriteLine($"\nТекущая доступность: {(productToEdit.IsAvailable ? "✅ В наличии" : "❌ Нет в наличии")}");
            Console.Write("Изменить доступность? (да/нет): ");
            string toggleInput = Console.ReadLine()?.ToLower() ?? "";
            if (toggleInput == "да" || toggleInput == "д")
            {
                productToEdit.IsAvailable = !productToEdit.IsAvailable;
            }
            
            if (ProductRepository.Update(productToEdit))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n✅ Продукт обновлен!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n❌ Ошибка при обновлении!");
                Console.ResetColor();
            }
            
            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }
        
        private static void DeleteProduct()
        {
            Console.Clear();
            Console.WriteLine("❌ Удаление продукта");
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            
            var products = ProductRepository.GetAll();
            if (products.Count == 0)
            {
                Console.WriteLine("\n📭 Продуктов нет для удаления");
                Console.ReadKey();
                return;
            }
            
            Console.WriteLine("\nВыберите продукт для удаления:");
            foreach (var prod in products)
            {
                Console.WriteLine($"  {prod.Id}. {prod.Name} - {prod.Price}₸");
            }
            
            Console.Write("\nID продукта: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\n❌ Неверный ID!");
                Console.ReadKey();
                return;
            }
            
            var productToDelete = ProductRepository.GetById(id);
            if (productToDelete == null)
            {
                Console.WriteLine("\n❌ Продукт не найден!");
                Console.ReadKey();
                return;
            }
            
            Console.Write($"\n⚠️  Вы уверены что хотите удалить продукт '{productToDelete.Name}'? (да/нет): ");
            string confirmation = Console.ReadLine()?.ToLower() ?? "";
            
            if (confirmation == "да" || confirmation == "д")
            {
                if (ProductRepository.Delete(id))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n✅ Продукт удален!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n❌ Ошибка при удалении!");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("\n❌ Удаление отменено");
            }
            
            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }
        
        private static void ToggleAvailability()
        {
            Console.Clear();
            Console.WriteLine("🔄 Изменение доступности продукта");
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            
            var allProducts = ProductRepository.GetAll();
            if (allProducts.Count == 0)
            {
                Console.WriteLine("\n📭 Продуктов нет");
                Console.ReadKey();
                return;
            }
            
            Console.WriteLine("\nВыберите продукт:");
            foreach (var prod in allProducts)
            {
                string status = prod.IsAvailable ? "✅" : "❌";
                Console.WriteLine($"  {prod.Id}. {prod.Name} - {status}");
            }
            
            Console.Write("\nID продукта: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\n❌ Неверный ID!");
                Console.ReadKey();
                return;
            }
            
            var productToUpdate = ProductRepository.GetById(id);
            if (productToUpdate == null)
            {
                Console.WriteLine("\n❌ Продукт не найден!");
                Console.ReadKey();
                return;
            }
            
            productToUpdate.IsAvailable = !productToUpdate.IsAvailable;
            string newStatus = productToUpdate.IsAvailable ? "✅ В наличии" : "❌ Нет в наличии";
            
            if (ProductRepository.Update(productToUpdate))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n✅ Продукт теперь {newStatus}!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n❌ Ошибка при обновлении!");
                Console.ResetColor();
            }
            
            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }
        
        private static void SearchProducts()
        {
            Console.Clear();
            Console.WriteLine("🔍 Поиск продуктов");
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            
            Console.Write("\nВведите поисковый запрос: ");
            string keyword = Console.ReadLine()?.Trim() ?? "";
            
            var results = ProductRepository.Search(keyword);
            
            Console.WriteLine($"\n📊 Найдено продуктов: {results.Count}");
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            
            if (results.Count == 0)
            {
                Console.WriteLine("   Ничего не найдено");
            }
            else
            {
                foreach (var prod in results)
                {
                    string status = prod.IsAvailable ? "✅" : "❌";
                    var category = CategoryRepository.GetById(prod.CategoryId);
                    string categoryName = category?.Name ?? "Неизвестно";
                    
                    Console.WriteLine($"\n  {prod.Id}. {prod.Name}");
                    Console.WriteLine($"     Категория: {categoryName}");
                    Console.WriteLine($"     Цена: {prod.Price}₸ {status}");
                    Console.WriteLine($"     Описание: {prod.Description}");
                }
            }
            
            Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}
