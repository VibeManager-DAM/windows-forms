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
using System.Windows.Shapes;

namespace VibeManager.Pages
{
    /// <summary>
    /// Ventana de confirmación para salir de la aplicación.
    /// </summary>
    public partial class Exit : Window
    {
        /// <summary>
        /// Obtiene un valor que indica si el usuario confirmó la salida.
        /// </summary>
        public bool UserConfirmedExit { get; private set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Exit"/> y configura el estado inicial.
        /// </summary>
        public Exit()
        {
            InitializeComponent();
            UserConfirmedExit = false;
        }

        /// <summary>
        /// Establece la confirmación de salida como verdadera y cierra la ventana con un resultado afirmativo.
        /// </summary>
        /// <param name="sender">El objeto que generó el evento.</param>
        /// <param name="e">Datos del evento de clic.</param>
        private void ConfirmExit(object sender, RoutedEventArgs e)
        {
            UserConfirmedExit = true;
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Cancela la acción de salida y cierra la ventana con un resultado negativo.
        /// </summary>
        /// <param name="sender">El objeto que generó el evento.</param>
        /// <param name="e">Datos del evento de clic.</param>
        private void CancelExit(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
