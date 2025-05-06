using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeManager.Data;

namespace VibeManager.Models.Controllers
{
    public class SpacesOrm
    {
        public static List<Space> SelectAllSpaces()
        {
            try
            {
                var spaces = Orm.db.SPACES
                    .Select(s => new Space
                    {
                        Id = s.id,
                        Name = s.name,
                        Capacity = s.capacity,
                        Latitude = (double)s.latitude,
                        Longitude = (double)s.longitude,
                        Address = s.address,
                        SquareMeters = (double)s.square_meters
                    })
                    .ToList();

                return spaces;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(Orm.ErrorMessage(ex));
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error: " + ex.Message);
            }
            return new List<Space>();
        }

    }
}
