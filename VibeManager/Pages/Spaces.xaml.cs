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
    public partial class Spaces : UserControl, INotifyPropertyChanged
    {
        private const int PageSize = 5;
        private int _currentPage = 1;

        public ObservableCollection<Space> AllSpaces { get; set; }
        public ObservableCollection<Space> PagedSpaces { get; set; }
        private ObservableCollection<Space> FilteredSpaces { get; set; } = new ObservableCollection<Space>();


        private Space _selectedSpace;
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

        private void LoadSpaces()
        {
            var spacesFromDb = SpacesOrm.SelectAllSpaces();

            AllSpaces.Clear();
            foreach (var space in spacesFromDb)
            {
                AllSpaces.Add(space);
            }
        }


        private void FilterAndPaginateSpaces()
        {
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? AllSpaces
                : new ObservableCollection<Space>(
                    AllSpaces.Where(s => s.Name.ToLower().Contains(SearchText.ToLower())));

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


        private void PreviousPage(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                FilterAndPaginateSpaces();
            }
        }

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

        private void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            MapControl.MapProvider = GMapProviders.OpenStreetMap;
            MapControl.Position = new PointLatLng(41.3851, 2.1734);
            MapControl.MinZoom = 5;
            MapControl.MaxZoom = 18;
            MapControl.Zoom = 14;
            MapControl.CanDragMap = true;
            MapControl.ShowTileGridLines = false;
            MapControl.DragButton = System.Windows.Input.MouseButton.Left;

            UpdateMapClip();
            LoadAllMarkers();
        }

        private void MapControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateMapClip();
        }

        private void UpdateMapClip()
        {
            clipGeometry.Rect = new Rect(0, 0, MapControl.ActualWidth, MapControl.ActualHeight);
            clipGeometry.RadiusX = 20;
            clipGeometry.RadiusY = 20;
        }

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }
    }
}
