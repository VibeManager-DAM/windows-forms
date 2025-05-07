using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeManager.Data
{
    public class UserSession
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string FullName { get; set; }
    }

}
