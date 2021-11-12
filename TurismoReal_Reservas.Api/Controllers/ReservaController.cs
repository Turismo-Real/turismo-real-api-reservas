using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TurismoReal_Reservas.Core.Entities;
using TurismoReal_Reservas.Core.Interfaces;

namespace TurismoReal_Reservas.Api.Controllers
{
    [Route("api/v1/[controller]")]
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
        public async Task<List<Reserva>> GetReservas()
        {
            List<Reserva> reservas = await _reservaRepository.GetReservas();
            return reservas;
        }

        // GET /api/v1/reserva/{id}
        [HttpGet("{id}")]
        public async Task<object> GetReserva(int id)
        {
            Reserva reserva = await _reservaRepository.GetReserva(id);
            if (reserva.idReserva == 0) return new { message = $"No existe reserva con id {id}." };
            return reserva;
        }

        // POST /api/v1/reserva/
        [HttpPost]
        public async Task<object> CreateReserva([FromBody] Reserva reserva)
        {
            int saved = await _reservaRepository.CreateReserva(reserva);

            if (saved == -1) return new { message = "Cliente no existe.", saved = false };
            if (saved == -2) return new { message = "Departamento no existe.", saved = false };
            if (saved == -3) return new { message = "Ya existe una reserva en las fechas ingresadas.", saved = false };
            if (saved == -4) return new { message = "La reserva no debe superar los 30 días.", saved = false };
            if (saved == 0) return new { message = "Error al ingresar reserva.", saved = false };
            return new { message = "Reserva ingresada correctamente.", saved = true, idReserva = saved };
        }

        // PUT /api/v1/reserva/{id}
        [HttpPut("{id}")]
        public async Task<object> UpdateReserva(int id, [FromBody] Reserva reserva)
        {
            int updated = await _reservaRepository.UpdateReserva(id, reserva);

            if (updated == -1) return new { message = $"No existe reserva con id {id}.", updated = false };
            if (updated == 0) return new { message = "Error al actualizar reserva", updated = false };
            return new { message = "Reserva actualizada.", updated = true };
        }

        // DELETE /api/v1/reserva/{id}
        [HttpDelete("{id}")]
        public async Task<object> DeleteReserva(int id)
        {
            int removed = await _reservaRepository.DeleteReserva(id);

            if (removed == -1) return new { message = $"No existe reserva con id {id}.", removed = false };
            if (removed == 0) return new { message = "Error al eliminar reserva.", removed = false };
            return new { message = "Reserva eliminada.", removed = true };
        }

    }
}
