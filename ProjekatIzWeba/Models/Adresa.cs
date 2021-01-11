using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PW32_2016_WebProjekat.Models
{
    public class Adresa
    {
        public int Broj { get; set; }
        public string Ulica { get; set; }
        public string NaseljenoMesto { get; set; }
        public int PostanskiBroj { get; set; }

        public Adresa()
        {

        }

        public Adresa(string naseljenoMesto, string ulica, int broj, int postanskiBroj)
        {
            NaseljenoMesto = naseljenoMesto;
            Ulica = ulica;
            Broj = broj;
            PostanskiBroj = postanskiBroj;
        }

        public override string ToString()
        {
            return $"{NaseljenoMesto}|{Ulica}|{Broj}|{PostanskiBroj}";
        }

    }
}