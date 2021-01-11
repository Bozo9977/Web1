using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace PW32_2016_WebProjekat.Models
{
    public class KorisnikGetter
    {
        private static readonly string path = HostingEnvironment.MapPath("~/Files/Korisnici.txt");

        public static Korisnik GetKorisnik(string username, string password)
        {
            Korisnik ret = null;
            string path = HostingEnvironment.MapPath("~/Files/Korisnici.txt");

            foreach (var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(line) || string.IsNullOrEmpty(line))
                {
                    continue;
                }
                else
                {
                    string[] korisnik = line.Split('|');

                    if (korisnik[2] == username && korisnik[3] == password)
                    {
                        if (korisnik[5] == "GOST")
                        {
                            ret = new Gost(korisnik[0], korisnik[1], korisnik[2], korisnik[3], korisnik[4]);
                        }else if (korisnik[5] == "DOMACIN")
                        {
                            ret = new Domacin(korisnik[0], korisnik[1], korisnik[2], korisnik[3], korisnik[4]);
                        }
                        else
                        {
                            ret = new Admin(korisnik[0], korisnik[1], korisnik[2], korisnik[3], korisnik[4]);
                        }
                    }
                    else
                    {
                        continue;
                    }

                }
            }
            return ret;
        }

        public static Korisnik GetKorisnikByIme(string username)
        {
            Korisnik ret = null;
            string path = HostingEnvironment.MapPath("~/Files/Korisnici.txt");

            foreach (var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(line) || string.IsNullOrEmpty(line))
                {
                    continue;
                }
                else
                {
                    string[] korisnik = line.Split('|');

                    if (korisnik[2] == username)
                    {
                        if(korisnik[5].ToLower() == "administrator")
                        {
                            ret = new Admin(korisnik[0], korisnik[1], korisnik[2], korisnik[3], korisnik[4]);
                        }else if(korisnik[5].ToLower() == "gost")
                        {
                            ret = new Gost(korisnik[0], korisnik[1], korisnik[2], korisnik[3], korisnik[4]);
                        }
                        else
                        {
                            ret = new Domacin(korisnik[0], korisnik[1], korisnik[2], korisnik[3], korisnik[4]);
                        }
                            
                        
                    }
                    else
                    {
                        continue;
                    }

                }
            }
            return ret;
        }

        public static int GetBrojLinije(string username)
        {
            int ret = 0;
            string path = HostingEnvironment.MapPath("~/Files/Korisnici.txt");

            foreach (var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(line) || string.IsNullOrEmpty(line))
                {
                    ret++;
                    continue;
                }
                else
                {
                    
                    string[] korisnik = line.Split('|');

                    if (korisnik[2] == username)
                    {
                        
                        if (korisnik[5] == "GOST")
                        {
                            break;
                        }
                        else
                        {
                            continue;
                        }
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

        public static List<KorisnikModel> GetKorisnike()
        {
            List<KorisnikModel> ret = new List<KorisnikModel>();
            string[] lines = File.ReadAllLines(path);
            foreach(var line in lines)
            {
                if(string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                else
                {
                    KorisnikModel km = new KorisnikModel(KorisnikGetter.GetKorisnikByIme(line.Split('|')[2]));
                    ret.Add(km);
                }
            }

            return ret;
        }

        //public static List<KorisnikModel> GetKorisnikeDomacina(string korisnickoIme)
        //{

        //}



    }
}