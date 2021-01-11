using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PW32_2016_WebProjekat.Models
{
    public class Domacin: Korisnik
    {
        public List<Apartman> Izdajem { get; set; }
        public Domacin()
        {
            Uloga = Uloge.DOMACIN;
        }
        public Domacin(string ime, string prezime, string korisnickoIme, string lozinka, string pol):base(ime, prezime,korisnickoIme,lozinka,pol)
        {
            Uloga = Uloge.DOMACIN;
            Izdajem = new List<Apartman>();
        }

        public override string ToString()
        {
            return "\n"+base.ToString();
        }
    }
}