using System.Text.Json.Serialization;

namespace api.parcial1.Models
{
    public class Ciudades
    {
        public int Id { get; set; }
        public string? Ciudad { get; set; }
        public string? Estado { get; set; }
        [JsonIgnore]
        public virtual List<Clientes>? Clientes { get; set; }
    }
}
