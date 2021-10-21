using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TurismoReal_Reservas.Core.Interfaces;
using TurismoReal_Reservas.Infra.Context;

namespace TurismoReal_Reservas.Infra.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        protected readonly OracleContext _context;

        public ReservaRepository()
        {
            _context = new OracleContext();
        }

        // GET ALL RESERVAS
        public async Task<List<object>> GetReservas()
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }

        // GET RESERVA BY ID
        public async Task<object> GetReserva(int id)
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }

        // ADD RESERVA
        public async Task<object> CreateReserva(object reserva)
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }

        // UPDATE RESERVA
        public async Task<object> UpdateReserva(int id, object reserva)
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }

        // DELETE RESERVA
        public async Task<object> DeleteReserva(int id)
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }

    }
}
