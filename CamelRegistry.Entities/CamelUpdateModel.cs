using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelRegistry.Entities
{
    public class CamelUpdateModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Color { get; set; }
        public int HumbCount { get; set; }
        public DateTime? LastFed { get; set; }
    }
}
