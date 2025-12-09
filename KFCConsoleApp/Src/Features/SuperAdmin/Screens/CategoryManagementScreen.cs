using Commons.Models;
using Commons.Repositories;
using Commons.Services;

namespace KFCConsoleApp.Features.SuperAdmin.Screens
{
    internal class CategoryManagementScreen
    {
        public static void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                
                Console.WriteLine("============================================");
                Console.WriteLine("==        🏷️  Управление категориями     ==");
                Console.WriteLine("============================================");
                
                var categories = CategoryRepository.GetAll();
                
                Console.WriteLine("\n📋 Список категорий:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                
                if (categories.Count == 0)
                {
                    Console.WriteLine("   Категорий нет");
                }
                else
                {
                    foreach (var category in categories)
                    {
                        Console.WriteLine($"   {category.Id}. {category.Name}");
                        Console.WriteLine($"      📝 {category.Description}");
                        Console.WriteLine();
                    }
                }
                
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("\nОпции:");
                Console.WriteLine("  1. ➕ Добавить категорию");
                Console.WriteLine("  2. ✏️  Редактировать категорию");
                Console.WriteLine("  3. ❌ Удалить категорию");
                Console.WriteLine("  0. ↩️  Назад");
                
                Console.Write("\nВыберите действие: ");
                string choice = Console.ReadLine() ?? "";
                
                switch (choice)
                {
                    case "1":
                        AddCategory();
                        break;
                    case "2":
                        EditCategory();
                        break;
                    case "3":
                        DeleteCategory();
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
        
        private static void AddCategory()
        {
            Console.Clear();
            Console.WriteLine("➕ Добавление новой категории");
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            
            Console.Write("\nНазвание категории: ");
            string name = Console.ReadLine()?.Trim() ?? "";
            
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("\n❌ Название не может быть пустым!");
                Console.ReadKey();
                return;
            }
            
            Console.Write("Описание: ");
            string description = Console.ReadLine()?.Trim() ?? "";
            
            var category = new CategoryModel
            {
                Name = name,
                Description = description,
                CreatedBy = AuthService.CurrentUser?.Id ?? 2
            };
            
            if (CategoryRepository.Add(category))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n✅ Категория '{name}' добавлена!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n❌ Ошибка при добавлении категории!");
                Console.ResetColor();
            }
            
            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }
        
        private static void EditCategory()
        {
            Console.Clear();
            Console.WriteLine("✏️  Редактирование категории");
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            
            var categories = CategoryRepository.GetAll();
            if (categories.Count == 0)
            {
                Console.WriteLine("\n📭 Категорий нет для редактирования");
                Console.ReadKey();
                return;
            }
            
            Console.WriteLine("\nВыберите категорию для редактирования:");
            foreach (var category in categories)
            {
                Console.WriteLine($"  {category.Id}. {category.Name}");
            }
            
            Console.Write("\nID категории: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\n❌ Неверный ID!");
                Console.ReadKey();
                return;
            }
            
            var categoryToEdit = CategoryRepository.GetById(id);
            if (categoryToEdit == null)
            {
                Console.WriteLine("\n❌ Категория не найдена!");
                Console.ReadKey();
                return;
            }
            
            Console.WriteLine($"\nТекущее название: {categoryToEdit.Name}");
            Console.Write("Новое название: ");
            string newName = Console.ReadLine()?.Trim() ?? "";
            
            Console.WriteLine($"\nТекущее описание: {categoryToEdit.Description}");
            Console.Write("Новое описание: ");
            string newDescription = Console.ReadLine()?.Trim() ?? "";
            
            if (!string.IsNullOrWhiteSpace(newName))
                categoryToEdit.Name = newName;
            
            if (!string.IsNullOrWhiteSpace(newDescription))
                categoryToEdit.Description = newDescription;
            
            if (CategoryRepository.Update(categoryToEdit))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n✅ Категория обновлена!");
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
        
        private static void DeleteCategory()
        {
            Console.Clear();
            Console.WriteLine("❌ Удаление категории");
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            
            var categories = CategoryRepository.GetAll();
            if (categories.Count == 0)
            {
                Console.WriteLine("\n📭 Категорий нет для удаления");
                Console.ReadKey();
                return;
            }
            
            Console.WriteLine("\nВыберите категорию для удаления:");
            foreach (var category in categories)
            {
                Console.WriteLine($"  {category.Id}. {category.Name}");
            }
            
            Console.Write("\nID категории: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\n❌ Неверный ID!");
                Console.ReadKey();
                return;
            }
            
            var categoryToDelete = CategoryRepository.GetById(id);
            if (categoryToDelete == null)
            {
                Console.WriteLine("\n❌ Категория не найдена!");
                Console.ReadKey();
                return;
            }
            
            Console.Write($"\n⚠️  Вы уверены что хотите удалить категорию '{categoryToDelete.Name}'? (да/нет): ");
            string confirmation = Console.ReadLine()?.ToLower() ?? "";
            
            if (confirmation == "да" || confirmation == "д" || confirmation == "y" || confirmation == "yes")
            {
                if (CategoryRepository.Delete(id))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n✅ Категория удалена!");
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
    }
}
