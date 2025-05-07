using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using VibeManager.Data;
using VibeManager.Models.Controllers;

namespace VibeManager.Pages
{
    /// <summary>
    /// Lógica de interacción para la página de espacios (Spaces.xaml).
    /// Muestra una lista de espacios y permite paginarlos, filtrarlos y visualizarlos en un mapa.
    /// </summary>
    public partial class Spaces : UserControl, INotifyPropertyChanged
    {
        private const int PageSize = 5; // Número de espacios por página
        private int _currentPage = 1; // Página actual

        /// <summary>
        /// Colección de todos los espacios disponibles.
        /// </summary>
        public ObservableCollection<Space> AllSpaces { get; set; }

        /// <summary>
        /// Colección de espacios a mostrar en la página actual, después de aplicar filtros y paginación.
        /// </summary>
        public ObservableCollection<Space> PagedSpaces { get; set; }

        private ObservableCollection<Space> FilteredSpaces { get; set; } = new ObservableCollection<Space>();

        private Space _selectedSpace;

        /// <summary>
        /// Espacio actualmente seleccionado.
        /// </summary>
        public Space SelectedSpace
        {
            get { return _selectedSpace; }
            set
            {
                _selectedSpace = value;
                OnPropertyChanged();
                FocusOnSelectedMarker();
            }
        }

        private string _searchText;

        /// <summary>
        /// Texto de búsqueda utilizado para filtrar los espacios por nombre.
        /// </summary>
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                _currentPage = 1;
                FilterAndPaginateSpaces();
            }
        }

        private RectangleGeometry clipGeometry;

        /// <summary>
        /// Inicializa la página de espacios y configura los elementos visuales y datos iniciales.
        /// </summary>
        public Spaces()
        {
            InitializeComponent();

            AllSpaces = new ObservableCollection<Space>();
            PagedSpaces = new ObservableCollection<Space>();

            clipGeometry = new RectangleGeometry();
            MapControl.Clip = clipGeometry;

            this.DataContext = this;

            LoadSpaces();
            FilterAndPaginateSpaces();
        }

        /// <summary>
        /// Carga los espacios desde la base de datos y los agrega a la colección AllSpaces.
        /// </summary>
        private void LoadSpaces()
        {
            var spacesFromDb = SpacesOrm.SelectAllSpaces();

            AllSpaces.Clear();
            foreach (var space in spacesFromDb)
            {
                AllSpaces.Add(space);
            }
        }

        /// <summary>
        /// Filtra y pagina los espacios según el texto de búsqueda y la página actual.
        /// </summary>
        private void FilterAndPaginateSpaces()
        {
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? AllSpaces
                : new ObservableCollection<Space>(AllSpaces.Where(s => s.Name.ToLower().Contains(SearchText.ToLower())));

            FilteredSpaces.Clear();
            foreach (var s in filtered)
            {
                FilteredSpaces.Add(s);
            }

            var paged = filtered.Skip((_currentPage - 1) * PageSize).Take(PageSize);

            PagedSpaces.Clear();
            foreach (var space in paged)
            {
                PagedSpaces.Add(space);
            }

            LoadAllMarkers();
        }

        /// <summary>
        /// Cambia a la página anterior, si es posible, y recarga los espacios.
        /// </summary>
        private void PreviousPage(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                FilterAndPaginateSpaces();
            }
        }

        /// <summary>
        /// Cambia a la siguiente página, si es posible, y recarga los espacios.
        /// </summary>
        private void NextPage(object sender, RoutedEventArgs e)
        {
            int totalItems = string.IsNullOrWhiteSpace(SearchText)
                ? AllSpaces.Count
                : AllSpaces.Count(s => s.Name.ToLower().Contains(SearchText.ToLower()));

            if ((_currentPage * PageSize) < totalItems)
            {
                _currentPage++;
                FilterAndPaginateSpaces();
            }
        }

        /// <summary>
        /// Configura el mapa cuando se carga la página y muestra la ubicación inicial.
        /// </summary>
        private void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            MapControl.MapProvider = GMapProviders.OpenStreetMap;
            MapControl.Position = new PointLatLng(41.3851, 2.1734); // Posición inicial (Barcelona)
            MapControl.MinZoom = 5;
            MapControl.MaxZoom = 18;
            MapControl.Zoom = 14;
            MapControl.CanDragMap = true;
            MapControl.ShowTileGridLines = false;
            MapControl.DragButton = System.Windows.Input.MouseButton.Left;

            UpdateMapClip();
            LoadAllMarkers();
        }

        /// <summary>
        /// Actualiza la región recortada del mapa cuando cambia el tamaño del control.
        /// </summary>
        private void MapControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateMapClip();
        }

        /// <summary>
        /// Actualiza la geometría del recorte del mapa en función del tamaño actual del control.
        /// </summary>
        private void UpdateMapClip()
        {
            clipGeometry.Rect = new Rect(0, 0, MapControl.ActualWidth, MapControl.ActualHeight);
            clipGeometry.RadiusX = 20;
            clipGeometry.RadiusY = 20;
        }

        /// <summary>
        /// Carga los marcadores en el mapa para todos los espacios filtrados.
        /// </summary>
        private void LoadAllMarkers()
        {
            MapControl.Markers.Clear();

            foreach (Space space in FilteredSpaces)
            {
                if (space.Latitude != 0 && space.Longitude != 0)
                {
                    GMapMarker marker = new GMapMarker(new PointLatLng(space.Latitude, space.Longitude));
                    marker.Shape = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Stroke = Brushes.DarkRed,
                        Fill = Brushes.Red,
                        StrokeThickness = 2
                    };
                    MapControl.Markers.Add(marker);
                }
            }
        }

        /// <summary>
        /// Enfoca el marcador del espacio seleccionado en el mapa.
        /// </summary>
        private void FocusOnSelectedMarker()
        {
            if (SelectedSpace != null && SelectedSpace.Latitude != 0 && SelectedSpace.Longitude != 0)
            {
                double lat = SelectedSpace.Latitude;
                double lng = SelectedSpace.Longitude;

                MapControl.Position = new PointLatLng(lat, lng);
                MapControl.Markers.Clear();

                GMapMarker marker = new GMapMarker(new PointLatLng(lat, lng));
                marker.Shape = new Ellipse
                {
                    Width = 14,
                    Height = 14,
                    Stroke = Brushes.Blue,
                    Fill = Brushes.LightBlue,
                    StrokeThickness = 3
                };
                MapControl.Markers.Add(marker);
            }
        }

        /// <summary>
        /// Evento que se dispara cuando una propiedad cambia en la clase.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifica que una propiedad ha cambiado para que los controles de la UI se actualicen.
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }
    }
}
