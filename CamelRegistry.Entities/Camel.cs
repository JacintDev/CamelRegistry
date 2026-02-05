using System.ComponentModel.DataAnnotations;

namespace CamelRegistry.Entities
{
    public class Camel
    {
        
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int HumbCount { get; set; }
        public DateTime? LastFed { get; set; }
    }
}
