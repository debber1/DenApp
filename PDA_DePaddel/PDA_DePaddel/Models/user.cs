using System;
using System.Collections.Generic;
using System.Text;

namespace PDA_DePaddel.Models
{
    public class user
    {
        public int ID { get; set; }
        public string Voornaam { get; set; }
        public string Naam { get; set; }

        public user() { }
        public user(string Voornaam, string Naam)
        {
            this.Voornaam = Voornaam;
            this.Naam = Naam;
        }

        public bool checkinformation()
        {
            if (!this.Voornaam.Equals("") && !this.Naam.Equals(""))
                return true;
            else
                return false;

        }
    }
}
