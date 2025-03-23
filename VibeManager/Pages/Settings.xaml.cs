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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VibeManager.Pages
{
    /// <summary>
    /// Lógica de interacción para Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        private const double DefaultSize = 90;
        private const double SelectedSize = 110;
        private Image _selectedImage = null;

        public Settings()
        {
            InitializeComponent();
            ResetFlagSizes();
        }

        private void SetCatalan(object sender, MouseButtonEventArgs e)
        {
            ((App)Application.Current).ChangeLanguage("ca");
            UpdateFlagSelection(ImgCatalan);
        }

        private void SetSpanish(object sender, MouseButtonEventArgs e)
        {
            ((App)Application.Current).ChangeLanguage("es");
            UpdateFlagSelection(ImgSpanish);
        }

        private void SetEnglish(object sender, MouseButtonEventArgs e)
        {
            ((App)Application.Current).ChangeLanguage("en");
            UpdateFlagSelection(ImgEnglish);
        }

        private void UpdateFlagSelection(Image selectedImage)
        {
            if (_selectedImage == selectedImage) return;

            _selectedImage = selectedImage;

            ResetFlagSizes();


            AnimateFlagSize(selectedImage, SelectedSize);
        }

        private void ResetFlagSizes()
        {
            AnimateFlagSize(ImgCatalan, DefaultSize);
            AnimateFlagSize(ImgSpanish, DefaultSize);
            AnimateFlagSize(ImgEnglish, DefaultSize);
        }

        private void AnimateFlagSize(Image img, double size)
        {
            var animation = new DoubleAnimation
            {
                To = size,
                Duration = TimeSpan.FromMilliseconds(200),
                EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseOut }
            };

            img.BeginAnimation(WidthProperty, animation);
            img.BeginAnimation(HeightProperty, animation);
        }

        private void LogoutClicked(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.DataContext is VibeManager.ViewModels.MainViewModel mainViewModel)
            {
                mainViewModel.CurrentView = new VibeManager.ViewModels.LoginVM(mainViewModel);
            }
        }
    }
}
