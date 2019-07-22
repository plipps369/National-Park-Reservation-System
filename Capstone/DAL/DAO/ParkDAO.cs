using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;


namespace Capstone.DAL
{
    public class ParkDAO : ParkIDAO
    {

        private string dbConnectionString;

        public ParkDAO(string connectionString)
        {
            dbConnectionString = connectionString;
        }

        public IList<Park> GetParks()
        {
            List<Park> result = new List<Park>();
            const string sql = "SELECT * FROM park;";
            #region SQLConnection
            using (SqlConnection conn = new SqlConnection(dbConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(GetPark(reader));
                }
            }
            #endregion
            return result;
        }

        public void ParkInfo(Park park)
        {
            Console.Clear();
            Console.WriteLine("   ParkID".PadRight(20) + park.ParkID);
            Console.WriteLine("   Name".PadRight(20) + park.Name);
            Console.WriteLine("   Location".PadRight(20) + park.Location);
            Console.WriteLine("   Established".PadRight(20) + park.EstablishYear.ToShortDateString());
            Console.WriteLine("   Area".PadRight(20) + park.Area);
            Console.WriteLine("   Annual Visitors".PadRight(20) + park.Visitors);
            Console.WriteLine("\n \n " + park.Description + "\n \n");
        }

        public Park GetPark(SqlDataReader reader)
        {
        Park item = new Park();
        item.ParkID = Convert.ToInt32(reader["park_id"]);
        item.Name = Convert.ToString(reader["name"]);
        item.Location = Convert.ToString(reader["location"]);
        item.EstablishYear = Convert.ToDateTime(reader["establish_date"]);
        item.Area = Convert.ToInt32(reader["area"]);
        item.Visitors = Convert.ToInt32(reader["visitors"]);
        item.Description = Convert.ToString(reader["description"]);

        return item;
        }
    }

    
}



