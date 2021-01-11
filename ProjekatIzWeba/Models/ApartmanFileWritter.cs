using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace PW32_2016_WebProjekat.Models
{
    public class ApartmanFileWritter
    {
        public static readonly string path = HostingEnvironment.MapPath("~/Files/Apartmani.txt");

        public static void ZapisiApartman(Apartman a)
        {
            if (string.IsNullOrEmpty(File.ReadAllText(path)) || string.IsNullOrWhiteSpace(File.ReadAllText(path)))
            {
                File.WriteAllText(path, a.ToString());
            }
            else
            {
                File.AppendAllText(path, $"\n{a.ToString()}");
            }
        }

        public static void ZapisiIzmenjen(int brLinije, Apartman apartman)
        {
            string[] lines = File.ReadAllLines(path);
            lines[brLinije] = "";
            lines[brLinije] = apartman.ToString();

            File.WriteAllLines(path, lines);
        }

        public static void ObrisiApartman(int brLinije)
        {
            string[] lines = File.ReadAllLines(path);
            lines[brLinije] += "OBRISAN";

            File.WriteAllLines(path, lines);
        }


        public static void IzmeniKorisnickoIme(string stariK, string noviK)
        {
            string[] lines = File.ReadAllLines(path);
            string[] pom = new string[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                if (!string.IsNullOrEmpty(lines[i]) && !string.IsNullOrWhiteSpace(lines[i]))
                {
                    if (lines[i].Split('|')[4] == stariK)
                    {
                        string[] splitovano = lines[i].Split('|');
                        string promena = "";
                        for (int j = 0; j < splitovano.Length; j++)
                        {
                            if (j == 4)
                                promena += $"{noviK}|";
                            else
                            {
                                promena += $"{splitovano[j]}|";
                            }
                        }

                        pom[i] = promena;
                    }
                    else
                    {
                        pom[i] = lines[i];
                    }
                }
                else
                {
                    continue;
                }

            }
            File.WriteAllLines(path, pom);
        }
    }
}