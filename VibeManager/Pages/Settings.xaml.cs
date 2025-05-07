using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace VibeManager.Pages
{
    /// <summary>
    /// Lógica de interacción para la página de configuración (Settings.xaml).
    /// Permite al usuario cambiar el idioma y realizar otras configuraciones.
    /// </summary>
    public partial class Settings : UserControl
    {
        private const double DefaultSize = 90; // Tamaño por defecto de las banderas
        private const double SelectedSize = 110; // Tamaño de la bandera seleccionada
        private Image _selectedImage = null; // Imagen de la bandera actualmente seleccionada

        /// <summary>
        /// Inicializa la página de configuración, reseteando el tamaño de las banderas.
        /// </summary>
        public Settings()
        {
            InitializeComponent();
            ResetFlagSizes();
        }

        /// <summary>
        /// Establece el idioma a catalán y anima el cambio de tamaño de la bandera correspondiente.
        /// </summary>
        /// <param name="sender">El origen del evento (generalmente la imagen de la bandera).</param>
        /// <param name="e">El argumento del evento.</param>
        private void SetCatalan(object sender, MouseButtonEventArgs e)
        {
            ((App)Application.Current).ChangeLanguage("ca");
            UpdateFlagSelection(ImgCatalan);
        }

        /// <summary>
        /// Establece el idioma a español y anima el cambio de tamaño de la bandera correspondiente.
        /// </summary>
        /// <param name="sender">El origen del evento (generalmente la imagen de la bandera).</param>
        /// <param name="e">El argumento del evento.</param>
        private void SetSpanish(object sender, MouseButtonEventArgs e)
        {
            ((App)Application.Current).ChangeLanguage("es");
            UpdateFlagSelection(ImgSpanish);
        }

        /// <summary>
        /// Establece el idioma a inglés y anima el cambio de tamaño de la bandera correspondiente.
        /// </summary>
        /// <param name="sender">El origen del evento (generalmente la imagen de la bandera).</param>
        /// <param name="e">El argumento del evento.</param>
        private void SetEnglish(object sender, MouseButtonEventArgs e)
        {
            ((App)Application.Current).ChangeLanguage("en");
            UpdateFlagSelection(ImgEnglish);
        }

        /// <summary>
        /// Actualiza la selección de la bandera y anima su cambio de tamaño.
        /// </summary>
        /// <param name="selectedImage">La imagen de la bandera seleccionada.</param>
        private void UpdateFlagSelection(Image selectedImage)
        {
            if (_selectedImage == selectedImage) return;

            _selectedImage = selectedImage;

            ResetFlagSizes();

            // Animar el tamaño de la bandera seleccionada
            AnimateFlagSize(selectedImage, SelectedSize);
        }

        /// <summary>
        /// Restablece el tamaño de todas las banderas a su tamaño por defecto.
        /// </summary>
        private void ResetFlagSizes()
        {
            AnimateFlagSize(ImgCatalan, DefaultSize);
            AnimateFlagSize(ImgSpanish, DefaultSize);
            AnimateFlagSize(ImgEnglish, DefaultSize);
        }

        /// <summary>
        /// Anima el cambio de tamaño de una bandera a un tamaño especificado.
        /// </summary>
        /// <param name="img">La imagen de la bandera a animar.</param>
        /// <param name="size">El tamaño al que debe cambiar la bandera.</param>
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

        /// <summary>
        /// Evento que se dispara al hacer clic en el botón de cierre de sesión.
        /// Cambia la vista actual a la de inicio de sesión.
        /// </summary>
        /// <param name="sender">El origen del evento (generalmente el botón de cerrar sesión).</param>
        /// <param name="e">El argumento del evento.</param>
        private void LogoutClicked(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.DataContext is VibeManager.ViewModels.MainViewModel mainViewModel)
            {
                mainViewModel.CurrentView = new VibeManager.ViewModels.LoginVM(mainViewModel);
            }
        }
    }
}
