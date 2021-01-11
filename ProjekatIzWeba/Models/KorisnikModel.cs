using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PW32_2016_WebProjekat.Models
{
    public class KorisnikModel
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string KorisnickoIme { get; set; }
        public string Pol { get; set; }
        public string Uloga { get; set; }
        public KorisnikModel()
        {

        }

        public KorisnikModel(Korisnik korisnik)
        {
            Ime = korisnik.Ime;
            Prezime = korisnik.Prezime;
            KorisnickoIme = korisnik.KorisnickoIme;
            Pol = korisnik.Pol.ToString();
            Uloga = korisnik.Uloga.ToString();
        }
    }
}