using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeManager.Models.Controllers
{
    /// <summary>
    /// Proporciona métodos para operar sobre las reservas en la base de datos.
    /// </summary>
    public class ReservesOrm
    {
        /// <summary>
        /// Obtiene el número total de reservas registradas en la base de datos.
        /// </summary>
        /// <returns>El número total de reservas como un entero.</returns>
        public static int getTotalReserves()
        {
            int totalReserves = 0;

            try
            {
                totalReserves = Orm.db.RESERVES
                    .Count();
            }
            catch (SqlException sqlException)
            {
                Console.WriteLine(Orm.ErrorMessage(sqlException));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }


            return totalReserves;
        }
    }
}
