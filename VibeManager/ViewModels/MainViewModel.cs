using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VibeManager.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            // Mostrar la vista de login al inicio
            CurrentView = new LoginVM(this);
        }

        public void ShowDashboard()
        {
            CurrentView = new HomeVM();
        }
        public void ShowEvents()
        {
            CurrentView = new MenuEventVM();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
