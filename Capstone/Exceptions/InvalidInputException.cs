using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Exceptions
{
    class InvalidInputException : Exception
    {
        public string Messege()
        {
            return "Your Input Is Invalid";
        }
    }
}
