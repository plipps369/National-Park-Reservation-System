
using Capstone.DAL;
using Capstone.Models;
using System;
using Capstone.Helper;
using System.Collections.Generic;

namespace Home
{
    public class CLI
    {
        /// <summary>
        /// Creating the necessary objects for CLI to run.
        /// </summary>
        public IDAO _db = null;
        public ParkIDAO _park = null;
        public CLI(IDAO db, ParkIDAO park)
        {
            _db = db;
            _park = park;
        }

        //Starting the menu system on the main menu.
        public void ShowMain()
        {
            bool main = true;
            while (main == true)
            {

                Console.Clear();
                Console.WriteLine("\n ,,,,,,,,,,,,,,,,#@@@@,,,,,,,,");
                Console.WriteLine(" ,,,,,,,,,,,,,,,@@@@@@,,,,,,,,");
                Console.WriteLine(" ,,,,,,,,,,,,@@@@@@@@,,,,,,,,,");
                Console.WriteLine(" ,,,,,,,,,,,@@@@@@@@,,,,,,,,,,");
                Console.WriteLine(" ,,,,,,,,,@@@@@@@@@,,,,,,,,,,");
                Console.WriteLine(" ,,,,,,,,,@@@@@@@@@@@,,,,,,,,,");
                Console.WriteLine(" ,,,,,,,,@@@@@@@@@@,@@@@,,,,,,");
                Console.WriteLine(" ,,,,,,/@@@@@@@@@@,,,@@@@,,,,");
                Console.WriteLine(" ,,,,@@@@,@@@@@@@,,,,,,,,@,,,,");
                Console.WriteLine(" ,,,,,,,,,@@@@@@@@,,,,,,,,,,,,");
                Console.WriteLine(" ,,,,,,,,,@@@@@@@@@,,,,,,,,,,,");
                Console.WriteLine(" ,,,,,,,,,@@@/@@@@@@,,,,,,,,,,");
                Console.WriteLine(" ,,,,,,,,@@@@,,,@@@@,,,,,,,,,,");
                Console.WriteLine(" ,,,,,,,@@@/,,,,,@@@,,,,,,,,,,");
                Console.WriteLine(" ,,,,,,@@@,,,,,,,@@@,,,,,,,,,,");
                Console.WriteLine(" ,,,,@@@@@@@,,,,,&@@@@,,,,,,,,\n\n");

                int select = CLIHelper.GetSingleInteger(
                    " Welcome to the National Park Service Campground Reservation System" +
                    "\n1.List Parks\n2. Show Reservation History\n3. Exit", 1, 3);

                if (select == 1)
                {
                    ParkList();
                    ShowSubMenu();
                }
                else if (select == 2)
                {
                    _db.ShowReservations();
                }
                else
                { 
                   main = false;
                }
            }
        }

        public void ShowSubMenu()
        {
            var list = _park.GetParks();
            int choice = CLIHelper.GetInteger("", 1, 4);
            if (choice != 4)
            {
                bool subMenu = true;
                
                Park park = list[choice - 1];
                _park.ParkInfo(park);
                while (subMenu == true)
                {
                    int reservationSelect = CLIHelper.GetInteger
                    ("Press 1 To Select View CampGrounds\nPress Any Other Number to Return to The Main Menu");
                    if (reservationSelect == 1)
                    {
                        ShowReservationMenu(park.ParkID);
                        subMenu = false;
                    }
                    else 
                    {
                        subMenu = false;
                    }
                    
                }
            }
            else { };
        }

        public void ShowReservations()
        {
            var name = CLIHelper.GetString("Please Provide Your Name");
            var resList = _db.ReservationHistory(name);
            if (resList.Count != 0)
            {
                Console.WriteLine("\n   " + "ID".PadRight(5) + "Number".PadRight(13) +
                "Name".PadRight(25) + "FromDate".PadRight(16) + "ToDate".PadRight(15));

                foreach (Reservation item in resList)
                {
                    Console.WriteLine("   " + item.ReservationID.ToString().PadRight(8) +
                                              item.SiteID.ToString().PadRight(10) +
                                              item.Name.ToString().PadRight(25) +
                                              item.FromDate.ToShortDateString().PadRight(16) +
                                              item.ToDate.ToShortDateString().PadRight(15));
                                                              
                }
                int done = CLIHelper.GetInteger("Press Any Number To Return To Main Menu");
            }
        }

        public void ShowReservationMenu(int ID)
        {
            Console.Clear();
            List<Campground> campList = _db.GetCampGrounds(ID);
            ShowCampGrounds(campList);
            
            string Name;
            DateTime FromDate;
            DateTime ToDate;
            DateTime CreateDate;
            

            int makeResInt = CLIHelper.GetInteger("\n\nIf You Would Like To Make A Reservation, " +
            "Select A Campground \nOtherwise Press 0 To Return To The Previous Menu.", 0, campList.Count);

            if (makeResInt != 0)
            {
                Console.Clear();
                Console.WriteLine("We Will First Ask For Some Basic Information About Your Stay,"
                                 + " Then Show  Our Matching Campsites.");

                Name = CLIHelper.GetString("What Name Would You Like To Make The Reservation Under?");
                Console.Clear();
                FromDate = CLIHelper.GetDateTime("When Does Your Reservation Begin?");
                Console.Clear();
                ToDate = CLIHelper.GetDateTime("When Does Your Reservation End?");
                CreateDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                Reservation New = _db.MakeReservation(ID, Name, FromDate, ToDate);
                TimeSpan Duration = ToDate - FromDate;
                decimal price = Duration.Days * campList[makeResInt - 1].DailyFeeMoney;
                ShowCampSites(makeResInt, price);
                var menuNumbers = _db.GetCampSites(makeResInt);
                var reserve = CLIHelper.GetInteger("Select a Campsite by Code-Number to Make a Reservation\n Press 0 to Exit", 0, menuNumbers.Count);
                if (reserve != 0)
                {
                    var newReservation = _db.MakeReservation(menuNumbers[reserve - 1].SiteID, Name, FromDate, ToDate);
                    int confirm = _db.ReservationInsert(newReservation);
                    if (confirm != 0)
                    {
                        Console.WriteLine($"Your Confirmation Number is {newReservation.ReservationID}");
                        Console.WriteLine("Press Any Key To Continue");
                        Console.ReadKey();
                    }
                    else;
                }
                else;
            }
            else;
           
        }

        public void ShowCampGrounds(List<Campground> list)
        {
            if (list.Count > 0)
            {
                Console.WriteLine("\n   "+"ID".PadRight(5) +
                                  "NAME".PadRight(40) +
                                  "Open    Close     DailyFee");

                foreach (Campground item in list)
                {
                    _db.AddDictionary(item.CampgroundID, item.DailyFeeMoney);
                    Console.WriteLine("   " + item.CampgroundID.ToString().PadRight(5) +
                                              item.Name.ToString().PadRight(42) +
                                              item.OpenMonth.ToString().PadRight(8) +
                                              item.CloseMonth.ToString().PadRight(8) +
                                              "$" + item.DailyFee.ToString());
                }
            }
            else
            {
                Console.WriteLine("There Are No Campgrounds In The Selected Park, Please Select A Different One.");
            }
        }

        public void ShowCampSites(int campgroundID, decimal price)
        {
            var loop = _db.GetCampSites(campgroundID);
            Console.Clear();
            Console.WriteLine(" Code".PadRight(8) +
                                  "Number".PadRight(10) +
                                  "Occupancy".PadRight(12) +
                                  "WCAccessible".PadRight(19) +
                                  "MaxRV Length".PadRight(13) +
                                  "Utilities".PadRight(12) +
                                  "Total Price".PadRight(5)

                  );
            foreach (Site item in loop)
            {
                int i = 1;
                Console.WriteLine("   "+i.ToString().PadRight(5) +
                    "  "+item.SiteNumber.ToString().PadRight(10) +
                    "  "+item.Occupancy.ToString().PadRight(12)+
                    ConvertHash[item.WCAccessible.ToString()].PadRight(18) +
                    item.MaxRvLength.ToString()+"FT".PadRight(19)+
                    ConvertHash[item.Utilities.ToString()].PadRight(9).PadLeft(5)+
                    "   $" + price
                    );
                i++;
            }
            
        }

        public void ShowParks()
        {
            var list = _park.GetParks();
        }

        public void ParkList()
        {
            Console.Clear();
            var list = _park.GetParks();
            foreach (Park item in list)
            {
                Console.WriteLine("   " + item.ParkID.ToString().PadRight(5) + item.Name.ToString().PadRight(20));
            }
            Console.WriteLine("\nSelect A Park To View Info");
            Console.WriteLine("Press 4 To Return To The Main Menu");


        }

        public Dictionary<string, string> ConvertHash = new Dictionary<string, string>
        {
            {"True","yes"},
            {"False","no" }
        };
    }
}


