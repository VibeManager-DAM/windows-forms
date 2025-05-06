using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation; // Importar el espacio de nombres adecuado para WPF
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Media;
using VibeManager.Data;

namespace VibeManager.Pages
{
    public partial class MenuEvent : UserControl
    {
        // Lista de espacios con sus coordenadas y capacidad (simulada)
        public List<Space> Spaces { get; set; }

        // Espacio seleccionado actualmente
        public Space SelectedSpace { get; set; }

        public MenuEvent()
        {
            InitializeComponent();

            // Configuración del mapa GMap
            GMapControl.MapProvider = GMapProviders.GoogleMap;
            GMapControl.Position = new PointLatLng(41.3784, 2.1925); // Centrado en Barcelona
            GMapControl.MinZoom = 2;
            GMapControl.MaxZoom = 18;
            GMapControl.Zoom = 12; // Zoom adecuado para Barcelona

            // Cargar los espacios con sus coordenadas y capacidades
            LoadSpaces();
        }

        // Función para cargar los espacios de ejemplo (con latitud, longitud y capacidad)
        private void LoadSpaces()
        {
            // Aquí cargarías los espacios desde la base de datos, pero por ahora los simulo
            Spaces = new List<Space>
            {
                new Space { Name = "Espacio 1 - Sala A", Latitude = 41.3784, Longitude = 2.1925, Capacity = 50 },
                new Space { Name = "Espacio 2 - Sala B", Latitude = 41.3851, Longitude = 2.1734, Capacity = 100 },
                new Space { Name = "Espacio 3 - Sala C", Latitude = 41.4023, Longitude = 2.1919, Capacity = 200 }
            };

            // Agregar los espacios al ComboBox
            foreach (var space in Spaces)
            {
                SpaceComboBox.Items.Add(space.Name);
            }

            // Agregar los marcadores de los espacios al mapa
            foreach (var space in Spaces)
            {
                var marker = new GMapMarker(new PointLatLng(space.Latitude, space.Longitude))
                {
                    Shape = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Fill = Brushes.Red
                    }
                };
                GMapControl.Markers.Add(marker);
            }
        }

        // Evento cuando se selecciona un espacio en el ComboBox
        private void SpaceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSpaceName = (string)SpaceComboBox.SelectedItem;
            var selectedSpace = Spaces.Find(space => space.Name == selectedSpaceName);

            if (selectedSpace != null)
            {
                // Centrar el mapa en las coordenadas del espacio seleccionado
                GMapControl.Position = new PointLatLng(selectedSpace.Latitude, selectedSpace.Longitude);

                // Actualizar la capacidad automáticamente en el TextBox
                CapacityTextBox.Text = selectedSpace.Capacity.ToString();

                // Actualizar el marcador para el espacio seleccionado (nuevo marcador azul)
                UpdateMarkers(selectedSpace);
            }
        }

        // Evento para manejar el clic en el mapa (seleccionar un espacio al hacer clic en el mapa)
        private void GMapControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(GMapControl);
            var latLng = GMapControl.FromLocalToLatLng((int)point.X, (int)point.Y);

            // Buscar el espacio más cercano
            var nearestSpace = FindNearestSpace(latLng);
            if (nearestSpace != null)
            {
                // Seleccionar el espacio
                SelectedSpace = nearestSpace;

                // Mostrar la capacidad en el TextBox
                CapacityTextBox.Text = nearestSpace.Capacity.ToString();

                // Actualizar la ComboBox y centrar el mapa en el espacio seleccionado
                SpaceComboBox.SelectedItem = nearestSpace.Name;
                GMapControl.Position = new PointLatLng(nearestSpace.Latitude, nearestSpace.Longitude);

                // Actualizar el marcador para el espacio seleccionado
                UpdateMarkers(nearestSpace);
            }
        }

        // Función para encontrar el espacio más cercano al punto de clic
        private Space FindNearestSpace(PointLatLng clickedPoint)
        {
            Space nearestSpace = null;
            double minDistance = double.MaxValue;

            foreach (var space in Spaces)
            {
                // Usar la fórmula de Haversine para calcular la distancia entre dos puntos geográficos
                var distance = HaversineDistance(clickedPoint.Lat, clickedPoint.Lng, space.Latitude, space.Longitude);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestSpace = space;
                }
            }

            return nearestSpace;
        }

        // Fórmula de Haversine para calcular la distancia en metros entre dos puntos geográficos
        private double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371000; // Radio de la Tierra en metros
            var lat1Rad = ToRadians(lat1);
            var lon1Rad = ToRadians(lon1);
            var lat2Rad = ToRadians(lat2);
            var lon2Rad = ToRadians(lon2);

            var dLat = lat2Rad - lat1Rad;
            var dLon = lon2Rad - lon1Rad;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = R * c; // Distancia en metros
            return distance;
        }

        // Convierte grados a radianes
        private double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        // Función para actualizar los marcadores en el mapa
        private void UpdateMarkers(Space selectedSpace)
        {
            // Limpiar solo el marcador azul (seleccionado)
            GMapControl.Markers.Clear();

            // Agregar los marcadores de todos los espacios (en rojo)
            foreach (var space in Spaces)
            {
                var marker = new GMapMarker(new PointLatLng(space.Latitude, space.Longitude))
                {
                    Shape = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Fill = Brushes.Red
                    }
                };
                GMapControl.Markers.Add(marker);
            }

            // Agregar un marcador azul para el espacio seleccionado
            var selectedMarker = new GMapMarker(new PointLatLng(selectedSpace.Latitude, selectedSpace.Longitude))
            {
                Shape = new Ellipse
                {
                    Width = 15,
                    Height = 15,
                    Fill = Brushes.Blue
                }
            };
            GMapControl.Markers.Add(selectedMarker);
        }

        // Evento para gestionar el checkbox de asientos disponibles
        private void SeatsAvailableCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Mostrar los campos para filas y columnas
            SeatsPanel.Visibility = Visibility.Visible;
        }

        private void SeatsAvailableCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Ocultar los campos para filas y columnas
            SeatsPanel.Visibility = Visibility.Collapsed;
        }

        // Guardar evento
        private void SaveEventButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string description = DescriptionTextBox.Text;
            DateTime date = DatePicker.SelectedDate.Value;
            TimeSpan time = TimeSpan.Parse(((ComboBoxItem)TimeComboBox.SelectedItem).Content.ToString());
            int capacity = int.Parse(CapacityTextBox.Text);
            bool seatsAvailable = SeatsAvailableCheckBox.IsChecked.Value;
            int rows = seatsAvailable ? int.Parse(RowsTextBox.Text) : 0;
            int columns = seatsAvailable ? int.Parse(ColumnsTextBox.Text) : 0;
            string selectedSpace = SpaceComboBox.SelectedItem?.ToString();

            // Aquí puedes guardar los datos en la base de datos
            MessageBox.Show("Evento guardado exitosamente.");
        }
    }

}
