using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjekatIzWeba.Models
{
    public class ApartmanPretragaModel
    {
        public string Dolazak { get; set; }
        public string Odlazak { get; set; }
        public string Grad { get; set; }
        public int CenaOD { get; set; }
        public int CenaDO { get; set; }
        public int BrSobaOD { get; set; }
        public int BrSobaDO { get; set; }
        public int BrOsobaOD { get; set; }

    }
}