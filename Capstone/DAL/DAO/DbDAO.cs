
using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.Models
{
   
    public class DbDAO : IDAO
    {
        Dictionary<int, decimal> priceDictionary = new Dictionary<int, decimal>();
        private const string getLastIdSQL = "SELECT CAST(SCOPE_IDENTITY() as int);";
        int parkIdNumber = 0;
        
        private string dbConnectionString;
        public DbDAO(string connectionString)
        {
            dbConnectionString = connectionString;
        }

        #region Parks
        public IList<Park> ListParks()
        {
            string input = "";
            List<Park> result = new List<Park>();
            const string sql = "SELECT * FROM park;";
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
            if (result.Count > 0)
            {
                foreach (Park item in result)
                {
                    Console.WriteLine("   " + item.ParkID.ToString().PadRight(5) + item.Name.ToString().PadRight(20));
                }
                Console.WriteLine("  \nSelect A Park To View More Details");
                input = Console.ReadLine();
                parkIdNumber = int.Parse(input);
            }
            try
            {
                parkInfo(int.Parse(input) - 1);
            }
            catch
            {
                throw new Exception();   //fix this later
            }

            return result;
            void parkInfo(int selection)
            {
                Console.Clear();
                Console.WriteLine("   ParkID".PadRight(20) + result[selection].ParkID);
                Console.WriteLine("   Name".PadRight(20) + result[selection].Name);
                Console.WriteLine("   Location".PadRight(20) + result[selection].Location);
                Console.WriteLine("   Established".PadRight(20) + result[selection].EstablishYear.ToShortDateString());
                Console.WriteLine("   Area".PadRight(20) + result[selection].Area);
                Console.WriteLine("   Annual Visitors".PadRight(20) + result[selection].Visitors);
                Console.WriteLine("\n \n " + "    " + result[selection].Description + "\n \n");
            }
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
        #endregion

        #region Campgrounds
        public Campground GetCampgroundFromReader(SqlDataReader reader)
        {
            Campground item = new Campground();
            item.CampgroundID = Convert.ToInt32(reader["campground_id"]);
            item.ParkID = Convert.ToInt32(reader["park_id"]);
            item.Name = Convert.ToString(reader["name"]);
            item.OpenMonth = Convert.ToInt32(reader["open_from_mm"]);
            item.CloseMonth = Convert.ToInt32(reader["open_to_mm"]);
            item.DailyFee = Convert.ToInt32(reader["daily_fee"]);

            return item;
        }

        public List<Campground> GetCampGrounds(int ID)
        {
            List<Campground> result = new List<Campground>();
            const string sql = "SELECT * FROM campground where park_id =";
            using (SqlConnection conn = new SqlConnection(dbConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql + $"{ID}", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(GetCampgroundFromReader(reader));
                }
            }
            return result;
        }

        public void AddDictionary(int key, decimal value)
        {
            //Work here
        }
        public decimal SetPrice(string choice)
        {
           return priceDictionary[int.Parse(choice)];
        }
        #endregion

        #region Sites
        public Site GetSiteFromReader(SqlDataReader reader)
        {
            Site item = new Site();
            item.SiteID = Convert.ToInt32(reader["site_id"]);
            item.CampgroundID = Convert.ToInt32(reader["campground_id"]);
            item.SiteNumber = Convert.ToInt32(reader["site_number"]);
            item.Occupancy = Convert.ToInt32(reader["max_occupancy"]);
            item.WCAccessible = Convert.ToBoolean(reader["accessible"]);
            item.MaxRvLength = Convert.ToInt32(reader["max_rv_length"]);
            item.Utilities = Convert.ToBoolean(reader["utilities"]);

            return item;
        }

        public List<Site> GetCampSites(int campgroundID)
        {
            List<Site> result = new List<Site>();
            const string sql = "SELECT * FROM site where campground_id =";
            using (SqlConnection conn = new SqlConnection(dbConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql + $"{campgroundID}", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(GetSiteFromReader(reader));
                }
            }
            return result;
        }
        
        #endregion

        #region Reservations
        public Reservation MakeReservation(int siteID, string name, DateTime fromDate, DateTime toDate)
        {
            Reservation newReservation = new Reservation()
            {
                SiteID = siteID,
                Name = name,
                FromDate = fromDate,
                ToDate = toDate,
                CreateDate = DateTime.Now
            };
           
            return newReservation;
        }

        public int ReservationInsert(Reservation newReservation)
        {
            const string sql = "INSERT into reservation(site_id, name, from_date, to_date, create_date) " +
                               "VALUES (@site, @name, @from_date, @to_date, @create_date);";
            using (SqlConnection conn = new SqlConnection(dbConnectionString))
            {
                conn.Open();

                // Parameretize query
                SqlCommand cmd = new SqlCommand(sql + getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@site", newReservation.SiteID);
                cmd.Parameters.AddWithValue("@name", newReservation.Name);
                cmd.Parameters.AddWithValue("@from_date", newReservation.FromDate);
                cmd.Parameters.AddWithValue("@to_date", newReservation.ToDate);
                cmd.Parameters.AddWithValue("@create_date", newReservation.CreateDate);

                // Execute SQL command
                newReservation.ReservationID = (int)cmd.ExecuteScalar();
            }

            return newReservation.ReservationID;
            
        }
       #endregion

        #region DateTime
        public DateTime GetDateTime(string message)
        {
            string userInput = String.Empty;
            DateTime dateValue = DateTime.MinValue;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid date format. Please try again");
                }
                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!DateTime.TryParse(userInput, out dateValue));

            return dateValue;
        }
        
        public Reservation GetReservationFromReader(SqlDataReader reader)
        {
            Reservation item = new Reservation();
            item.ReservationID = Convert.ToInt32(reader["reservation_id"]);
            item.SiteID = Convert.ToInt32(reader["site_id"]);
            item.Name = Convert.ToString(reader["name"]);
            item.FromDate = Convert.ToDateTime(reader["from_date"]);
            item.ToDate = Convert.ToDateTime(reader["to_date"]);
            item.CreateDate = Convert.ToDateTime(reader["create_date"]);
            
            return item;
        }

        public List<Reservation> ReservationHistory(string Name)
        {
            List<Reservation> result = new List<Reservation>();
            const string sql = "SELECT * FROM reservation where name =";
            using (SqlConnection conn = new SqlConnection(dbConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql + $"'{Name}'", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(GetReservationFromReader(reader));
                }
            }
            
            return result;
        }
        


        #endregion

        public void GetDates(Reservation reservation, Site site, string choice)
        {
            const string sql = "Select Top 5 * From [site] " +
                                "Where site_id Not In(Select site.site_id From [site] " +
                                "Join reservation On reservation.site_id = site.site_id " +
                                "Where (Not (reservation.to_date < @from_date Or reservation.from_date > @to_date))) " +
                                "And campground_id = @CampgroundId " +
                                "And max_occupancy >= @Occupancy " +
                                "And accessible >= @IsAccessible " +
                                "And max_rv_length >= @RvLength " +
                                "And utilities >= @HasUtilities " +
                                "Order By site_number;";
            using (SqlConnection conn = new SqlConnection(dbConnectionString))
            {
                conn.Open();

                // Parameretize query
                SqlCommand cmd = new SqlCommand(sql + getLastIdSQL, conn);
                cmd.Parameters.AddWithValue("@CampgroundId", choice);
                cmd.Parameters.AddWithValue("@Occupancy", site.Occupancy);
                cmd.Parameters.AddWithValue("@from_date", reservation.FromDate);
                cmd.Parameters.AddWithValue("@to_date", reservation.ToDate);
                cmd.Parameters.AddWithValue("@IsAccessible", site.WCAccessible);
                cmd.Parameters.AddWithValue("@RvLength", site.MaxRvLength);
                cmd.Parameters.AddWithValue("@HasUtilities", site.Utilities);
                
             
            }
           
        }
    }
}

