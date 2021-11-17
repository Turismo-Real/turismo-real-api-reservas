namespace TurismoReal_Reservas.Core.Entities
{
    public class Servicio
    {
        public int id { get; set; }
        public int idServicio { get; set; }
        public string servicio { get; set; }
        public string tipo { get; set; }
        public double valor { get; set; }
        public int conductor { get; set; }
        public string origen { get; set; }
    }
}
