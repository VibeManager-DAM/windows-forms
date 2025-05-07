using System.Windows;
using System.Windows.Controls;
using VibeManager.ViewModels;

namespace VibeManager.Pages
{
    /// <summary>
    /// Representa la página principal del usuario dentro de la aplicación.
    /// </summary>
    public partial class Home : UserControl
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Home"/> y establece su contexto de datos.
        /// </summary>
        public Home()
        {
            InitializeComponent();
            DataContext = new HomeVM();
        }

        /// <summary>
        /// Ejecuta el comando de alternancia del menú lateral definido en el ViewModel.
        /// </summary>
        /// <param name="sender">El objeto que generó el evento.</param>
        /// <param name="e">Los datos del evento de clic.</param>
        private void ToggleMenu(object sender, RoutedEventArgs e)
        {
            ((HomeVM)DataContext).ToggleMenuCommand.Execute(null);
        }
    }
}
