using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeManager.Models.Controllers
{
    public class ReservesOrm
    {
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
