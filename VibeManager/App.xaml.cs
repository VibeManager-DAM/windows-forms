using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VibeManager.Data;

namespace VibeManager
{
    /// <summary>
    /// Clase principal de la aplicación que gestiona el ciclo de vida de la misma.
    /// Incluye funcionalidades para cambiar el idioma de la interfaz de usuario.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Propiedad estática que almacena la sesión del usuario actual.
        /// </summary>
        public static UserSession CurrentUser { get; set; }

        /// <summary>
        /// Cambia el idioma de la interfaz de usuario de la aplicación.
        /// Carga un nuevo diccionario de recursos basado en el código de idioma proporcionado.
        /// </summary>
        /// <param name="languageCode">El código del idioma (por ejemplo, "en" para inglés, "es" para español).</param>
        public void ChangeLanguage(string languageCode)
        {
            string dictionaryPath = $"Languages/strings.{languageCode}.xaml";

            // Cargar el nuevo diccionario de recursos
            ResourceDictionary newDictionary = new ResourceDictionary
            {
                Source = new Uri(dictionaryPath, UriKind.Relative)
            };

            // Mantener el resto de recursos y solo sustituir el de idioma
            var existingDictionaries = Application.Current.Resources.MergedDictionaries
                                         .Where(d => !d.Source.OriginalString.Contains("Languages/strings"))
                                         .ToList();

            // Limpiar los diccionarios de recursos existentes
            Application.Current.Resources.MergedDictionaries.Clear();

            // Volver a añadir los diccionarios existentes
            foreach (var dict in existingDictionaries)
            {
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }

            // Añadir el nuevo diccionario de idioma
            Application.Current.Resources.MergedDictionaries.Add(newDictionary);
        }
    }
}
