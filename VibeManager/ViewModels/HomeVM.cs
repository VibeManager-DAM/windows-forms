using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace VibeManager.ViewModels
{
    /// <summary>
    /// ViewModel para la pantalla de inicio (Home).
    /// Controla la visibilidad y el comportamiento del menú lateral, así como las vistas principales.
    /// </summary>
    public class HomeVM : INotifyPropertyChanged
    {
        private object _currentView; // Vista actual que se muestra en la UI
        private double _sidebarWidth = 80; // Ancho inicial del menú lateral cuando está colapsado
        private Visibility _menuTextVisibility = Visibility.Collapsed; // Visibilidad del texto en el menú lateral

        /// <summary>
        /// Vista actual que se está mostrando en la pantalla de inicio.
        /// </summary>
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Ancho del menú lateral.
        /// </summary>
        public double SidebarWidth
        {
            get => _sidebarWidth;
            set { _sidebarWidth = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Visibilidad del texto en el menú lateral.
        /// </summary>
        public Visibility MenuTextVisibility
        {
            get => _menuTextVisibility;
            set { _menuTextVisibility = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Comando para alternar la visibilidad del menú lateral.
        /// </summary>
        public ICommand ToggleMenuCommand { get; }

        /// <summary>
        /// Comando para mostrar la vista del dashboard.
        /// </summary>
        public ICommand ShowDashboardCommand { get; }

        /// <summary>
        /// Comando para mostrar la vista de usuarios.
        /// </summary>
        public ICommand ShowUsersCommand { get; }

        /// <summary>
        /// Comando para mostrar la vista de espacios.
        /// </summary>
        public ICommand ShowSpacesCommand { get; }

        /// <summary>
        /// Comando para mostrar la vista de configuración.
        /// </summary>
        public ICommand ShowSettingsCommand { get; }

        /// <summary>
        /// Constructor de la clase HomeVM.
        /// Inicializa la vista principal y los comandos de navegación.
        /// </summary>
        public HomeVM()
        {
            CurrentView = new DashboardVM();

            ToggleMenuCommand = new RelayCommand(o => ToggleMenu());
            ShowDashboardCommand = new RelayCommand(o => CurrentView = new DashboardVM());
            ShowUsersCommand = new RelayCommand(o => CurrentView = new UsersVM());
            ShowSpacesCommand = new RelayCommand(o => CurrentView = new SpacesVM());
            ShowSettingsCommand = new RelayCommand(o => CurrentView = new SettingsVM());
        }

        /// <summary>
        /// Alterna el estado del menú lateral (colapsado/expandido) y la visibilidad del texto.
        /// </summary>
        private void ToggleMenu()
        {
            if (SidebarWidth == 80)
            {
                SidebarWidth = 250;
                MenuTextVisibility = Visibility.Visible;
            }
            else
            {
                SidebarWidth = 80;
                MenuTextVisibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Evento que se dispara cuando una propiedad cambia, para notificar a la UI.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifica que una propiedad ha cambiado para que la interfaz de usuario se actualice.
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
