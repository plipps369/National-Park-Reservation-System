using Capstone.Models;

using System.Collections.Generic;
using System.Data.SqlClient;


namespace Capstone.DAL
{
    public interface ParkIDAO
    {
        IList<Park> GetParks();

        void ParkInfo(Park park);

        Park GetPark(SqlDataReader reader);
    }
}
