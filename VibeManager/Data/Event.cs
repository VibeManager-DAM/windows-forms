using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeManager.Data
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int Capacity { get; set; }
        public bool Seats { get; set; }
        public int NumRows { get; set; }
        public int NumColumns { get; set; }
        public string SpaceName { get; set; }
    }
}
