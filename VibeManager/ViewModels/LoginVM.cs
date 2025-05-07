using System.Windows;
using VibeManager.Models.Controllers;

namespace VibeManager.ViewModels
{
    public class LoginVM
    {
        private MainViewModel _mainViewModel;

        public string Username { get; set; }
        public string Password { get; set; }

        public RelayCommand LoginCommand { get; }

        public LoginVM(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            LoginCommand = new RelayCommand(Login);
        }

        private void Login(object parameter)
        {
            var user = UsersOrm.Login(Username, Password);

            if (user == null)
            {
                MessageBox.Show("Usuario o contraseña incorrectos");
                return;
            }

            // Guardar sesión
            App.CurrentUser = user;

            if (user.RoleId == 3) // admin
            {
                _mainViewModel.ShowDashboard();
            }
            else if (user.RoleId == 1) // organizer
            {
                _mainViewModel.ShowEvents();
            }
            else
            {
                MessageBox.Show("Rol no autorizado");
            }
        }

    }
}
