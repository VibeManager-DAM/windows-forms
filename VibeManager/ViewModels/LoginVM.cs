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
            int? idRol = UsersOrm.Login(Username, Password);
            if (idRol.Equals(3)) // admin
            {
                _mainViewModel.ShowDashboard();
            }
            else if (idRol.Equals(1)) // organizer
            {
                _mainViewModel.ShowEvents();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos");
            }
        }
    }
}
