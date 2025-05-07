using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VibeManager.ViewModels
{
    /// <summary>
    /// ViewModel principal de la aplicación.
    /// Gestiona la vista actual mostrada y facilita la navegación entre las diferentes vistas.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private object _currentView;

        /// <summary>
        /// Propiedad que representa la vista actual que se está mostrando en la interfaz de usuario.
        /// </summary>
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Constructor del ViewModel principal.
        /// Inicializa la vista actual con la vista de inicio de sesión.
        /// </summary>
        public MainViewModel()
        {
            // Mostrar la vista de login al inicio
            CurrentView = new LoginVM(this);
        }

        /// <summary>
        /// Cambia la vista actual a la vista de Dashboard (pantalla principal).
        /// </summary>
        public void ShowDashboard()
        {
            CurrentView = new HomeVM();
        }

        /// <summary>
        /// Cambia la vista actual a la vista de Eventos.
        /// </summary>
        public void ShowEvents()
        {
            CurrentView = new MenuEventVM();
        }

        /// <summary>
        /// Evento que notifica cuando una propiedad ha cambiado.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Método que dispara el evento PropertyChanged para notificar los cambios en las propiedades.
        /// </summary>
        /// <param name="name">Nombre de la propiedad que ha cambiado.</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
