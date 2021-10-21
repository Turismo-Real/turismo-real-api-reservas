using System;

namespace TurismoReal_Reservas.Core.Entities
{
    public class Servicio
    {
        public int idServicio { get; set; }
        public DateTime fecDesde { get; set; }
        public DateTime fecHasta { get; set; }
        public double valor { get; set; }
        public string origen { get; set; }
        public DateTime fecHoraLlegada { get; set; }
        public int conductor { get; set; }
    }
}
