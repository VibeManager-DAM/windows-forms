using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeManager.Models.Controllers
{

    /// <summary>
    /// Clase encargada de proporcionar acceso a la base de datos y manejo centralizado de errores SQL.
    /// </summary>
    class Orm
    {
        /// <summary>
        /// Instancia estática del contexto de base de datos que permite interactuar con las entidades de la aplicación.
        /// </summary>
        public static VibeEntities db = new VibeEntities();

        /// <summary>
        /// Devuelve un mensaje de error personalizado basado en el número del error de una excepción <see cref="SqlException"/>.
        /// </summary>
        /// <param name="sqlException">La excepción SQL capturada.</param>
        /// <returns>Una cadena que describe el error de manera comprensible para el usuario.</returns>
        public static string ErrorMessage(SqlException sqlException)
        {
            string message = "";

            switch (sqlException.Number)
            {
                case 2:
                    message = "The server is not operational.";
                    break;
                case 547:
                    message = "The record cannot be deleted because it has related records.";
                    break;
                case 4060:
                    message = "Could not connect to the database.";
                    break;
                case 18456:
                    message = "Login failed.";
                    break;
                case 2601:
                    message = "A record with the same value already exists.";
                    break;
                default:
                    message = sqlException.Number + " - " + sqlException.Message;
                    break;
            }

            return message;
        }
    }
}
