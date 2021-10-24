using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TurismoReal_Reservas.Core.Entities;
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
        public async Task<int> CreateReserva(Reserva reserva)
        {
            _context.OpenConnection();
            OracleCommand cmd = new OracleCommand("sp_agregar_reserva", _context.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.BindByName = true;
            cmd.Parameters.Add("fec_desde_r", OracleDbType.Varchar2).Direction = ParameterDirection.Input;
            cmd.Parameters.Add("fec_hasta_r", OracleDbType.Varchar2).Direction = ParameterDirection.Input;
            cmd.Parameters.Add("id_usuario_r", OracleDbType.Int32).Direction = ParameterDirection.Input;
            cmd.Parameters.Add("id_departamento_r", OracleDbType.Int32).Direction = ParameterDirection.Input;
            cmd.Parameters.Add("saved", OracleDbType.Int32).Direction = ParameterDirection.Output;

            cmd.Parameters["fec_desde_r"].Value = reserva.fecDesde.ToString("dd/MM/yyyy");
            cmd.Parameters["fec_hasta_r"].Value = reserva.fecHasta.ToString("dd/MM/yyyy");
            cmd.Parameters["id_usuario_r"].Value = reserva.idUsuario;
            cmd.Parameters["id_departamento_r"].Value = reserva.idDepartamento;

            await cmd.ExecuteNonQueryAsync();
            // Retorna el id de la reserva
            int saved = Convert.ToInt32(cmd.Parameters["saved"].Value.ToString());
            _context.CloseConnection();
            return saved;
        }

        // UPDATE RESERVA
        public async Task<object> UpdateReserva(int id, object reserva)
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }

        // DELETE RESERVA
        public async Task<int> DeleteReserva(int id)
        {
            _context.OpenConnection();
            OracleCommand cmd = new OracleCommand("sp_eliminar_reserva", _context.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.BindByName = true;

            cmd.Parameters.Add("reserva_id", OracleDbType.Int32).Direction = ParameterDirection.Input;
            cmd.Parameters.Add("removed", OracleDbType.Int32).Direction = ParameterDirection.Output;

            cmd.Parameters["reserva_id"].Value = id;
            await cmd.ExecuteNonQueryAsync();
            int removed = Convert.ToInt32(cmd.Parameters["removed"].Value.ToString());
            return removed;
        }

    }
}
