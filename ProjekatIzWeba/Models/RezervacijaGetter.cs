using PW32_2016_WebProjekat.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace ProjekatIzWeba.Models
{
    public class RezervacijaGetter
    {
        private static readonly string path = HostingEnvironment.MapPath("~/Files/Rezervacije.txt");

        public static List<Rezervacija> GetRezervacije(string username)
        {
            List<Rezervacija> rezervacijeKorisnika = new List<Rezervacija>();

            string[] lines = File.ReadAllLines(path);
            foreach(var line in lines)
            {
                if(string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if(line.Split('|')[2] == username)
                {
                    Rezervacija rez = new Rezervacija(int.Parse(line.Split('|')[0]), line.Split('|')[1], DateTime.ParseExact(line.Split('|')[3], "yyyy-MM-dd", CultureInfo.InvariantCulture), int.Parse(line.Split('|')[4]), line.Split('|')[2], line.Split('|')[6], double.Parse(line.Split('|')[5]));
                    rezervacijeKorisnika.Add(rez);
                }
            }
            for(int i = 0; i< rezervacijeKorisnika.Count; i++)
            {
                if (rezervacijeKorisnika[i] == null)
                    rezervacijeKorisnika.Remove(rezervacijeKorisnika[i]);
            }
            return rezervacijeKorisnika;
        }

        public static List<Rezervacija> GetRezervacijeDomacina(string apartman)
        {
            List<Rezervacija> rezervacijeKorisnika = new List<Rezervacija>();

            string[] lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (line.Split('|')[1] == apartman)
                {
                    Rezervacija rez = new Rezervacija(int.Parse(line.Split('|')[0]), line.Split('|')[1], DateTime.ParseExact(line.Split('|')[3], "yyyy-MM-dd", CultureInfo.InvariantCulture), int.Parse(line.Split('|')[4]), line.Split('|')[2], line.Split('|')[6], double.Parse(line.Split('|')[5]));
                    rezervacijeKorisnika.Add(rez);
                }
            }

            for(int i =0; i< rezervacijeKorisnika.Count; i++)
            {
                if (rezervacijeKorisnika[i] == null)
                    rezervacijeKorisnika.Remove(rezervacijeKorisnika[i]);
            }

            string putanjaBloka = HostingEnvironment.MapPath("~/Files/Blokirani.txt");
            List<string> blokirani = File.ReadAllLines(putanjaBloka).ToList();
            
            for(int i=0; i < rezervacijeKorisnika.Count; i++)
            {
                if (blokirani.Contains(rezervacijeKorisnika[i].Gost))
                    rezervacijeKorisnika.Remove(rezervacijeKorisnika[i]);
            }

            return rezervacijeKorisnika;
        }

        public static List<Rezervacija> GetRezervacijeAdmin()
        {
            List<Rezervacija> rezervacijeKorisnika = new List<Rezervacija>();
            List<Apartman> apartmani = ApartmanGetter.GetApartmane();
            for(int i =0; i < apartmani.Count; i++)
            {
                if (apartmani[i] == null)
                {
                    apartmani.Remove(apartmani[i]);
                }
            }

            string[] lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                else
                {
                    
                    Rezervacija rez = new Rezervacija(int.Parse(line.Split('|')[0]), line.Split('|')[1], DateTime.ParseExact(line.Split('|')[3], "yyyy-MM-dd", CultureInfo.InvariantCulture), int.Parse(line.Split('|')[4]), line.Split('|')[2], line.Split('|')[6], double.Parse(line.Split('|')[5]));
                    rezervacijeKorisnika.Add(rez);
                    
                }
                
            }

            return rezervacijeKorisnika;
        }

        public static Rezervacija GetRezervacijaById(int broj)
        {
            string[] lines = File.ReadAllLines(path);
            foreach(var line in lines)
            {
                if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                if (int.Parse(line.Split('|')[0]) == broj)
                {
                    return new Rezervacija(int.Parse(line.Split('|')[0]), line.Split('|')[1], DateTime.ParseExact(line.Split('|')[3], "yyyy-MM-dd", CultureInfo.InvariantCulture), int.Parse(line.Split('|')[4]), line.Split('|')[2], line.Split('|')[6], double.Parse(line.Split('|')[5]));
                }
            }

            return null;
        }

    }
}