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
                reserva.servicios = GetServicios(reserva.idReserva);
                reserva.asistentes = GetAsistentes(reserva.idReserva);
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
                reserva.servicios = GetServicios(reserva.idReserva);
                reserva.asistentes = GetAsistentes(reserva.idReserva);
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
            // Agregar servicios
            if (saved <= 0) return saved;
            AddServices(saved, reserva.servicios);
            // Agregar asistentes
            AddAsistentes(saved, reserva.asistentes);
            _context.CloseConnection();
            return saved;
        }

        public bool AddServices(int id, List<Servicio> services)
        {
            try
            {
                OracleCommand cmd = new OracleCommand("sp_agregar_servicio_reserva", _context.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.BindByName = true;
                cmd.Parameters.Add("reserva_id", OracleDbType.Int32).Direction = ParameterDirection.Input;
                cmd.Parameters.Add("servicio_id", OracleDbType.Int32).Direction = ParameterDirection.Input;
                cmd.Parameters.Add("conductor_id", OracleDbType.Int32).Direction = ParameterDirection.Input;
                cmd.Parameters.Add("saved", OracleDbType.Int32).Direction = ParameterDirection.Output;

                foreach (Servicio service in services)
                {
                    cmd.Parameters["reserva_id"].Value = id;
                    cmd.Parameters["servicio_id"].Value = service.idServicio;
                    cmd.Parameters["conductor_id"].Value = service.conductor;
                    OracleDataReader reader = cmd.ExecuteReader();
                    int saved = int.Parse(cmd.Parameters["saved"].Value.ToString());
                }
                return true;
            } catch(Exception ex)
            {
                return false;
            }
        } 

        public bool AddAsistentes(int id, List<Asistente> asistentes)
        {
            try
            {
                OracleCommand cmd = new OracleCommand("sp_agregar_asistente_reserva", _context.GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.BindByName = true;
                cmd.Parameters.Add("reserva_id", OracleDbType.Int32).Direction = ParameterDirection.Input;
                cmd.Parameters.Add("pasaporte", OracleDbType.Varchar2).Direction = ParameterDirection.Input;
                cmd.Parameters.Add("numrut", OracleDbType.Varchar2).Direction = ParameterDirection.Input;
                cmd.Parameters.Add("dvrut", OracleDbType.Varchar2).Direction = ParameterDirection.Input;
                cmd.Parameters.Add("pnombre", OracleDbType.Varchar2).Direction = ParameterDirection.Input;
                cmd.Parameters.Add("snombre", OracleDbType.Varchar2).Direction = ParameterDirection.Input;
                cmd.Parameters.Add("papellido", OracleDbType.Varchar2).Direction = ParameterDirection.Input;
                cmd.Parameters.Add("sapellido", OracleDbType.Varchar2).Direction = ParameterDirection.Input;
                cmd.Parameters.Add("correo", OracleDbType.Varchar2).Direction = ParameterDirection.Input;
                cmd.Parameters.Add("saved", OracleDbType.Int32).Direction = ParameterDirection.Output;

                foreach (Asistente asistente in asistentes)
                {
                    cmd.Parameters["reserva_id"].Value = id;
                    cmd.Parameters["pasaporte"].Value = asistente.pasaporte;
                    cmd.Parameters["numrut"].Value = asistente.numRut;
                    cmd.Parameters["dvrut"].Value = asistente.dvRut;
                    cmd.Parameters["pnombre"].Value = asistente.primerNombre;
                    cmd.Parameters["snombre"].Value = asistente.segundoNombre;
                    cmd.Parameters["papellido"].Value = asistente.primerApellido;
                    cmd.Parameters["sapellido"].Value = asistente.segundoApellido;
                    cmd.Parameters["correo"].Value = asistente.correo;
                    OracleDataReader reader = cmd.ExecuteReader();
                    int saved = int.Parse(cmd.Parameters["saved"].Value.ToString());
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
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

        public List<Servicio> GetServicios(int id)
        {
            OracleCommand cmd = new OracleCommand("sp_obten_servicios_reserva", _context.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.BindByName = true;
            cmd.Parameters.Add("reserva_id", OracleDbType.Int32).Direction = ParameterDirection.Input;
            cmd.Parameters.Add("servicios", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            cmd.Parameters["reserva_id"].Value = id;
            OracleDataReader reader = cmd.ExecuteReader();

            List<Servicio> servicios = new List<Servicio>();
            while (reader.Read())
            {
                Servicio servicio = new Servicio();
                servicio.id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("id_res_serv")).ToString());
                servicio.idServicio = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("id_servicio")).ToString());
                servicio.servicio = reader.GetValue(reader.GetOrdinal("nombre_s")).ToString();
                servicio.tipo = reader.GetValue(reader.GetOrdinal("tipo_s")).ToString();
                servicio.valor = Convert.ToDouble(reader.GetValue(reader.GetOrdinal("valor_s")).ToString());
                servicio.conductor = Convert.ToInt32(GetIdConductor(reader.GetValue(reader.GetOrdinal("id_conductor")).ToString()));
                servicios.Add(servicio);
            }
            return servicios;
        }

        public string GetIdConductor(string conductor_id) => conductor_id.Equals(string.Empty) ? "0" : conductor_id;

        public List<Asistente> GetAsistentes(int id)
        {
            OracleCommand cmd = new OracleCommand("sp_obten_asistentes_reserva", _context.GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.BindByName = true;
            cmd.Parameters.Add("reserva_id", OracleDbType.Int32).Direction = ParameterDirection.Input;
            cmd.Parameters.Add("asistentes", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            cmd.Parameters["reserva_id"].Value = id;
            OracleDataReader reader = cmd.ExecuteReader();

            List<Asistente> asistentes = new List<Asistente>();
            while (reader.Read())
            {
                Asistente asistente = new Asistente();
                asistente.pasaporte = reader.GetValue(reader.GetOrdinal("pasaporte")).ToString();
                asistente.numRut = reader.GetValue(reader.GetOrdinal("numrut_asistente")).ToString();
                asistente.dvRut = reader.GetValue(reader.GetOrdinal("dvrut_asistente")).ToString();
                asistente.primerNombre = reader.GetValue(reader.GetOrdinal("pnombre_asistente")).ToString();
                asistente.segundoNombre = reader.GetValue(reader.GetOrdinal("snombre_asistente")).ToString();
                asistente.primerApellido = reader.GetValue(reader.GetOrdinal("apepat_asistente")).ToString();
                asistente.segundoApellido = reader.GetValue(reader.GetOrdinal("apemat_asistente")).ToString();
                asistente.correo = reader.GetValue(reader.GetOrdinal("correo_asistente")).ToString();
                asistentes.Add(asistente);
            }
            return asistentes;
        }

    }
}
