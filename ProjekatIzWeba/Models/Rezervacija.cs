using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace PW32_2016_WebProjekat.Models
{
    public class RezervacijaModel
    {
        public int Id { get; set; }
        public string RezervisaniApartman { get; set; }
        public string PocetniDatum { get; set; }
        public int BrojNocenja { get; set; }
        public double UkupnaCena { get; set; }
        public string Gost { get; set; }
        public StatusRezervacije Status { get; set; }
    }
    public class Rezervacija
    {
        public int Id { get; set; }
        public string RezervisaniApartman { get; set; }
        public DateTime PocetniDatum { get; set; }
        public int BrojNocenja { get; set; }
        public double UkupnaCena { get; set; }
        public string Gost { get; set; }
        public StatusRezervacije Status { get; set; }

        public Rezervacija()
        {

        }

        public Rezervacija(RezervacijaModel rm)
        {
            Id = rm.Id;
            Status = rm.Status;
            RezervisaniApartman = rm.RezervisaniApartman;
            PocetniDatum = DateTime.ParseExact(rm.PocetniDatum, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            BrojNocenja = rm.BrojNocenja;
            Gost = rm.Gost;
            UkupnaCena = BrojNocenja * ApartmanGetter.GetApartmanByNaziv(RezervisaniApartman).CenaPoNocenju;
        }
        public Rezervacija(int id,string apartman, DateTime pocetak, int brNocenja, string gost, string status)
        {
            Id = id;
            RezervisaniApartman = apartman;
            PocetniDatum = pocetak;
            BrojNocenja = brNocenja;
            Gost = gost;
            Status = StatusRezervacije.KREIRANA;
            if (status == "KREIRANA")
                Status = StatusRezervacije.KREIRANA;
            else if (status == "ODBIJENA")
                Status = StatusRezervacije.ODBIJENA;
            else if (status == "ODUSTANAK")
                Status = StatusRezervacije.ODUSTANAK;
            else if (status == "PRIHVACENA")
                Status = StatusRezervacije.PRIHVACENA;
            else if (status == "ZAVRSENA")
                Status = StatusRezervacije.ZAVRSENA;
            UkupnaCena = brNocenja * ApartmanGetter.GetApartmanByNaziv(apartman).CenaPoNocenju;
        }

        public Rezervacija(int id, string apartman, DateTime pocetak, int brNocenja, string gost, string status, double ukupnaCena)
        {
            Id = id;
            RezervisaniApartman = apartman;
            PocetniDatum = pocetak;
            BrojNocenja = brNocenja;
            Gost = gost;
            Status = StatusRezervacije.KREIRANA;
            if (status == "KREIRANA")
                Status = StatusRezervacije.KREIRANA;
            else if (status == "ODBIJENA")
                Status = StatusRezervacije.ODBIJENA;
            else if (status == "ODUSTANAK")
                Status = StatusRezervacije.ODUSTANAK;
            else if (status == "PRIHVACENA")
                Status = StatusRezervacije.PRIHVACENA;
            else if (status == "ZAVRSENA")
                Status = StatusRezervacije.ZAVRSENA;
            UkupnaCena = ukupnaCena;
        }

        public override string ToString()
        {
            return $"{Id}|{RezervisaniApartman}|{Gost}|{PocetniDatum.ToString("yyyy/MM/dd")}|{BrojNocenja}|{UkupnaCena}|{Status}";
        }

        private static readonly string path = HostingEnvironment.MapPath("~/Files/Rezervacije.txt");

        public static int GetBrojObjekta()
        {
            string[] lines = File.ReadAllLines(path);
            int broj = 0;
            foreach(var line in lines)
            {
                if(string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                else
                {
                    int.TryParse(line.Split('|')[0], out broj);
                }
            }

            if (broj == 0)
                broj = 1;

            return broj;
        }
    }
}