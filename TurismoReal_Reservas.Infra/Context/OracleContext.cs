using Oracle.ManagedDataAccess.Client;
using System;

namespace TurismoReal_Reservas.Infra.Context
{
    public class OracleContext
    {
        protected readonly string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
        private readonly OracleConnection connection;

        public OracleContext()
        {
            connection = new OracleConnection(connectionString);
        }

        public OracleConnection GetConnection()
        {
            return connection;
        }

        public void OpenConnection()
        {
            connection.Open();
        }

        public void CloseConnection()
        {
            connection.Close();
        }
    }
}
