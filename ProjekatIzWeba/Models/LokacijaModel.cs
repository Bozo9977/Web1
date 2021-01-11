using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PW32_2016_WebProjekat.Models
{
    public class LokacijaModel
    {
        public string GeografskaSirina { get; set; }
        public string GeografskaDuzina { get; set; }
        public Adresa Adresa { get; set; }

    }
}