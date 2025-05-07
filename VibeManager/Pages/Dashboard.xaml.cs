using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using VibeManager.Models.Controllers;

namespace VibeManager.Pages
{
    /// <summary>
    /// Representa la página del panel principal del sistema, mostrando estadísticas generales y gráficos.
    /// </summary>
    public partial class Dashboard : UserControl
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Dashboard"/> y carga los datos del panel.
        /// </summary>
        public Dashboard()
        {
            InitializeComponent();
            LoadDashboardData();
        }

        /// <summary>
        /// Carga los datos estadísticos del sistema como el total de usuarios, eventos y reservas,
        /// y los muestra en los controles correspondientes.
        /// </summary>
        private void LoadDashboardData()
        {

            int totalUsers = UsersOrm.getTotalUsers(); // Ejemplo: obtener de base de datos
            int totalEvents = EventsOrm.getTotalEvents(); ; // Ejemplo: obtener de base de datos
            int totalReservations = ReservesOrm.getTotalReserves(); // Ejemplo: obtener de base de datos

            // Mostrar datos en los controles TextBlock
            TotalUsers.Text = totalUsers.ToString();
            TotalEvents.Text = totalEvents.ToString();
            TotalReservations.Text = totalReservations.ToString();

            // Llenar los gráficos con datos de ejemplo (estos también deberían ser dinámicos)
            LoadActivityChart();

        }

        /// <summary>
        /// Genera y muestra un gráfico de barras con datos de ejemplo de actividad de reservas y eventos.
        /// </summary>
        private void LoadActivityChart()
        {
            // Crear el modelo del gráfico
            var model = new PlotModel { Title = "Actividad de Reservas y Eventos" };

            // Crear la serie de barras (BarSeries)
            var series = new BarSeries
            {
                ItemsSource = new List<BarItem>
                {
                    new BarItem { Value = 10 }, // Ejemplo: Número de reservas
                    new BarItem { Value = 20 }  // Ejemplo: Número de eventos
                },
                LabelPlacement = LabelPlacement.Inside, // Etiquetas dentro de las barras
                LabelFormatString = "{0}" // Formato de las etiquetas
            };

            // Agregar la serie al modelo
            model.Series.Add(series);

            // Asignar el modelo al gráfico
            ActivityChart.Model = model;
        }
    }
}
