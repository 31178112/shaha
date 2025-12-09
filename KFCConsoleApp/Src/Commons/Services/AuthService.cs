using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Commons.Models;

namespace Commons.Services
{
    public class AuthService : IAuthService
    {
        private const string UsersFilePath = "Data/users.json";
        private List<UserModel> _users;

        public AuthService()
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            if (File.Exists(UsersFilePath))
            {
                var json = File.ReadAllText(UsersFilePath);
                _users = JsonSerializer.Deserialize<List<UserModel>>(json) ?? new List<UserModel>();
            }
            else
            {
                _users = new List<UserModel>();
                SaveUsers();
            }

            // Добавляем предопределенных пользователей, если их нет
            AddPredefinedUsers();
        }

        private void SaveUsers()
        {
            var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(UsersFilePath, json);
        }

        private void AddPredefinedUsers()
        {
            foreach (var predefinedUser in UserModel.PredefinedUsers)
            {
                if (!_users.Any(u => u.Username == predefinedUser.Username))
                {
                    _users.Add(predefinedUser);
                }
            }
            SaveUsers();
        }

        public bool Login(string username, string password)
        {
            var user = _users.FirstOrDefault(u => 
                u.Username == username && u.Password == password);
            
            return user != null;
        }

        public UserModel GetUser(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public void Register(UserModel user)
        {
            if (_users.Any(u => u.Username == user.Username))
            {
                throw new InvalidOperationException("Пользователь с таким именем уже существует");
            }

            _users.Add(user);
            SaveUsers();
        }

        public void UpdateUser(UserModel updatedUser)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (existingUser != null)
            {
                _users.Remove(existingUser);
                _users.Add(updatedUser);
                SaveUsers();
            }
        }

        public void DeleteUser(int userId)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                _users.Remove(user);
                SaveUsers();
            }
        }

        public List<UserModel> GetAllUsers()
        {
            return _users.ToList();
        }

        public List<UserModel> GetUsersByBranch(int branchId)
        {
            return _users.Where(u => u.BranchId == branchId).ToList();
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var user = _users.FirstOrDefault(u => 
                u.Username == username && u.Password == oldPassword);
            
            if (user != null)
            {
                user.Password = newPassword;
                SaveUsers();
                return true;
            }

            return false;
        }

        public bool IsUsernameAvailable(string username)
        {
            return !_users.Any(u => u.Username == username);
        }

        public bool IsAdmin(string username)
        {
            var user = GetUser(username);
            return user != null && (user.Role == UserModel.UserRole.Administrator || 
                   user.Role == UserModel.UserRole.SuperAdministrator);
        }

        public bool IsSuperAdmin(string username)
        {
            var user = GetUser(username);
            return user != null && user.Role == UserModel.UserRole.SuperAdministrator;
        }
    }

    public interface IAuthService
    {
        bool Login(string username, string password);
        UserModel GetUser(string username);
        void Register(UserModel user);
        void UpdateUser(UserModel updatedUser);
        void DeleteUser(int userId);
        List<UserModel> GetAllUsers();
        List<UserModel> GetUsersByBranch(int branchId);
        bool ChangePassword(string username, string oldPassword, string newPassword);
        bool IsUsernameAvailable(string username);
        bool IsAdmin(string username);
        bool IsSuperAdmin(string username);
    }
}
