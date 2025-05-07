using System;
using System.Collections.Generic;
using System.Linq;
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
using VibeManager.ViewModels;

namespace VibeManager.Pages
{
    /// <summary>
    /// Representa la vista de inicio de sesión de la aplicación.
    /// </summary>
    public partial class Login : UserControl
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Login"/>.
        /// </summary>
        public Login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Maneja el evento <c>PasswordChanged</c> del <see cref="PasswordBox"/>, 
        /// actualizando la propiedad <c>Password</c> del <see cref="LoginVM"/>.
        /// </summary>
        /// <param name="sender">El control <see cref="PasswordBox"/> que generó el evento.</param>
        /// <param name="e">Datos del evento <see cref="RoutedEventArgs"/>.</param>
        private void txtPass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginVM viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
