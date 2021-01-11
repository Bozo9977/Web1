using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PW32_2016_WebProjekat.Models
{
    public class Gost: Korisnik
    {
        public List<Apartman> IznajmljeniApartmani { get; set; }
        public List<Rezervacija> Rezervacije { get; set; }
        public Gost():base()
        {
            Uloga = Uloge.GOST;
            IznajmljeniApartmani = new List<Apartman>();
            Rezervacije = new List<Rezervacija>();
        }
        public Gost(string ime, string prezime, string korisnickoIme, string lozinka, string pol) : base(ime, prezime, korisnickoIme, lozinka, pol)
        {
            Uloga = Uloge.GOST;
            IznajmljeniApartmani = new List<Apartman>();
            Rezervacije = new List<Rezervacija>();
        }

        public override string ToString()
        {
            return $"\n{base.ToString()}";
        }

       
    }
}