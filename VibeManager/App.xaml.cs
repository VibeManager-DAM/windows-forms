using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VibeManager.Data;

namespace VibeManager
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static UserSession CurrentUser { get; set; }

        public void ChangeLanguage(string languageCode)
        {
            string dictionaryPath = $"Languages/strings.{languageCode}.xaml";

            ResourceDictionary newDictionary = new ResourceDictionary
            {
                Source = new Uri(dictionaryPath, UriKind.Relative)
            };

            // Manté la resta de recursos i només substitueix el d'idioma
            var existingDictionaries = Application.Current.Resources.MergedDictionaries
                                         .Where(d => !d.Source.OriginalString.Contains("Languages/strings"))
                                         .ToList();

            Application.Current.Resources.MergedDictionaries.Clear();

            // Reafegir els diccionaris existents
            foreach (var dict in existingDictionaries)
            {
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }

            // Afegir el nou diccionari d'idioma
            Application.Current.Resources.MergedDictionaries.Add(newDictionary);
        }
    }
}
