using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PW32_2016_WebProjekat.Models
{
    public class ApartmanModel
    {

        

        #region Polja
        public TipApartmana Tip { get; set; }
        public string Naziv { get; set; }
        public int BrSoba { get; set; }
        public int BrGostiju { get; set; }
        public LokacijaModel Lokacija { get; set; }
        public string Domacin { get; set; }
        public int Prijava { get; set; }
        public int Odjava { get; set; }
        public double CenaPoNocenju { get; set; }
        public StatusApartmana Status { get; set; }
        #endregion

        #region ListeApartmana
        public List<Komentar> Komentari { get; set; }
        public List<string> Slike { get; set; }
        public List<string> DatumiZaIzdavanje { get; set; }
        public List<DateTime> DostupniDatumi { get; set; }
        public List<SadrzajApartmana> SadrzajApartmana { get; set; }
        public List<Rezervacija> Rezervacije { get; set; }
        #endregion

    }
}