using Microsoft.Extensions.Hosting;

namespace LaRosalinaAPI.Models
{
    public partial class DashboardIndicadores
    {
        public decimal ocupacion { get; set; }
        public decimal ppn { get; set; }
        public decimal ingresos { get; set; }
        public decimal saldo { get; set; }
        public decimal efectivo { get; set; }
    }
}
