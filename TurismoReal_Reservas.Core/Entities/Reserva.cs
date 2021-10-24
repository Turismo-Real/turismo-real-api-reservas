using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TurismoReal_Reservas.Core.Entities
{
    public class Reserva
    {
        public int idReserva { get; set; }
        public DateTime fecHoraReserva { get; set; }

        [DataType(DataType.Date)]
        public DateTime fecDesde { get; set; }

        [DataType(DataType.Date)]
        public DateTime fecHasta { get; set; }
        public double valorArriendo { get; set; }
        public DateTime? fecHoraCheckIn { get; set; }
        public DateTime? fecHoraCheckOut { get; set; }
        public bool checkInConforme { get; set; }
        public bool checkOutConforme { get; set; }
        public string estadoCheckIn { get; set; }
        public string estadoCheckOut { get; set; }
        public string estadoReserva { get; set; }
        public int idUsuario { get; set; }
        public int idDepartamento { get; set; }
        public List<Servicio> servicios { get; set; }
        public List<Asistente> asistentes { get; set; }
    }
}
