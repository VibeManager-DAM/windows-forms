using System.Windows;
using VibeManager.Models.Controllers;

namespace VibeManager.ViewModels
{
    /// <summary>
    /// ViewModel para la pantalla de inicio de sesión (Login).
    /// Gestiona la autenticación del usuario y redirige a la vista correspondiente según el rol.
    /// </summary>
    public class LoginVM
    {
        private MainViewModel _mainViewModel;

        /// <summary>
        /// Nombre de usuario ingresado.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Contraseña ingresada.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Comando para realizar el inicio de sesión.
        /// </summary>
        public RelayCommand LoginCommand { get; }

        /// <summary>
        /// Constructor del ViewModel de inicio de sesión.
        /// Inicializa los datos del comando y el ViewModel principal.
        /// </summary>
        /// <param name="mainViewModel">ViewModel principal de la aplicación.</param>
        public LoginVM(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            LoginCommand = new RelayCommand(Login);
        }

        /// <summary>
        /// Método que realiza el proceso de inicio de sesión.
        /// Valida las credenciales del usuario y redirige a la vista correspondiente según el rol.
        /// </summary>
        /// <param name="parameter">Parámetro opcional (no utilizado en este caso).</param>
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
