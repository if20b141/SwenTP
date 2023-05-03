using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;

namespace DAL
{
    public class DataHandler
    {
        public static readonly DataHandler Instance = new DataHandler();

        private string connectionString;
        public DataHandler()
        {
            GetConnectionString(10);
        }
        public List<string> GetConnectionString(int maxSemaphore)
        {
            List<string> list = new List<string>();
            var connection = ConfigurationManager.AppSettings["ConnectionString"];
            list.Add(connection);


            //connectionString = ;
            semaphore = new SemaphoreSlim(maxSemaphore);
            return list;
        }

        public SemaphoreSlim semaphore;

       /* public void SetTestFunction()
        {
            semaphore.Wait();
            NpgsqlConnection newConnection = new NpgsqlConnection(connectionString);
            newConnection.Open();
            using(var Command = new NpgsqlCommand("INSERT into test(id, routeName, length) VALUES (@id, @name, @length)", newConnection))
            {
                Command.Parameters.AddWithValue("id", 2);
                Command.Parameters.AddWithValue("name", "Günther");
                Command.Parameters.AddWithValue("length", 24);

                Command.Prepare();
                Command.ExecuteNonQuery();
                newConnection.Close();
                semaphore.Release();
            }
        }
        public List<string> GetTestFunction()
        {
            List<string> list = new List<string>();
            semaphore.Wait();
            NpgsqlConnection newConnection = new NpgsqlConnection(connectionString);
            newConnection.Open();
            using (var Command = new NpgsqlCommand("SELECT * FROM test", newConnection))
            {
                NpgsqlDataReader dr = Command.ExecuteReader();
                if (dr.Read())
                {
                    list.Add(dr.GetString(1));
                }
                newConnection.Close();
                semaphore.Release();
                return list;
            }
        }
       */
    }
      
}
