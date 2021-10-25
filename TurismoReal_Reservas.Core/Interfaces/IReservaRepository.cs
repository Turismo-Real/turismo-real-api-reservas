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
        Task<List<Reserva>> GetReservas();
        // GET RESERVA BY ID
        Task<Reserva> GetReserva(int id);
        // ADD RESERVA
        Task<int> CreateReserva(Reserva reserva);
        // UPDATE RESERVA
        Task<int> UpdateReserva(int id, Reserva reserva);
        // DELETE RESERVA
        Task<int> DeleteReserva(int id);
    }
}
