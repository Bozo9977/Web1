using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace PW32_2016_WebProjekat.Models
{
    public class Apartman
    {
        private static int id;
        public static int Id { get => id; set => id = value; }
        public TipApartmana Tip { get; set; }
        public string Naziv { get; set; }
        public int BrSoba { get; set; }
        public int BrGostiju { get; set; }
        public Lokacija Lokacija { get; set; }
        public string Domacin { get; set; }
        public int Prijava { get; set; }
        public int Odjava { get; set; }
        public double CenaPoNocenju { get; set; }
        public StatusApartmana Status { get; set; }


        #region ListeApartmana
        public List<Komentar> Komentari { get; set; }
        public List<string> Slike { get; set; }
        public List<DateTime> DatumiZaIzdavanje { get; set; }
        public List<DateTime> DostupniDatumi { get; set; }
        public List<SadrzajApartmana> SadrzajApartmana { get; set; }
        public List<Rezervacija> Rezervacije { get; set; }
        #endregion

        public Apartman(ApartmanModel a)
        {
            Lokacija = new Lokacija();
            Tip = a.Tip;
            Naziv = a.Naziv;
            BrSoba = a.BrSoba;
            BrGostiju = a.BrGostiju;
            Lokacija.GeografskaDuzina = double.Parse(a.Lokacija.GeografskaDuzina);
            Lokacija.GeografskaSirina = double.Parse(a.Lokacija.GeografskaSirina);
            Lokacija.Adresa = a.Lokacija.Adresa;
            Domacin = a.Domacin;
            Prijava = a.Prijava;
            Odjava = a.Odjava;
            CenaPoNocenju = a.CenaPoNocenju;
            Status = StatusApartmana.NEAKTIVAN;
            Komentari = a.Komentari;
            Slike = a.Slike;
            DatumiZaIzdavanje = KonvertujUDateTimeListu(a.DatumiZaIzdavanje);
            SadrzajApartmana = a.SadrzajApartmana;
            DostupniDatumi = DatumiZaIzdavanje;
            Rezervacije = a.Rezervacije;
        }

        private List<DateTime> KonvertujUDateTimeListu(List<String> datumi)
        {
            if (datumi.Count > 0 && datumi[0]!="")
                return datumi.Select(date => DateTime.Parse(date)).ToList();
            else
                return null;
        }

        public Apartman()
        {
            Status = StatusApartmana.NEAKTIVAN;
            SadrzajApartmana = new List<SadrzajApartmana>();
            Rezervacije = new List<Rezervacija>();
            Slike = new List<string>();
            DostupniDatumi = new List<DateTime>();
            DatumiZaIzdavanje = new List<DateTime>();
            Komentari = new List<Komentar>();
        }

        public Apartman(string naziv,string tip, int brojSoba, int brojGostiju, string domacinKorisnickoIme, int prijava, int odjava, double cenaPoNocenju, Lokacija lokacija, string status, List<SadrzajApartmana> sadrzaj, List<DateTime> datumi, List<string> slike)
        {
            SadrzajApartmana = sadrzaj;
            Slike = slike;
            Naziv = naziv;
            BrSoba = brojSoba;
            BrGostiju = brojGostiju;
            Domacin = domacinKorisnickoIme;
            DatumiZaIzdavanje = datumi;
            Prijava = prijava;
            Odjava = odjava;
            CenaPoNocenju = cenaPoNocenju;
            Lokacija = lokacija;
            Status = StatusApartmana.NEAKTIVAN;
            if (tip == "CEO")
            {
                Tip = TipApartmana.CEO;
            }
            else
            {
                Tip = TipApartmana.SOBA;
            }
            if (status == null)
            {
                Status = StatusApartmana.NEAKTIVAN;
            }
            else
            {
                if (status == "AKTIVAN")
                    Status = StatusApartmana.AKTIVAN;
                else
                    Status = StatusApartmana.NEAKTIVAN;
            }
        }

        private List<string> ListaIdSadrzaja()
        {
            List<string> ret = new List<string>();
            foreach(var item in SadrzajApartmana)
            {
                ret.Add(item.Id);
            }
            return ret;
        }

        public override string ToString()
        {
            return $"{Naziv}|{Tip.ToString()}|{BrSoba}|{BrGostiju}|{Domacin}|{Prijava}|{Odjava}|{CenaPoNocenju}|{Lokacija.ToString()}|{Status.ToString()}|{string.Join(",",ListaIdSadrzaja().ToArray())}|{ZapisDatuma(DatumiZaIzdavanje)}|{string.Join(",",Slike)}|";
        }

        private string ZapisDatuma(List<DateTime> datumi)
        {
            
            if (datumi != null)
            {
                List<string> pom = new List<string>();
                foreach(var item in datumi)
                {
                    //pom.Add(item.ToString("dd/MM/yyyy"));
                    pom.Add(item.ToString("yyyy/MM/dd"));
                }

                return string.Join(",", pom.ToArray());
            }
            else
            {
                return "";
            }
        }
    }
}