using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace PW32_2016_WebProjekat.Models
{
    public class KorisnikFileWriter
    {
        private static readonly string path = HostingEnvironment.MapPath("~/Files/Korisnici.txt");
        public static void Zapisi(int brLinije, Korisnik korisnik)
        {
            string[] lines = File.ReadAllLines(path);
            lines[brLinije] = "";
            lines[brLinije] = korisnik.ToString();

            File.WriteAllLines(path, lines);
        }

        public static void DodajKorisnika(Korisnik korisnik)
        {
            if (string.IsNullOrEmpty(File.ReadAllText(path)) || string.IsNullOrWhiteSpace(File.ReadAllText(path)))
            {
                File.WriteAllText(path, korisnik.ToString());
            }
            else
            {
                File.AppendAllText(path, $"\n{korisnik.ToString()}");
            }
        }
    }
}