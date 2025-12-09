using Commons.Models;
using System.Text.Json;

namespace Commons.Repositories
{
    internal class CategoryRepository
    {
        private static List<CategoryModel> _categories = new();
        private static string _categoriesFilePath = "";
        private static int _nextId = 1;

        public static void Initialize(string filePath)
        {
            _categoriesFilePath = filePath;
            LoadCategories();
            _nextId = _categories.Count > 0 ? _categories.Max(c => c.Id) + 1 : 1;
        }

        private static void LoadCategories()
        {
            if (!File.Exists(_categoriesFilePath))
            {
                CreateDefaultCategories();
                SaveCategories();
            }
            else
            {
                try
                {
                    string json = File.ReadAllText(_categoriesFilePath);
                    _categories = JsonSerializer.Deserialize<List<CategoryModel>>(json) ?? new List<CategoryModel>();
                }
                catch
                {
                    CreateDefaultCategories();
                    SaveCategories();
                }
            }
        }

        private static void SaveCategories()
        {
            string json = JsonSerializer.Serialize(_categories, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_categoriesFilePath, json);
        }

        private static void CreateDefaultCategories()
        {
            _categories = new List<CategoryModel>
            {
                new CategoryModel { Id = 1, Name = "🍗 Курица", Description = "Куриные блюда KFC", CreatedBy = 2 },
                new CategoryModel { Id = 2, Name = "🍔 Бургеры", Description = "Классические бургеры", CreatedBy = 2 },
                new CategoryModel { Id = 3, Name = "🍟 Гарниры", Description = "Картофель и гарниры", CreatedBy = 2 },
                new CategoryModel { Id = 4, Name = "🥤 Напитки", Description = "Холодные и горячие напитки", CreatedBy = 2 },
                new CategoryModel { Id = 5, Name = "🍰 Десерты", Description = "Сладкие десерты", CreatedBy = 2 }
            };
            _nextId = 6;
        }

        // CRUD операции
        public static List<CategoryModel> GetAll()
        {
            return _categories;
        }

        public static CategoryModel? GetById(int id)
        {
            return _categories.FirstOrDefault(c => c.Id == id);
        }

        public static bool Add(CategoryModel category)
        {
            category.Id = _nextId++;
            category.CreatedAt = DateTime.Now;
            _categories.Add(category);
            SaveCategories();
            return true;
        }

        public static bool Update(CategoryModel category)
        {
            var existing = GetById(category.Id);
            if (existing == null) return false;

            existing.Name = category.Name;
            existing.Description = category.Description;
            SaveCategories();
            return true;
        }

        public static bool Delete(int id)
        {
            var category = GetById(id);
            if (category == null) return false;

            _categories.Remove(category);
            SaveCategories();
            return true;
        }
    }
}
