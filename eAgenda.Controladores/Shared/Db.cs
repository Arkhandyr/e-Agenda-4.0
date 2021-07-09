using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.SQLite;

namespace eAgenda.Controladores.Shared
{
    public delegate T ConverterDelegate<T>(IDataReader reader);

    public static class Db
    {
        private static readonly string connectionString = "";
        private static readonly string bancoUtilizado = "";

        static Db()
        {
            bancoUtilizado = ConfigurationManager.AppSettings["DB"].ToLower().Trim();
            connectionString = ConfigurationManager.ConnectionStrings[bancoUtilizado].ConnectionString;
        }

        public static int Insert(string sql, Dictionary<string, object> parameters)
        {
            if (bancoUtilizado.Equals("dbeagenda"))
            {
                SqlConnection connection = new SqlConnection(connectionString);

                SqlCommand command = new SqlCommand(sql.AppendSelectIdentity(), connection);

                command.SetParametersSQL(parameters);

                connection.Open();

                int id = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();

                return id;
            }
            else if (bancoUtilizado.Equals("dbeagendasqlite"))
            {
                SQLiteConnection connection = new SQLiteConnection(connectionString);

                SQLiteCommand command = new SQLiteCommand(sql.AppendSelectIdentitySQLite(), connection);

                command.SetParametersSQLite(parameters);

                connection.Open();

                int id = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();

                return id;
            }

            return -1;
        }

        public static void Update(string sql, Dictionary<string, object> parameters = null)
        {
            if (bancoUtilizado.Equals("dbeagenda"))
            {
                SqlConnection connection = new SqlConnection(connectionString);

                SqlCommand command = new SqlCommand(sql, connection);

                command.SetParametersSQL(parameters);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
            else if (bancoUtilizado.Equals("dbeagendasqlite"))
            {
                SQLiteConnection connection = new SQLiteConnection(connectionString);

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                command.SetParametersSQLite(parameters);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static void Delete(string sql, Dictionary<string, object> parameters)
        {
            Update(sql, parameters);
        }

        public static List<T> GetAll<T>(string sql, ConverterDelegate<T> convert, Dictionary<string, object> parameters = null)
        {
            if (bancoUtilizado.Equals("dbeagenda"))
            {
                SqlConnection connection = new SqlConnection(connectionString);

                SqlCommand command = new SqlCommand(sql, connection);

                command.SetParametersSQL(parameters);

                connection.Open();

                var list = new List<T>();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var obj = convert(reader);
                    list.Add(obj);
                }

                connection.Close();
                return list;
            }

            else if (bancoUtilizado.Equals("dbeagendasqlite"))
            {
                SQLiteConnection connection = new SQLiteConnection(connectionString);

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                command.SetParametersSQLite(parameters);

                connection.Open();

                var list = new List<T>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = convert(reader);
                        list.Add(obj);
                    }
                }

                connection.Close();
                return list;
            }

            return null;
        }

        public static T Get<T>(string sql, ConverterDelegate<T> convert, Dictionary<string, object> parameters)
        {
            if (bancoUtilizado.Equals("dbeagenda"))
            {
                SqlConnection connection = new SqlConnection(connectionString);

                SqlCommand command = new SqlCommand(sql, connection);

                command.SetParametersSQL(parameters);

                connection.Open();

                T t = default;

                var reader = command.ExecuteReader();

                if (reader.Read())
                    t = convert(reader);

                connection.Close();
                return t;
            }
            else if(bancoUtilizado.Equals("dbeagendasqlite"))
            {
                SQLiteConnection connection = new SQLiteConnection(connectionString);

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                command.SetParametersSQLite(parameters);

                connection.Open();

                T t = default;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        t = convert(reader);
                }

                connection.Close();
                return t;
            }
            return default;
        }

        public static bool Exists(string sql, Dictionary<string, object> parameters)
        {
            if (bancoUtilizado.Equals("dbeagenda"))
            {
                SqlConnection connection = new SqlConnection(connectionString);

                SqlCommand command = new SqlCommand(sql, connection);

                command.SetParametersSQL(parameters);

                connection.Open();

                int numberRows = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();

                return numberRows > 0;
            }
            else
            {
                SQLiteConnection connection = new SQLiteConnection(connectionString);

                SQLiteCommand command = new SQLiteCommand(sql, connection);

                command.SetParametersSQLite(parameters);

                connection.Open();

                int numberRows = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();

                return numberRows > 0;
            }
        }

        private static void SetParametersSQL(this SqlCommand command, Dictionary<string, object> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return;

            foreach (var parameter in parameters)
            {
                string name = parameter.Key;

                object value = parameter.Value.IsNullOrEmpty() ? DBNull.Value : parameter.Value;

                SqlParameter dbParameter = new SqlParameter(name, value);

                command.Parameters.Add(dbParameter);
            }
        }

        private static void SetParametersSQLite(this SQLiteCommand command, Dictionary<string, object> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return;

            foreach (var parameter in parameters)
            {
                string name = parameter.Key;

                object value = parameter.Value.IsNullOrEmpty() ? DBNull.Value : parameter.Value;

                SQLiteParameter dbParameter = new SQLiteParameter(name, value);

                command.Parameters.Add(dbParameter);
            }
        }

        private static string AppendSelectIdentity(this string sql)
        {
            return sql + ";SELECT SCOPE_IDENTITY()";
        }

        private static string AppendSelectIdentitySQLite(this string sql)
        {
            return sql + ";SELECT last_insert_rowid()";
        }

        public static bool IsNullOrEmpty(this object value)
        {
            return (value is string && string.IsNullOrEmpty((string)value)) ||
                    value == null;
        }

    }
}
