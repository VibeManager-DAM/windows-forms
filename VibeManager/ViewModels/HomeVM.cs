using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace VibeManager.ViewModels
{
    public class HomeVM : INotifyPropertyChanged
    {
        private object _currentView;
        private double _sidebarWidth = 80; // Ancho inicial del menú colapsado
        private Visibility _menuTextVisibility = Visibility.Collapsed; // Ocultar texto al inicio

        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public double SidebarWidth
        {
            get => _sidebarWidth;
            set { _sidebarWidth = value; OnPropertyChanged(); }
        }

        public Visibility MenuTextVisibility
        {
            get => _menuTextVisibility;
            set { _menuTextVisibility = value; OnPropertyChanged(); }
        }

        public ICommand ToggleMenuCommand { get; }
        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowUsersCommand { get; }
        public ICommand ShowSpacesCommand { get; }
        public ICommand ShowSettingsCommand { get; }

        public HomeVM()
        {
            CurrentView = new DashboardVM();

            ToggleMenuCommand = new RelayCommand(o => ToggleMenu());
            ShowDashboardCommand = new RelayCommand(o => CurrentView = new DashboardVM());
            ShowUsersCommand = new RelayCommand(o => CurrentView = new UsersVM());
            ShowSpacesCommand = new RelayCommand(o => CurrentView = new SpacesVM());
            ShowSettingsCommand = new RelayCommand(o => CurrentView = new SettingsVM());
        }

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
