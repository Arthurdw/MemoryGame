using MySql.Data.MySqlClient;
using System;

namespace Memory_Game
{
    public class MySqlClient
    {
        public string Server { get; private set; }
        public string Port { get; private set; }
        public string Database { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }

        public MySqlClient(string userName, string password, string database, string server, string port)
        {
            this.UserName = userName;
            this.Password = password;
            this.Database = database;
            this.Server = server;
            this.Port = port;
        }

        public static MySqlClient From(string userName, string password, string database, string server)
            => new MySqlClient(userName, password, database, server, "3306");
    }

    public class MySqlHandler
    {
        public readonly MySqlConnection Connection;

        public MySqlHandler(MySqlClient client, string sslMode = "None")
        {
            string connectionString = $"Server={client.Server};" +
                                    $"Port={client.Port};" +
                                    $"SslMode={sslMode};" +
                                    $"Database={client.Database};" +
                                    $"Uid={client.UserName};" +
                                    $"Pwd={client.Password};";

            this.Connection = new MySqlConnection(connectionString);
        }

        private T PerformSqlAction<T>(Func<T> executable)
        {
            this.Connection.Open();
            var returnValue = executable();
            this.Connection.Close();
            return returnValue;
        }

        public MySqlCommand Prepare(string sqlStatement, params (string, object)[] parameters)
        {
            MySqlCommand command = new MySqlCommand(sqlStatement, this.Connection);

            foreach (var (parameterName, value) in parameters)
                command.Parameters.AddWithValue(parameterName, value);

            return command;
        }

        public int Execute(MySqlCommand command)
            => this.PerformSqlAction(command.ExecuteNonQuery);
    }
}