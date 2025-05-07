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
using VibeManager.Models.Controllers;

namespace VibeManager.Pages
{
    /// <summary>
    /// Control de usuario que maneja la gestión de eventos y la visualización en el mapa de GMap.
    /// Permite seleccionar espacios, ver eventos y gestionarlos (agregar, editar, eliminar).
    /// </summary>
    public partial class MenuEvent : UserControl
    {
        /// <summary>
        /// Lista de espacios con sus coordenadas y capacidad.
        /// </summary>
        public List<Space> Spaces { get; set; }

        /// <summary>
        /// Espacio seleccionado actualmente.
        /// </summary>
        public Space SelectedSpace { get; set; }

        /// <summary>
        /// Inicializa el control de usuario y configura el mapa GMap y los datos de eventos.
        /// </summary>
        public MenuEvent()
        {
            InitializeComponent();
            EventsDataGrid.ItemsSource = EventsOrm.GetAllEvents();

            // Configuración del mapa GMap
            GMapControl.MapProvider = GMapProviders.GoogleMap;
            GMapControl.Position = new PointLatLng(41.3784, 2.1925); // Centrado en Barcelona
            GMapControl.MinZoom = 2;
            GMapControl.MaxZoom = 18;
            GMapControl.Zoom = 12; // Zoom adecuado para Barcelona

            // Cargar los espacios con sus coordenadas y capacidades
            LoadSpaces();
        }

        /// <summary>
        /// Carga los espacios disponibles y sus ubicaciones, agregando marcadores en el mapa y poblando el ComboBox.
        /// </summary>
        private void LoadSpaces()
        {
            Spaces = SpacesOrm.SelectAllSpaces();

            SpaceComboBox.Items.Clear();

            foreach (var space in Spaces)
            {
                SpaceComboBox.Items.Add(space.Name);
            }

            GMapControl.Markers.Clear();

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

        /// <summary>
        /// Evento que se dispara cuando se selecciona un espacio en el ComboBox.
        /// Centra el mapa en el espacio seleccionado y actualiza la capacidad.
        /// </summary>
        /// <param name="sender">El origen del evento (ComboBox).</param>
        /// <param name="e">El argumento del evento.</param>
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

        /// <summary>
        /// Evento para manejar el clic en el mapa y seleccionar un espacio.
        /// Busca el espacio más cercano al punto de clic.
        /// </summary>
        /// <param name="sender">El origen del evento (GMapControl).</param>
        /// <param name="e">El argumento del evento.</param>
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

        /// <summary>
        /// Busca el espacio más cercano a las coordenadas proporcionadas.
        /// </summary>
        /// <param name="clickedPoint">Las coordenadas del punto en el mapa.</param>
        /// <returns>El espacio más cercano.</returns>
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

        /// <summary>
        /// Calcula la distancia en metros entre dos puntos geográficos utilizando la fórmula de Haversine.
        /// </summary>
        /// <param name="lat1">Latitud del primer punto.</param>
        /// <param name="lon1">Longitud del primer punto.</param>
        /// <param name="lat2">Latitud del segundo punto.</param>
        /// <param name="lon2">Longitud del segundo punto.</param>
        /// <returns>La distancia en metros.</returns>

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

        /// <summary>
        /// Convierte grados a radianes.
        /// </summary>
        /// <param name="degrees">Valor en grados.</param>
        /// <returns>El valor en radianes.</returns>
        private double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        /// <summary>
        /// Actualiza los marcadores en el mapa, colocando un marcador rojo para todos los espacios
        /// y un marcador azul para el espacio seleccionado.
        /// </summary>
        /// <param name="selectedSpace">El espacio seleccionado que se mostrará con el marcador azul.</param>
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

        /// <summary>
        /// Activa los controles relacionados con la disponibilidad de asientos cuando la casilla está marcada.
        /// </summary>
        /// <param name="sender">El origen del evento (CheckBox).</param>
        /// <param name="e">El argumento del evento.</param>
        private void SeatsAvailableCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SeatsPanel.IsEnabled = true;
        }

        /// <summary>
        /// Desactiva los controles relacionados con la disponibilidad de asientos cuando la casilla está desmarcada.
        /// </summary>
        /// <param name="sender">El origen del evento (CheckBox).</param>
        /// <param name="e">El argumento del evento.</param>
        private void SeatsAvailableCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SeatsPanel.IsEnabled = false;
        }

        /// <summary>
        /// Guarda el evento en la base de datos, ya sea creando un evento nuevo o actualizando uno existente.
        /// </summary>
        /// <param name="sender">El origen del evento (Button).</param>
        /// <param name="e">El argumento del evento.</param>
        private void SaveEventButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string title = TitleTextBox.Text;
                string description = DescriptionTextBox.Text;
                DateTime? selectedDate = DatePicker.SelectedDate;
                if (!selectedDate.HasValue)
                {
                    MessageBox.Show("Selecciona una fecha.");
                    return;
                }

                TimeSpan time = TimePickerControl.Value?.TimeOfDay ?? TimeSpan.Zero;
                int capacity = int.Parse(CapacityTextBox.Text);
                bool seatsAvailable = SeatsAvailableCheckBox.IsChecked == true;
                int rows = seatsAvailable ? int.Parse(RowsTextBox.Text) : 0;
                int columns = seatsAvailable ? int.Parse(ColumnsTextBox.Text) : 0;
                string selectedSpace = SpaceComboBox.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(selectedSpace))
                {
                    MessageBox.Show("Selecciona un espacio.");
                    return;
                }

                Event eventToSave;

                if (EventsDataGrid.SelectedItem is Event selectedEvent)
                {
                    // Modo edición
                    eventToSave = selectedEvent;
                }
                else
                {
                    // Nuevo evento
                    eventToSave = new Event();
                }

                eventToSave.Title = title;
                eventToSave.Description = description;
                eventToSave.Date = selectedDate.Value;
                eventToSave.Time = time;
                eventToSave.Capacity = capacity;
                eventToSave.Seats = seatsAvailable;
                eventToSave.NumRows = rows;
                eventToSave.NumColumns = columns;

                bool success = EventsOrm.CreateOrUpdateEvent(eventToSave, selectedSpace);
                if (success)
                {
                    MessageBox.Show("Evento guardado correctamente.");
                    EventsDataGrid.ItemsSource = EventsOrm.GetAllEvents();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Error al guardar el evento.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Evento que se dispara al hacer clic en el botón para limpiar los campos del formulario.
        /// Llama al método <see cref="ClearFields"/> para resetear todos los campos.
        /// </summary>
        /// <param name="sender">El origen del evento (Button).</param>
        /// <param name="e">El argumento del evento.</param>
        private void ClearFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        /// <summary>
        /// Limpia todos los campos del formulario, incluyendo los controles de texto, fechas y casillas.
        /// </summary>
        private void ClearFields()
        {
            TitleTextBox.Clear();
            DescriptionTextBox.Clear();
            DatePicker.SelectedDate = null;
            TimePickerControl.Value = null;
            CapacityTextBox.Clear();
            SeatsAvailableCheckBox.IsChecked = false;
            RowsTextBox.Clear();
            ColumnsTextBox.Clear();
            SpaceComboBox.SelectedIndex = -1;
            EventsDataGrid.UnselectAll();
        }

        /// <summary>
        /// Evento que se dispara al hacer clic en el botón para eliminar un evento seleccionado.
        /// Si el evento está seleccionado, se elimina de la base de datos y se actualiza la vista.
        /// </summary>
        /// <param name="sender">El origen del evento (Button).</param>
        /// <param name="e">El argumento del evento.</param>
        private void DeleteEventButton_Click(object sender, RoutedEventArgs e)
        {
            if (EventsDataGrid.SelectedItem is Event selected)
            {
                bool deleted = EventsOrm.DeleteEventById(selected.Id);
                if (deleted)
                {
                    MessageBox.Show("Evento eliminado correctamente.");
                    EventsDataGrid.ItemsSource = EventsOrm.GetAllEvents();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Error al eliminar el evento.");
                }
            }
        }

        /// <summary>
        /// Evento que se dispara cuando se cambia la selección de un evento en el DataGrid.
        /// Rellena los campos del formulario con los datos del evento seleccionado.
        /// </summary>
        /// <param name="sender">El origen del evento (DataGrid).</param>
        /// <param name="e">El argumento del evento.</param>
        private void EventsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EventsDataGrid.SelectedItem is Event selectedEvent)
            {
                TitleTextBox.Text = selectedEvent.Title;
                DescriptionTextBox.Text = selectedEvent.Description;
                DatePicker.SelectedDate = selectedEvent.Date;
                TimePickerControl.Value = DateTime.Today.Add(selectedEvent.Time);
                CapacityTextBox.Text = selectedEvent.Capacity.ToString();
                SeatsAvailableCheckBox.IsChecked = selectedEvent.Seats;
                RowsTextBox.Text = selectedEvent.NumRows.ToString();
                ColumnsTextBox.Text = selectedEvent.NumColumns.ToString();
                SpaceComboBox.SelectedItem = selectedEvent.SpaceName;

                var space = Spaces.Find(s => s.Name == selectedEvent.SpaceName);
                if (space != null)
                {
                    GMapControl.Position = new PointLatLng(space.Latitude, space.Longitude);
                    UpdateMarkers(space);
                }
            }
        }
    }
}
