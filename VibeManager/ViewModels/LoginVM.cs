using System.Windows;

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
            if (Username == "admin" && Password == "1234")
            {
                _mainViewModel.ShowDashboard();
            }
            else if (Username == "user" && Password == "1234")
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
