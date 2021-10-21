using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TurismoReal_Reservas.Core.Interfaces
{
    public interface IReservaRepository
    {
        // GET ALL RESERVAS
        Task<List<object>> GetReservas();
        // GET RESERVA BY ID
        Task<object> GetReserva(int id);
        // ADD RESERVA
        Task<object> CreateReserva(object reserva);
        // UPDATE RESERVA
        Task<object> UpdateReserva(int id, object reserva);
        // DELETE RESERVA
        Task<object> DeleteReserva(int id);
    }
}
