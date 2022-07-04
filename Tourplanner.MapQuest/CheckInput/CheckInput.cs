using System.Text.RegularExpressions;
using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public class CheckInput : ICheckInput
    {
        private bool CheckUserInput(string input)
        {
            if (Regex.Match(input, "^([a-zA-Z0-9-,]|\\s)*$").Success)
                return true;
            return false;
        }

        private bool CheckUserInputWithSymbols(string input)
        {
            if (Regex.Match(input, "^([a-zA-Z0-9,.!€$?-]|\\s)*$").Success)
                return true;
            return false;
        }

        public bool CheckUserInputTour(string name, string description, string from, string to, TransportType transportType)
        {
            if (name == null || !CheckUserInput(name)) throw new ArgumentException("Name is invalid");
            if (name == null || !CheckUserInputWithSymbols(description)) throw new ArgumentException("Description is invalid");
            if (name == null || !CheckUserInput(from)) throw new ArgumentException("Starting location is invalid");
            if (name == null || !CheckUserInput(to)) throw new ArgumentException("Ending location is invalid");

            return true;
        }

        public bool CheckUserInputLog(string comment)
        {
            if (comment == null || !CheckUserInputWithSymbols(comment)) throw new ArgumentException("Comment is invalid");

            return true;
        }
    }
}
