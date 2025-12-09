using System.Text.Json;

namespace Commons.Models
{
    internal class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Price * Quantity;

        public CartItem() { }

        public override string ToString()
        {
            return $"{ProductName} x{Quantity} = {TotalPrice}₸";
        }
    }

    internal class CartModel
    {
        public int UserId { get; set; }
        public List<CartItem> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(item => item.TotalPrice);
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public CartModel() { }

        public void AddItem(ProductModel product, int quantity = 1)
        {
            var existingItem = Items.FirstOrDefault(item => item.ProductId == product.Id);
            
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity
                });
            }
            
            UpdatedAt = DateTime.Now;
        }

        public void RemoveItem(int productId, int quantity = 1)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                item.Quantity -= quantity;
                if (item.Quantity <= 0)
                {
                    Items.Remove(item);
                }
                UpdatedAt = DateTime.Now;
            }
        }

        public void Clear()
        {
            Items.Clear();
            UpdatedAt = DateTime.Now;
        }

        public int GetItemCount()
        {
            return Items.Sum(item => item.Quantity);
        }

        public static List<CartModel> FromJson(string path)
        {
            if (!File.Exists(path)) return new List<CartModel>();
            
            try
            {
                string data = File.ReadAllText(path);
                return JsonSerializer.Deserialize<List<CartModel>>(data) ?? new List<CartModel>();
            }
            catch
            {
                return new List<CartModel>();
            }
        }

        public static string ToJson(List<CartModel> carts)
        {
            return JsonSerializer.Serialize(carts, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
