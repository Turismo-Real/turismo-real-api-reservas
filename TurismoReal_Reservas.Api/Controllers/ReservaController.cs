using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TurismoReal_Reservas.Core.Entities;
using TurismoReal_Reservas.Core.Interfaces;
using TurismoReal_Reservas.Core.Log;

namespace TurismoReal_Reservas.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly string serviceName = "turismo_real_reservas";

        public ReservaController(IReservaRepository reservaRepository)
        {
            _reservaRepository = reservaRepository;
        }
        
        // GET /api/v1/reserva
        [HttpGet]
        public async Task<List<Reserva>> GetReservas()
        {
            LogModel log = new LogModel();
            log.servicio = serviceName;
            log.method = "GET";
            log.endpoint = "/api/v1/reserva";
            DateTime startService = DateTime.Now;

            List<Reserva> reservas = await _reservaRepository.GetReservas();

            // LOG
            log.inicioSolicitud = startService;
            log.finSolicitud = DateTime.Now;
            log.tiempoSolicitud = (log.finSolicitud - log.inicioSolicitud).TotalMilliseconds + " ms";
            log.statusCode = 200;
            log.response = "Lista de reservas";
            Console.WriteLine(log.parseJson());
            // LOG
            return reservas;
        }

        // GET /api/v1/reserva/{id}
        [HttpGet("{id}")]
        public async Task<object> GetReserva(int id)
        {
            LogModel log = new LogModel();
            log.servicio = serviceName;
            log.method = "GET";
            log.endpoint = "/api/v1/reserva/{id}";
            DateTime startService = DateTime.Now;

            Reserva reserva = await _reservaRepository.GetReserva(id);

            if (reserva.idReserva == 0)
            {
                var notFoundResponse = new { message = $"No existe reserva con id {id}." };
                // LOG
                log.inicioSolicitud = startService;
                log.finSolicitud = DateTime.Now;
                log.tiempoSolicitud = (log.finSolicitud - log.inicioSolicitud).TotalMilliseconds + " ms";
                log.statusCode = 200;
                log.response = notFoundResponse;
                Console.WriteLine(log.parseJson());
                // LOG
            }
            // LOG
            log.inicioSolicitud = startService;
            log.finSolicitud = DateTime.Now;
            log.tiempoSolicitud = (log.finSolicitud - log.inicioSolicitud).TotalMilliseconds + " ms";
            log.statusCode = 200;
            log.response = reserva;
            Console.WriteLine(log.parseJson());
            // LOG
            return reserva;
        }

        // POST /api/v1/reserva/
        [HttpPost]
        public async Task<object> CreateReserva([FromBody] Reserva reserva)
        {
            LogModel log = new LogModel();
            log.servicio = serviceName;
            log.method = "POST";
            log.endpoint = "/api/v1/reserva";
            log.inicioSolicitud = DateTime.Now;

            int saved = await _reservaRepository.CreateReserva(reserva);

            if (saved == -1)
            {
                var notFoundClient = new { message = "Cliente no existe.", saved = false };
                // LOG
                log.EndLog(DateTime.Now, 200, notFoundClient);
                Console.WriteLine(log.parseJson());
                // LOG
                return notFoundClient;
            }
            if (saved == -2)
            {
                var notFoundDepto = new { message = "Departamento no existe.", saved = false };
                // LOG
                log.EndLog(DateTime.Now, 200, notFoundDepto);
                Console.WriteLine(log.parseJson());
                // LOG
                return notFoundDepto;
            }
            if (saved == -3)
            {
                var reservaExists = new { message = "Ya existe una reserva en las fechas ingresadas.", saved = false };
                // LOG
                log.EndLog(DateTime.Now, 200, reservaExists);
                Console.WriteLine(log.parseJson());
                // LOG
                return reservaExists;
            }
            if (saved == -4)
            {
                var tooDays = new { message = "La reserva no debe superar los 30 días.", saved = false };
                // LOG
                log.EndLog(DateTime.Now, 200, tooDays);
                Console.WriteLine(log.parseJson());
                // LOG
                return tooDays;
            }
            if (saved == 0)
            {
                var error = new { message = "Error al ingresar reserva.", saved = false };
                // LOG
                log.EndLog(DateTime.Now, 200, error);
                Console.WriteLine(log.parseJson());
                // LOG
                return error;
            }

            Reserva nuevaReserva = await _reservaRepository.GetReserva(saved);
            var responseOK = new { message = "Reserva ingresada correctamente.", saved = true, reserva = nuevaReserva };
            // LOG
            log.EndLog(DateTime.Now, 200, responseOK);
            Console.WriteLine(log.parseJson());
            // LOG
            return responseOK;
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
