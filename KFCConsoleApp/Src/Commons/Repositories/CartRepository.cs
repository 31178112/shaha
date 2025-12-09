using Commons.Models;
using System.Text.Json;

namespace Commons.Repositories
{
    internal class CartRepository
    {
        private static List<CartModel> _carts = new();
        private static string _cartsFilePath = "";

        public static void Initialize(string filePath)
        {
            _cartsFilePath = filePath;
            LoadCarts();
        }

        private static void LoadCarts()
        {
            if (!File.Exists(_cartsFilePath))
            {
                _carts = new List<CartModel>();
                SaveCarts();
            }
            else
            {
                try
                {
                    string json = File.ReadAllText(_cartsFilePath);
                    _carts = JsonSerializer.Deserialize<List<CartModel>>(json) ?? new List<CartModel>();
                }
                catch
                {
                    _carts = new List<CartModel>();
                    SaveCarts();
                }
            }
        }

        private static void SaveCarts()
        {
            string json = JsonSerializer.Serialize(_carts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_cartsFilePath, json);
        }

        public static CartModel GetUserCart(int userId)
        {
            var cart = _carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new CartModel { UserId = userId };
                _carts.Add(cart);
                SaveCarts();
            }
            return cart;
        }

        public static bool AddToCart(int userId, ProductModel product, int quantity = 1)
        {
            var cart = GetUserCart(userId);
            
            if (!product.IsAvailable)
            {
                Console.WriteLine($"❌ Товар '{product.Name}' временно недоступен!");
                return false;
            }

            cart.AddItem(product, quantity);
            SaveCarts();
            return true;
        }

        public static bool RemoveFromCart(int userId, int productId, int quantity = 1)
        {
            var cart = GetUserCart(userId);
            cart.RemoveItem(productId, quantity);
            SaveCarts();
            return true;
        }

        public static bool ClearCart(int userId)
        {
            var cart = GetUserCart(userId);
            cart.Clear();
            SaveCarts();
            return true;
        }

        public static bool UpdateCart(int userId, CartModel updatedCart)
        {
            var existingCart = _carts.FirstOrDefault(c => c.UserId == userId);
            if (existingCart != null)
            {
                _carts.Remove(existingCart);
            }
            
            _carts.Add(updatedCart);
            SaveCarts();
            return true;
        }
    }
}

