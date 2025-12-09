using Commons.Models;
using Commons.Repositories;
using Commons.Services;

namespace KFCConsoleApp.Features.User.Screens
{
    internal class CartScreen
    {
        public static void Show()
        {
            var user = AuthService.CurrentUser;
            if (user == null)
            {
                Console.WriteLine("❌ Для просмотра корзины нужно войти в систему!");
                Console.ReadKey();
                return;
            }
            
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                
                var cart = CartRepository.GetUserCart(user.Id);
                
                Console.WriteLine("============================================");
                Console.WriteLine("==             🛒 МОЯ КОРЗИНА           ==");
                Console.WriteLine("============================================");
                
                if (cart.Items.Count == 0)
                {
                    Console.WriteLine("\n📭 Корзина пуста");
                    Console.WriteLine("\n🥺 Добавьте товары из меню KFC!");
                }
                else
                {
                    Console.WriteLine($"\n📦 Товаров в корзине: {cart.GetItemCount()}");
                    Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                    
                    for (int i = 0; i < cart.Items.Count; i++)
                    {
                        var item = cart.Items[i];
                        Console.WriteLine($"\n  {i + 1}. {item.ProductName}");
                        Console.WriteLine($"     💰 Цена за шт: {item.Price}₸");
                        Console.WriteLine($"     📦 Количество: x{item.Quantity}");
                        Console.WriteLine($"     🧮 Итого: {item.TotalPrice}₸");
                        Console.WriteLine($"     [R{i + 1}] ❌ Удалить");
                        Console.WriteLine($"     [E{i + 1}] ✏️  Изменить количество");
                    }
                    
                    Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"💰 ОБЩАЯ СУММА: {cart.TotalAmount}₸");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                
                Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("\nОпции:");
                
                if (cart.Items.Count > 0)
                {
                    Console.WriteLine("  [R1-R{0}] - Удалить товар", cart.Items.Count);
                    Console.WriteLine("  [E1-E{0}] - Изменить количество", cart.Items.Count);
                    Console.WriteLine("  C - 🗑️  Очистить корзину");
                    Console.WriteLine("  O - ✅ Оформить заказ");
                }
                
                Console.WriteLine("  M - 🍗 Вернуться в меню");
                Console.WriteLine("  0 - ↩️  Назад");
                
                Console.Write("\nВыберите действие: ");
                string choice = Console.ReadLine()?.ToUpper() ?? "";
                
                // Удаление товара (R1, R2, ...)
                if (choice.StartsWith("R") && int.TryParse(choice.Substring(1), out int removeIndex) && 
                    removeIndex >= 1 && removeIndex <= cart.Items.Count)
                {
                    var itemToRemove = cart.Items[removeIndex - 1];
                    Console.Write($"\nУдалить '{itemToRemove.ProductName}'? (да/нет): ");
                    string confirm = Console.ReadLine()?.ToLower() ?? "";
                    
                    if (confirm == "да" || confirm == "д")
                    {
                        CartRepository.RemoveFromCart(user.Id, itemToRemove.ProductId, itemToRemove.Quantity);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n✅ Товар удален из корзины!");
                        Console.ResetColor();
                        Console.ReadKey();
                    }
                    continue;
                }
                
                // Изменение количества (E1, E2, ...)
                if (choice.StartsWith("E") && int.TryParse(choice.Substring(1), out int editIndex) && 
                    editIndex >= 1 && editIndex <= cart.Items.Count)
                {
                    var itemToEdit = cart.Items[editIndex - 1];
                    Console.Write($"\nНовое количество для '{itemToEdit.ProductName}' (текущее: {itemToEdit.Quantity}): ");
                    string qtyInput = Console.ReadLine()?.Trim() ?? "";
                    
                    if (int.TryParse(qtyInput, out int newQuantity) && newQuantity > 0)
                    {
                        // Удаляем старую запись и добавляем новую
                        CartRepository.RemoveFromCart(user.Id, itemToEdit.ProductId, itemToEdit.Quantity);
                        
                        var product = ProductRepository.GetById(itemToEdit.ProductId);
                        if (product != null && product.IsAvailable)
                        {
                            CartRepository.AddToCart(user.Id, product, newQuantity);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"\n✅ Количество обновлено!");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n❌ Неверное количество!");
                        Console.ResetColor();
                    }
                    Console.ReadKey();
                    continue;
                }
                
                switch (choice)
                {
                    case "C" when cart.Items.Count > 0:
                        Console.Write("\n🗑️  Очистить всю корзину? (да/нет): ");
                        string clearConfirm = Console.ReadLine()?.ToLower() ?? "";
                        if (clearConfirm == "да" || clearConfirm == "д")
                        {
                            CartRepository.ClearCart(user.Id);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n✅ Корзина очищена!");
                            Console.ResetColor();
                            Console.ReadKey();
                        }
                        break;
                    case "O" when cart.Items.Count > 0:
                        CheckoutOrder();
                        return;
                    case "M":
                        KfcMenuScreen.Show();
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
        
        private static void CheckoutOrder()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            
            var user = AuthService.CurrentUser;
            if (user == null) return;
            
            var cart = CartRepository.GetUserCart(user.Id);
            
            Console.WriteLine("============================================");
            Console.WriteLine("==           ✅ ОФОРМЛЕНИЕ ЗАКАЗА        ==");
            Console.WriteLine("============================================");
            
            Console.WriteLine($"\n👤 Заказчик: {user.Name}");
            Console.WriteLine($"📧 Email: {user.Email}");
            Console.WriteLine($"📅 Дата: {DateTime.Now:dd.MM.yyyy HH:mm}");
            
            Console.WriteLine("\n📦 Состав заказа:");
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            
            foreach (var item in cart.Items)
            {
                Console.WriteLine($"  {item.ProductName} x{item.Quantity} = {item.TotalPrice}₸");
            }
            
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"💰 ИТОГО К ОПЛАТЕ: {cart.TotalAmount}₸");
            Console.ForegroundColor = ConsoleColor.Yellow;
            
            Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine("\nСпособы оплаты:");
            Console.WriteLine("  1. 💳 Онлайн оплата (картой)");
            Console.WriteLine("  2. 💰 Наличными при получении");
            Console.WriteLine("  3. 📱 По QR-коду");
            
            Console.Write("\nВыберите способ оплаты: ");
            string paymentMethod = Console.ReadLine() ?? "";
            
            Console.Write("\nАдрес доставки (или 'самовывоз'): ");
            string address = Console.ReadLine()?.Trim() ?? "самовывоз";
            
            Console.Write("\nКомментарий к заказу: ");
            string comment = Console.ReadLine()?.Trim() ?? "";
            
            Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.Write("\n✅ Подтвердить заказ? (да/нет): ");
            string confirmation = Console.ReadLine()?.ToLower() ?? "";
            
            if (confirmation == "да" || confirmation == "д")
            {
                // TODO: Сохранение заказа в базу данных
                
                // Очищаем корзину после успешного заказа
                CartRepository.ClearCart(user.Id);
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n🎉 ЗАКАЗ УСПЕШНО ОФОРМЛЕН!");
                Console.WriteLine("\n📞 С вами свяжется оператор для подтверждения.");
                Console.WriteLine("⏱️  Примерное время доставки: 30-45 минут");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n⏸️  Заказ отменен");
                Console.ResetColor();
            }
            
            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}
