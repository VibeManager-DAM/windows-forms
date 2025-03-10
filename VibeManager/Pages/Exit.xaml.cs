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
    /// Lógica de interacción para Exit.xaml
    /// </summary>
    public partial class Exit : Window
    {
        public bool UserConfirmedExit { get; private set; }

        public Exit()
        {
            InitializeComponent();
            UserConfirmedExit = false;
        }

        private void ConfirmExit(object sender, RoutedEventArgs e)
        {
            UserConfirmedExit = true;
            this.DialogResult = true;
            this.Close();
        }

        private void CancelExit(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
