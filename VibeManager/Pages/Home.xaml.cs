using System.Windows;
using System.Windows.Controls;
using VibeManager.ViewModels;

namespace VibeManager.Pages
{
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
            DataContext = new HomeVM();
        }

        private void ToggleMenu(object sender, RoutedEventArgs e)
        {
            ((HomeVM)DataContext).ToggleMenuCommand.Execute(null);
        }
    }
}
