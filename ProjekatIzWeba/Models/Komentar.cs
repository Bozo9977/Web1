using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace PW32_2016_WebProjekat.Models
{
    public class Komentar
    {
        public int Id { get; set; }
        public string PostavioGost { get; set; }
        public string ZaApartman { get; set; }
        public string Tekst { get; set; }
        public int Ocena { get; set; }
        public bool ZaCitanje { get; set; }

        public Komentar()
        {
                
        }

        public Komentar(int id, string gost, string apartman, string tekst, int ocena)
        {
            Id = id;
            PostavioGost = gost;
            ZaApartman = apartman;
            Tekst = tekst;
            Ocena = ocena;
            ZaCitanje = false;
        }

        public override string ToString()
        {
            return $"{Id}|{ZaApartman}|{PostavioGost}|{Ocena}|{Tekst}|{ZaCitanje}";
        }

        private static readonly string path = HostingEnvironment.MapPath("~/Files/Komentari.txt");
        public static int GetBrojObjekta()
        {
            string[] lines = File.ReadAllLines(path);
            int broj = 0;
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                else
                {
                    broj = int.Parse(line.Split('|')[0]);
                }
            }

            return broj;
        }
    }
}