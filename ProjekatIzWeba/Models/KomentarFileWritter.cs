using PW32_2016_WebProjekat.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace ProjekatIzWeba.Models
{
    public class KomentarFileWritter
    {
        private static readonly string path = HostingEnvironment.MapPath("~/Files/Komentari.txt");
        private static readonly string putanjaRezervacija = HostingEnvironment.MapPath("~/Files/Rezervacije.txt");


        public static void OstaviKomentar(Komentar komentar)
        {
            if (File.Exists(path))
            {
                File.AppendAllText(path, $"\n{komentar.ToString()}");
            }
            else
            {
                File.WriteAllText(path, $"{komentar.ToString()}");

            }
        }

        public static bool ProveriStatus(string username)
        {
            string[] lines = File.ReadAllLines(putanjaRezervacija);
            foreach(var line in lines)
            {
                if (line.Split('|')[2] == username)
                {
                    if (line.Split('|')[6] == "ODBIJENA" || line.Split('|')[6] == "ZAVRSENA")
                        return true;
                }
            }

            return false;
        }

        public static void OmoguciCitanje(string id)
        {
            int brojLinije = KomentariGetter.GetBrojLinije(id);
            

            string[] lines = File.ReadAllLines(path);
            Komentar kom = new Komentar(int.Parse(id), lines[brojLinije].Split('|')[2], lines[brojLinije].Split('|')[1], lines[brojLinije].Split('|')[4], int.Parse(lines[brojLinije].Split('|')[3]))
            {
                ZaCitanje = true
            };

            lines[brojLinije] = "";
            lines[brojLinije] = kom.ToString();

            File.WriteAllLines(path, lines);
                
        }


        public static void IzmeniNazivApartmana(string naziv, string izmenjenNaziv)
        {
            string[] lines = File.ReadAllLines(path);
            string[] pom = new string[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                if (!string.IsNullOrEmpty(lines[i]) && !string.IsNullOrWhiteSpace(lines[i]))
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

        public static void IzmeniKorisnickoIme(string stariK, string noviK)
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