using System.Text.Json.Serialization;

namespace Commons.Models
{
    public class UserModel : IUserModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public int BranchId { get; set; }

        [JsonIgnore]
        public static UserModel AdminUser => new UserModel
        {
            Id = 1,
            Username = "admin",
            Password = "admin123",
            Email = "admin@kfc.com",
            Role = UserRole.Administrator,
            BranchId = 0
        };

        [JsonIgnore]
        public static UserModel SuperAdminUser => new UserModel
        {
            Id = 0,
            Username = "superadmin",
            Password = "superadmin123",
            Email = "superadmin@kfc.com",
            Role = UserRole.SuperAdministrator,
            BranchId = 0
        };

        public static List<UserModel> PredefinedUsers => new List<UserModel>
        {
            AdminUser,
            SuperAdminUser
        };

        public enum UserRole
        {
            Cashier,
            Administrator,
            SuperAdministrator
        }
    }

    public interface IUserModel
    {
        int Id { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string Email { get; set; }
        UserModel.UserRole Role { get; set; }
        int BranchId { get; set; }
    }
}
