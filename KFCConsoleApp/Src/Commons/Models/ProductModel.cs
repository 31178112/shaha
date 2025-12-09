using System.Text.Json;

namespace Commons.Models
{
    internal class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string ImageUrl { get; set; } = string.Empty; // Для будущего использования
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }

        public ProductModel() { }

        public override string ToString()
        {
            string status = IsAvailable ? "✅" : "❌";
            return $"{Id}. {Name} - {Price}₸ ({status})";
        }

        public string GetDetailedInfo()
        {
            return $"""
            {Name}
            📝 {Description}
            💰 Цена: {Price}sum
            📂 Категория ID: {CategoryId}
            {(IsAvailable ? "✅ В наличии" : "❌ Нет в наличии")}
            """;
        }

        public static List<ProductModel> FromJson(string path)
        {
            if (!File.Exists(path)) return new List<ProductModel>();
            
            try
            {
                string data = File.ReadAllText(path);
                return JsonSerializer.Deserialize<List<ProductModel>>(data) ?? new List<ProductModel>();
            }
            catch
            {
                return new List<ProductModel>();
            }
        }

        public static string ToJson(List<ProductModel> products)
        {
            return JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
