using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public interface IDAO
    {

        Campground GetCampgroundFromReader(SqlDataReader reader);

        List<Campground> GetCampGrounds(int ID);

        void AddDictionary(int key, decimal value);

        List<Site> GetCampSites(int campgroundID);

        Reservation MakeReservation(int SiteID, string Name, DateTime StartDate, DateTime EndDate);

        int ReservationInsert(Reservation newReservation);

        decimal SetPrice(string choice);

        DateTime GetDateTime(string message);

        List<Reservation> ReservationHistory(string search);      

    }
}
