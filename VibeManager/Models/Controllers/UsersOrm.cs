using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeManager.Models.Controllers
{
    public static class UsersOrm
    {
        public static int? Login(string username, string password)
        {
            int? idRol = null;

            try
            {
                idRol = Orm.db.USERS
                    .Where(u => u.fullname == username || u.email == username && u.password == password)
                    .Select(u => u.id_rol)
                    .FirstOrDefault();
            }
            catch (SqlException sqlException)
            {
                Console.WriteLine(Orm.ErrorMessage(sqlException));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return idRol;
        }
    }
}
