using System.Text.Json;

namespace Commons.Models
{
    internal class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }

        public CategoryModel() { }

        public static List<CategoryModel> FromJson(string path)
        {
            if (!File.Exists(path)) return new List<CategoryModel>();
            
            string data = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<CategoryModel>>(data) ?? new List<CategoryModel>();
        }

        public static string ToJson(List<CategoryModel> categories)
        {
            return JsonSerializer.Serialize(categories, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
