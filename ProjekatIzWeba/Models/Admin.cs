using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PW32_2016_WebProjekat.Models
{
    public class Admin: Korisnik
    {
        public Admin(string ime, string prezime, string korisnickoIme, string lozinka, string pol): base(ime,prezime,korisnickoIme,lozinka,pol)
        {
            this.Uloga = Uloge.ADMINISTRATOR;
        }

        public override string ToString()
        {
            return $"\n{base.ToString()}";
        }
    }
   
}