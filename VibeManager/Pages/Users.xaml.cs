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
using VibeManager.Data;
using VibeManager.Models.Controllers;

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
            ListUsers = new ObservableCollection<User>(UsersOrm.GetAllUsers());
            Roles = new ObservableCollection<Role>(UsersOrm.GetDistinctRolesFromUsers());

            OnPropertyChanged(nameof(ListUsers));
            OnPropertyChanged(nameof(Roles));

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
        private void SaveUser(object sender, RoutedEventArgs e)
        {
            if (SelectedUser == null || string.IsNullOrWhiteSpace(SelectedUser.Fullname) || string.IsNullOrWhiteSpace(SelectedUser.Email))
            {
                MessageBox.Show("Completa nombre y email.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool success = UsersOrm.SaveUser(SelectedUser);

            if (success)
            {
                MessageBox.Show("Usuario guardado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
                SelectedUser = new User();
                OnPropertyChanged(nameof(SelectedUser));
            }
            else
            {
                MessageBox.Show("Error al guardar el usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            if (SelectedUser == null || SelectedUser.Id == 0)
            {
                MessageBox.Show("Selecciona un usuario válido para eliminar.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("¿Estás seguro que deseas eliminar este usuario?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                bool success = UsersOrm.DeleteUser(SelectedUser.Id);

                if (success)
                {
                    MessageBox.Show("Usuario eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                    SelectedUser = new User();
                    OnPropertyChanged(nameof(SelectedUser));
                }
                else
                {
                    MessageBox.Show("Error al eliminar el usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ClearFields(object sender, RoutedEventArgs e)
        {
            SelectedUser = new User();
            OnPropertyChanged(nameof(SelectedUser));
        }
    }
}
