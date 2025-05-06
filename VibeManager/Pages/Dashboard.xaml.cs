using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using VibeManager.Models.Controllers;

namespace VibeManager.Pages
{
    public partial class Dashboard : UserControl
    {
        public Dashboard()
        {
            InitializeComponent();
            LoadDashboardData();
        }

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
