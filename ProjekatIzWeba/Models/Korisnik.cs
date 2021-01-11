using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PW32_2016_WebProjekat.Models
{
    public class Korisnik
    {

        

        public string KorisnickoIme { get; set; }
        
        public string Lozinka { get; set; }
        
        public string Ime { get; set; }
        
        public string Prezime { get; set; }
        
        public Polovi Pol { get; set; }
        
        public Uloge Uloga { get; set; }

        public Korisnik()
        {

        }

        public Korisnik(string ime, string prezime, string korisnickoIme, string lozinka, string pol)
        {
            KorisnickoIme = korisnickoIme;
            Ime = ime;
            Prezime = prezime;
            Lozinka = lozinka;
            if (pol.ToLower() == "zenski")
                Pol = Polovi.ZENSKI;
            else
                Pol = Polovi.MUSKI;
            //Uloga = Uloge.GOST;
        }

        public override string ToString()
        {
            return $"{Ime}|{Prezime}|{KorisnickoIme}|{Lozinka}|{Pol.ToString()}|{Uloga.ToString()}";
        }
    }
}