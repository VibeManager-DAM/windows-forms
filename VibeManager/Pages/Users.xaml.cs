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
    /// Lógica de interacción para la página de usuarios (Users.xaml).
    /// Permite ver, crear, modificar y eliminar usuarios, además de filtrarlos y paginarlos.
    /// </summary>
    public partial class Users : UserControl, INotifyPropertyChanged
    {
        private const int PageSize = 10; // Número de usuarios por página
        private int _currentPage = 1; // Página actual

        /// <summary>
        /// Lista de todos los usuarios.
        /// </summary>
        public ObservableCollection<User> ListUsers { get; set; }

        /// <summary>
        /// Lista de usuarios que se muestran en la página actual, después de aplicar filtros y paginación.
        /// </summary>
        public ObservableCollection<User> PagedUsers { get; set; } = new ObservableCollection<User>();

        /// <summary>
        /// Lista de roles disponibles para los usuarios.
        /// </summary>
        public ObservableCollection<Role> Roles { get; set; }

        private string _searchText;

        /// <summary>
        /// Texto de búsqueda para filtrar los usuarios por nombre o email.
        /// </summary>
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); FilterUsers(); }
        }

        private User _selectedUser;

        /// <summary>
        /// Usuario actualmente seleccionado para editar.
        /// </summary>
        public User SelectedUser
        {
            get => _selectedUser;
            set { _selectedUser = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Inicializa la página de usuarios y carga los datos necesarios.
        /// </summary>
        public Users()
        {
            InitializeComponent();
            DataContext = this;
            LoadData();
        }

        /// <summary>
        /// Carga todos los usuarios y roles desde la base de datos.
        /// </summary>
        private void LoadData()
        {
            ListUsers = new ObservableCollection<User>(UsersOrm.GetAllUsers());
            Roles = new ObservableCollection<Role>(UsersOrm.GetDistinctRolesFromUsers());

            OnPropertyChanged(nameof(ListUsers));
            OnPropertyChanged(nameof(Roles));

            FilterUsers();
        }

        /// <summary>
        /// Filtra y pagina los usuarios según el texto de búsqueda y la página actual.
        /// </summary>
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

        /// <summary>
        /// Evento que se dispara cuando una propiedad cambia, para notificar a la UI.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifica a la UI que una propiedad ha cambiado.
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Cambia a la página anterior, si es posible, y recarga los usuarios.
        /// </summary>
        private void PreviousPage(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                FilterUsers();
            }
        }

        /// <summary>
        /// Cambia a la siguiente página, si es posible, y recarga los usuarios.
        /// </summary>
        private void NextPage(object sender, RoutedEventArgs e)
        {
            if ((_currentPage * PageSize) < ListUsers.Count)
            {
                _currentPage++;
                FilterUsers();
            }
        }

        /// <summary>
        /// Guarda un nuevo usuario o actualiza uno existente.
        /// Verifica que el nombre y el email estén completos antes de guardar.
        /// </summary>
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
                SelectedUser = new User(); // Resetea el usuario seleccionado
                OnPropertyChanged(nameof(SelectedUser));
            }
            else
            {
                MessageBox.Show("Error al guardar el usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Elimina el usuario seleccionado.
        /// Pide confirmación antes de proceder con la eliminación.
        /// </summary>
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
                    SelectedUser = new User(); // Resetea el usuario seleccionado
                    OnPropertyChanged(nameof(SelectedUser));
                }
                else
                {
                    MessageBox.Show("Error al eliminar el usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Limpia los campos de edición de usuario.
        /// </summary>
        private void ClearFields(object sender, RoutedEventArgs e)
        {
            SelectedUser = new User();
            OnPropertyChanged(nameof(SelectedUser));
        }
    }
}
