using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VibeManager.Pages
{
    /// <summary>
    /// Lógica de interacción para Users.xaml
    /// </summary>
    public partial class Users : UserControl, INotifyPropertyChanged
    {
        private const int PageSize = 10;
        private int _currentPage = 1;
        public ObservableCollection<User> ListUsers { get; set; }
        public ObservableCollection<User> PagedUsers { get; set; } = new ObservableCollection<User>();
        public ObservableCollection<Role> Roles { get; set; }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); FilterUsers(); }
        }

        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set { _selectedUser = value; OnPropertyChanged(); }
        }

        public Users()
        {
            InitializeComponent();
            DataContext = this;
            LoadData();
        }

        private void LoadData()
        {
            Roles = new ObservableCollection<Role>
            {
                new Role { Id = 1, Name = "Superadministrador" },
                new Role { Id = 2, Name = "Organizador de eventos" },
                new Role { Id = 3, Name = "Usuario normal" }
            };

            ListUsers = new ObservableCollection<User>
            {
                new User { Id = 1, Fullname = "Admin", Email = "admin@example.com", IdRol = 1 },
                new User { Id = 2, Fullname = "Event Manager", Email = "event@example.com", IdRol = 2 },
                new User { Id = 3, Fullname = "User", Email = "user@example.com", IdRol = 3 },
                new User { Id = 4, Fullname = "User2", Email = "user2@example.com", IdRol = 3 },
                new User { Id = 5, Fullname = "User3", Email = "user3@example.com", IdRol = 3 },
                new User { Id = 6, Fullname = "User4", Email = "user4@example.com", IdRol = 3 },
                new User { Id = 7, Fullname = "User5", Email = "user5@example.com", IdRol = 3 },
                new User { Id = 8, Fullname = "User6", Email = "user6@example.com", IdRol = 2 },
                new User { Id = 9, Fullname = "Admin", Email = "admin@example.com", IdRol = 1 },
                new User { Id = 10, Fullname = "Event Manager", Email = "event@example.com", IdRol = 2 },
                new User { Id = 11, Fullname = "User", Email = "user@example.com", IdRol = 3 },
                new User { Id = 12, Fullname = "User2", Email = "user2@example.com", IdRol = 3 },
                new User { Id = 13, Fullname = "User3", Email = "user3@example.com", IdRol = 3 },
                new User { Id = 14, Fullname = "User4", Email = "user4@example.com", IdRol = 3 },
                new User { Id = 15, Fullname = "User5", Email = "user5@example.com", IdRol = 3 },
                new User { Id = 16, Fullname = "User6", Email = "user6@example.com", IdRol = 2 },
                new User { Id = 17, Fullname = "Admin", Email = "admin@example.com", IdRol = 1 },
                new User { Id = 18, Fullname = "Event Manager", Email = "event@example.com", IdRol = 2 },
                new User { Id = 19, Fullname = "User", Email = "user@example.com", IdRol = 3 },
                new User { Id = 20, Fullname = "User2", Email = "user2@example.com", IdRol = 3 },
                new User { Id = 21, Fullname = "User3", Email = "user3@example.com", IdRol = 3 },
                new User { Id = 22, Fullname = "User4", Email = "user4@example.com", IdRol = 3 },
                new User { Id = 23, Fullname = "User5", Email = "user5@example.com", IdRol = 3 },
                new User { Id = 24, Fullname = "User6", Email = "user6@example.com", IdRol = 2 }
            };

            foreach (var user in ListUsers)
            {
                user.RolName = Roles.FirstOrDefault(r => r.Id == user.IdRol)?.Name ?? "Desconocido";
            }

            FilterUsers();

            FilterUsers();
        }

        private void FilterUsers()
        {
            var filteredUsers = string.IsNullOrWhiteSpace(SearchText) ? ListUsers :
                                new ObservableCollection<User>(ListUsers.Where(u => u.Fullname.ToLower().Contains(SearchText.ToLower()) || u.Email.ToLower().Contains(SearchText.ToLower())));

            PagedUsers.Clear();
            foreach (var user in filteredUsers.Skip((_currentPage - 1) * PageSize).Take(PageSize))
            {
                PagedUsers.Add(user);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PreviousPage(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                FilterUsers();
            }
        }

        private void NextPage(object sender, RoutedEventArgs e)
        {
            if ((_currentPage * PageSize) < ListUsers.Count)
            {
                _currentPage++;
                FilterUsers();
            }
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public int IdRol { get; set; }
        public string RolName { get; set; }
    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
