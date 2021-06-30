// using System.IO;
// using System.Reflection;
// using NLog;
// using Npgsql;
//
// namespace PageObject.Services
// {
//     public static class SqlService
//     {
//         private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
//         
//         private static NpgsqlConnection _connection;
//         private static NpgsqlCommand _cmd;
//
//         public static void OpenConnection()
//         {
//             if (_connection != null) return;
//             _connection = new NpgsqlConnection(Configurator.ConnectionString);
//             _connection.Open();
//             Logger.Info("ServerVersion: {0}", _connection.ServerVersion);
//             Logger.Info("State: {0}", _connection.State);
//         }
//         
//         public static void CloseConnection()
//         {
//             _connection.Close();
//             Logger.Info("State: {0}", _connection.State);
//         }
//
//         public static NpgsqlDataReader ExecuteSql(string sql)
//         {
//             using var cmd = new NpgsqlCommand(sql, _connection);
//             return cmd.ExecuteReader();
//         }
//         
//         public static void DropTable(string tableName)
//         {
//             var sql = $"drop table if exists {tableName} cascade;";
//             using var cmd = new NpgsqlCommand(sql, _connection);
//             cmd.ExecuteNonQuery();
//         }
//
//         public static void CreateModelTable(string model)
//         {
//             var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
//             var file = Path.Combine(basePath, "TestData", "SQL", $"{model}.sql");
//             var sql = File.ReadAllText(file);
//             using var cmd = new NpgsqlCommand(sql, _connection);
//             cmd.ExecuteNonQuery();
//         }
//     }
// }