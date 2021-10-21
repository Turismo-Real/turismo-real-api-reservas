using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TurismoReal_Reservas.Core.Interfaces;

namespace TurismoReal_Reservas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaRepository _reservaRepository;

        public ReservaController(IReservaRepository reservaRepository)
        {
            _reservaRepository = reservaRepository;
        }
        
        // GET /api/v1/reserva
        [HttpGet]
        public async Task<List<object>> GetReservas()
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }

        // GET /api/v1/reserva/{id}
        [HttpGet("{id}")]
        public async Task<object> GetReserva(int id)
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }

        // POST /api/v1/reserva/
        [HttpPost]
        public async Task<object> CreateReserva([FromBody] object reserva)
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }

        // PUT /api/v1/reserva/{id}
        [HttpPut("{id}")]
        public async Task<object> UpdateReserva(int id, [FromBody] object reserva)
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }

        // DELETE /api/v1/reserva/{id}
        [HttpDelete("{id}")]
        public async Task<object> DeleteReserva(int id)
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }

    }
}
