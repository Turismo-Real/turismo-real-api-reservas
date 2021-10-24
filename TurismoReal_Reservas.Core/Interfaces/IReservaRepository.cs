using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TurismoReal_Reservas.Core.Entities;

namespace TurismoReal_Reservas.Core.Interfaces
{
    public interface IReservaRepository
    {
        // GET ALL RESERVAS
        Task<List<object>> GetReservas();
        // GET RESERVA BY ID
        Task<Reserva> GetReserva(int id);
        // ADD RESERVA
        Task<int> CreateReserva(Reserva reserva);
        // UPDATE RESERVA
        Task<object> UpdateReserva(int id, object reserva);
        // DELETE RESERVA
        Task<int> DeleteReserva(int id);
    }
}
