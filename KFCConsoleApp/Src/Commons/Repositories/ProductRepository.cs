using Commons.Models;
using System.Text.Json;

namespace Commons.Repositories
{
    internal class ProductRepository
    {
        private static List<ProductModel> _products = new();
        private static string _productsFilePath = "";
        private static int _nextId = 1;

        public static void Initialize(string filePath)
        {
            _productsFilePath = filePath;
            LoadProducts();
            _nextId = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
        }

        private static void LoadProducts()
        {
            if (!File.Exists(_productsFilePath))
            {
                CreateSampleProducts();
                SaveProducts();
            }
            else
            {
                try
                {
                    string json = File.ReadAllText(_productsFilePath);
                    _products = JsonSerializer.Deserialize<List<ProductModel>>(json) ?? new List<ProductModel>();
                }
                catch
                {
                    CreateSampleProducts();
                    SaveProducts();
                }
            }
        }

        private static void SaveProducts()
        {
            string json = JsonSerializer.Serialize(_products, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_productsFilePath, json);
        }

        private static void CreateSampleProducts()
        {
            _products = new List<ProductModel>
            {
                // Курица (CategoryId: 1)
                new ProductModel { Id = 1, Name = "🍗 Ориджинал (3 шт)", Description = "Классическая курочка KFC", Price = 1890, CategoryId = 1, CreatedBy = 2 },
                new ProductModel { Id = 2, Name = "🍗 Стрипсы (6 шт)", Description = "Куриные полоски", Price = 2190, CategoryId = 1, CreatedBy = 2 },
                new ProductModel { Id = 3, Name = "🍗 Крылышки (8 шт)", Description = "Острые куриные крылышки", Price = 1790, CategoryId = 1, CreatedBy = 2 },
                
                // Бургеры (CategoryId: 2)
                new ProductModel { Id = 4, Name = "🍔 Чизбургер", Description = "Бургер с сыром", Price = 990, CategoryId = 2, CreatedBy = 2 },
                new ProductModel { Id = 5, Name = "🍔 Чикенбургер", Description = "Бургер с курицей", Price = 1190, CategoryId = 2, CreatedBy = 2 },
                new ProductModel { Id = 6, Name = "🍔 Дабл Чизбургер", Description = "Двойной бургер с сыром", Price = 1590, CategoryId = 2, CreatedBy = 2 },
                
                // Гарниры (CategoryId: 3)
                new ProductModel { Id = 7, Name = "🍟 Картофель фри", Description = "Классическая порция", Price = 690, CategoryId = 3, CreatedBy = 2 },
                new ProductModel { Id = 8, Name = "🥔 Картофель по-деревенски", Description = "Специи и зелень", Price = 790, CategoryId = 3, CreatedBy = 2 },
                
                // Напитки (CategoryId: 4)
                new ProductModel { Id = 9, Name = "🥤 Кола (0.5л)", Description = "Газированный напиток", Price = 49000, CategoryId = 4, CreatedBy = 2 },
                new ProductModel { Id = 10, Name = "🥤 Фанта (0.5л)", Description = "Апельсиновый напиток", Price = 23490, CategoryId = 4, CreatedBy = 2 },
                new ProductModel { Id = 11, Name = "☕ Кофе", Description = "Натуральный кофе", Price = 59220, CategoryId = 4, CreatedBy = 2 },
                
                // Десерты (CategoryId: 5)
                new ProductModel { Id = 12, Name = "🍰 Чизкейк", Description = "Классический чизкейк", Price = 82290, CategoryId = 5, CreatedBy = 2 },
                new ProductModel { Id = 13, Name = "🍫 Шоколадный маффин", Description = "Шоколадный кекс", Price = 54490, CategoryId = 5, CreatedBy = 2 }
            };
            _nextId = 14;
        }

        // CRUD операции
        public static List<ProductModel> GetAll()
        {
            return _products;
        }

        public static List<ProductModel> GetByCategory(int categoryId)
        {
            return _products.Where(p => p.CategoryId == categoryId).ToList();
        }

        public static ProductModel? GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public static bool Add(ProductModel product)
        {
            product.Id = _nextId++;
            product.CreatedAt = DateTime.Now;
            _products.Add(product);
            SaveProducts();
            return true;
        }

        public static bool Update(ProductModel product)
        {
            var existing = GetById(product.Id);
            if (existing == null) return false;

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.CategoryId = product.CategoryId;
            existing.IsAvailable = product.IsAvailable;
            SaveProducts();
            return true;
        }

        public static bool Delete(int id)
        {
            var product = GetById(id);
            if (product == null) return false;

            _products.Remove(product);
            SaveProducts();
            return true;
        }

        public static List<ProductModel> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return _products;

            return _products.Where(p => 
                p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }
    }
}
