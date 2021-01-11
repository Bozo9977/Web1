using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PW32_2016_WebProjekat.Models
{
    public class Lokacija
    {
        public double GeografskaSirina { get; set; }
        public double GeografskaDuzina { get; set; }
        public Adresa Adresa { get; set; }

        public Lokacija()
        {

        }

        public Lokacija(double gDuzina, double gSirina, Adresa adresa)
        {
            GeografskaDuzina = gDuzina;
            GeografskaSirina = gSirina;
            Adresa = adresa;
        }

        public override string ToString()
        {
            return $"{GeografskaDuzina}|{GeografskaSirina}|{Adresa.ToString()}";
        }
    }
}