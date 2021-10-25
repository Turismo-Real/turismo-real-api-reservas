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
        public async Task<List<Reserva>> GetReservas()
        {
            _context.OpenConnection();
            OracleCommand cmd = new OracleCommand("sp_obten_reservas", _context.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.BindByName = true;
            cmd.Parameters.Add("reservas", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = (OracleDataReader) await cmd.ExecuteReaderAsync();

            List<Reserva> reservas = new List<Reserva>();
            while (reader.Read())
            {
                Reserva reserva = new Reserva();
                reserva.idReserva = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("id_reserva")).ToString());
                reserva.fecHoraReserva = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("fechora_reserva")).ToString());
                reserva.fecDesde = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("fec_desde")).ToString());
                reserva.fecHasta = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("fec_hasta")).ToString());
                reserva.valorArriendo = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("valor_arriendo")).ToString());
                reserva.fecHoraCheckIn = ConvertStringToDate(reader.GetValue(reader.GetOrdinal("fechora_checkin")).ToString());
                reserva.fecHoraCheckOut = ConvertStringToDate(reader.GetValue(reader.GetOrdinal("fechora_checkout")).ToString());
                reserva.checkInConforme = ConvertStringToBool(reader.GetValue(reader.GetOrdinal("checkin_conforme")).ToString());
                reserva.checkOutConforme = ConvertStringToBool(reader.GetValue(reader.GetOrdinal("checkout_conforme")).ToString());
                reserva.estadoCheckIn = reader.GetValue(reader.GetOrdinal("estado_checkin")).ToString();
                reserva.estadoCheckOut = reader.GetValue(reader.GetOrdinal("estado_checkout")).ToString();
                reserva.estadoReserva = reader.GetValue(reader.GetOrdinal("estado")).ToString();
                reserva.idUsuario = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("id_usuario")).ToString());
                reserva.idDepartamento = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("id_departamento")).ToString());
                reservas.Add(reserva);
            }
            _context.CloseConnection();
            return reservas;
        }

        // GET RESERVA BY ID
        public async Task<Reserva> GetReserva(int id)
        {
            _context.OpenConnection();
            OracleCommand cmd = new OracleCommand("sp_obten_reserva_id", _context.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.BindByName = true;
            cmd.Parameters.Add("reserva_id", OracleDbType.Int32).Direction = ParameterDirection.Input;
            cmd.Parameters.Add("reserva", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            cmd.Parameters["reserva_id"].Value = id;
            OracleDataReader reader = (OracleDataReader) await cmd.ExecuteReaderAsync();

            Reserva reserva = new Reserva();
            while (reader.Read())
            {
                reserva.idReserva = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("id_reserva")).ToString());
                reserva.fecHoraReserva = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("fechora_reserva")).ToString());
                reserva.fecDesde = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("fec_desde")).ToString());
                reserva.fecHasta = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("fec_hasta")).ToString());
                reserva.valorArriendo = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("valor_arriendo")).ToString());
                reserva.fecHoraCheckIn = ConvertStringToDate(reader.GetValue(reader.GetOrdinal("fechora_checkin")).ToString());
                reserva.fecHoraCheckOut = ConvertStringToDate(reader.GetValue(reader.GetOrdinal("fechora_checkout")).ToString());
                reserva.checkInConforme = ConvertStringToBool(reader.GetValue(reader.GetOrdinal("checkin_conforme")).ToString());
                reserva.checkOutConforme = ConvertStringToBool(reader.GetValue(reader.GetOrdinal("checkout_conforme")).ToString());
                reserva.estadoCheckIn = reader.GetValue(reader.GetOrdinal("estado_checkin")).ToString();
                reserva.estadoCheckOut = reader.GetValue(reader.GetOrdinal("estado_checkout")).ToString();
                reserva.estadoReserva = reader.GetValue(reader.GetOrdinal("estado")).ToString();
                reserva.idUsuario = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("id_usuario")).ToString());
                reserva.idDepartamento = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("id_departamento")).ToString());
            }
            _context.CloseConnection();
            return reserva;
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
        public async Task<int> UpdateReserva(int id, Reserva reserva)
        {
            _context.OpenConnection();
            OracleCommand cmd = new OracleCommand("sp_editar_reserva", _context.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.BindByName = true;
            cmd.Parameters.Add("reserva_id", OracleDbType.Int32).Direction = ParameterDirection.Input;
            cmd.Parameters.Add("checkin_conforme_r", OracleDbType.Int32).Direction = ParameterDirection.Input;
            cmd.Parameters.Add("checkout_conforme_r", OracleDbType.Int32).Direction = ParameterDirection.Input;
            cmd.Parameters.Add("estado_checkin_r", OracleDbType.Varchar2).Direction = ParameterDirection.Input;
            cmd.Parameters.Add("estado_checkout_r", OracleDbType.Varchar2).Direction = ParameterDirection.Input;
            cmd.Parameters.Add("estado_r", OracleDbType.Varchar2).Direction = ParameterDirection.Input;
            cmd.Parameters.Add("updated", OracleDbType.Int32).Direction = ParameterDirection.Output;

            cmd.Parameters["reserva_id"].Value = id;
            cmd.Parameters["checkin_conforme_r"].Value = reserva.checkInConforme ? 1 : 0;
            cmd.Parameters["checkout_conforme_r"].Value = reserva.checkOutConforme ? 1 : 0;
            cmd.Parameters["estado_checkin_r"].Value = reserva.estadoCheckIn;
            cmd.Parameters["estado_checkout_r"].Value = reserva.estadoCheckOut;
            cmd.Parameters["estado_r"].Value = reserva.estadoReserva;

            await cmd.ExecuteNonQueryAsync();
            int updated = int.Parse(cmd.Parameters["updated"].Value.ToString());
            _context.CloseConnection();
            return updated;
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

        // Convertir string bd a DateTime
        public DateTime? ConvertStringToDate(string fecha)
        {
            if (fecha.Equals(string.Empty)){
                return null;
            }
            return Convert.ToDateTime(fecha);
        }

        // Convertir string a bool
        public bool ConvertStringToBool(string conforme)
        {
            if (conforme.Equals(string.Empty)) return false;
            if (Convert.ToInt32(conforme) == 0) return false;
            return true;
        }

    }
}
