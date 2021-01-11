using PW32_2016_WebProjekat.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace ProjekatIzWeba.Models
{
    public class RezervacijaFileWritter
    {
        private static readonly string path = HostingEnvironment.MapPath("~/Files/Rezervacije.txt");

        public static void UpisiRezervaciju(Rezervacija rez)
        {
            if (string.IsNullOrEmpty(File.ReadAllText(path)) || string.IsNullOrWhiteSpace(File.ReadAllText(path)))
            {
                File.WriteAllText(path, rez.ToString());
            }
            else
            {
                File.AppendAllText(path, $"\n{rez.ToString()}");
            }
        }

        public static void ZapisiIzmenjen(int brLinije, Rezervacija rez)
        {
            string[] lines = File.ReadAllLines(path);
            lines[brLinije] = "";
            lines[brLinije] = rez.ToString();

            File.WriteAllLines(path, lines);
        }

        public static int GetBrojLinije(int id)
        {
            int ret = 0;


            foreach (var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(line) || string.IsNullOrEmpty(line))
                {
                    ret++;
                    continue;
                }
                else
                {
                    string[] rez = line.Split('|');

                    if (int.Parse(rez[0]) == id)
                    {
                        break;
                    }
                    else
                    {
                        ret++;
                        continue;
                    }

                }
            }
            return ret;
        }

        public static void IzmeniNazivApartmana(string naziv, string izmenjenNaziv)
        {
            string[] lines = File.ReadAllLines(path);
            string[] pom = new string[lines.Length];

            for(int i=0; i< lines.Length; i++)
            {
                if(!string.IsNullOrEmpty(lines[i]) && !string.IsNullOrWhiteSpace(lines[i]))
                {
                    if (lines[i].Split('|')[1] == naziv)
                    {
                        string[] splitovano = lines[i].Split('|');
                        string promena = "";
                        for (int j = 0; j < splitovano.Length; j++)
                        {
                            if (j == 1)
                                promena += $"{izmenjenNaziv}|";
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

        public static void IzmeniKorisnika(string stariK, string noviK)
        {
            string[] lines = File.ReadAllLines(path);
            string[] pom = new string[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                if (!string.IsNullOrEmpty(lines[i]) && !string.IsNullOrWhiteSpace(lines[i]))
                {
                    if (lines[i].Split('|')[2] == stariK)
                    {
                        string[] splitovano = lines[i].Split('|');
                        string promena = "";
                        for (int j = 0; j < splitovano.Length; j++)
                        {
                            if (j == 2)
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