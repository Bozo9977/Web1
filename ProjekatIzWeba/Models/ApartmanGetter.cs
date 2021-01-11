using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace PW32_2016_WebProjekat.Models
{
    public class ApartmanGetter
    {
        private static readonly string path = HostingEnvironment.MapPath("~/Files/Apartmani.txt");

        public static Apartman GetApartmanByNaziv(string naziv)
        {
            Apartman a = null;
            string[] lines = File.ReadAllLines(path);
            foreach(var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || string.IsNullOrEmpty(line))
                {
                    continue;
                }
                else
                {
                    string[] s = line.Split('|');
                    if (s[0] == naziv)
                    {
                        if (s[18] != "OBRISAN")
                        {
                            List<SadrzajApartmana> sadrzaj = ApartmanGetter.GetSadrzajApartmana(s[15]);
                            List<DateTime> datumi = ApartmanGetter.GetDatume(s[16]);
                            List<string> slike = ApartmanGetter.GetSlikeApartmana(s[17]);
                            a = new Apartman(s[0], s[1], int.Parse(s[2]), int.Parse(s[3]), s[4], int.Parse(s[5]), int.Parse(s[6]), double.Parse(s[7]), new Lokacija(double.Parse(s[8]), double.Parse(s[9]), new Adresa(s[10], s[11], int.Parse(s[12]), int.Parse(s[13]))), s[14], sadrzaj, datumi, slike);
                        }
                        
                    }
                    else
                    {
                        continue;
                    }
                }
                
            }

            return a;
        }

        private static List<string> GetSlikeApartmana(string slikeSastavljene)
        {
            string[] slike = slikeSastavljene.Split(',');
            return slike.ToList();
        }

        public static List<string> GetDatumeApartmana(string nazivApartmana)
        {
            List<string> datumi = new List<string>();

            CultureInfo parsiraj = CultureInfo.InvariantCulture;

            string[] lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || string.IsNullOrEmpty(line))
                {
                    continue;
                }
                else
                {
                    string[] s = line.Split('|');
                    if (s[0] == nazivApartmana)
                    {
                        //if(s[18]!= "OBRISAN")
                        //{
                            string[] datumiPom = s[16].Split(',');
                            foreach (var item in datumiPom)
                            {
                                DateTime datum = DateTime.ParseExact(item, "yyyy-MM-dd", parsiraj);
                                datumi.Add(datum.ToString("yyyy/MM/dd"));
                            }
                        //}
                        
                    }
                    else
                    {
                        continue;
                    }
                }

            }

            return datumi;
        }
        public static List<Apartman> GetApartmane()
        {
            List<Apartman> ret = new List<Apartman>();
            string[] lines = File.ReadAllLines(path);
            foreach(var line in lines)
            {
                if(string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                else
                {
                    ret.Add(ApartmanGetter.GetApartmanByNaziv(line.Split('|')[0]));
                }
            }
            for(int i =0; i< ret.Count; i++)
            {
                if (ret[i] == null)
                    ret.Remove(ret[i]);
            }

            return ret;
        }

        private static List<SadrzajApartmana> GetSadrzajApartmana(string idevi)
        {
            List<SadrzajApartmana> ret = new List<SadrzajApartmana>();
            SadrzajApartmana pom;
            string putanjaSadrzaj = HostingEnvironment.MapPath("~/Files/SadrzajiApartmana.txt");

            string[] sadrzaji = File.ReadAllLines(putanjaSadrzaj);

            string[] ids = idevi.Split(',');
            foreach(var item in ids)
            {
                for(int i = 0; i < sadrzaji.Length; i++)
                {
                    if(item == sadrzaji[i].Split('|')[0])
                    {
                        pom = new SadrzajApartmana(item, sadrzaji[i].Split('|')[1]);
                        ret.Add(pom);
                    }
                }
            }

            return ret;
        }

        private static List<DateTime> GetDatume(string datumi)
        {
            string[] datumiPom = datumi.Split(',');
            List<DateTime> ret = new List<DateTime>();
            foreach(var item in datumiPom)
            {
                ret.Add(DateTime.ParseExact(item, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            }

            return ret;
        }
        public static int GetBrojLinije(string apartmanNaziv)
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
                    string[] apartman = line.Split('|');

                    if (apartman[0] == apartmanNaziv)
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


        public static List<Apartman> GetAktivneApartmane()
        {
            List<Apartman> ret = new List<Apartman>();
            string[] lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                else
                {
                    Apartman a = ApartmanGetter.GetApartmanByNazivIStatus(line.Split('|')[0], "AKTIVAN");
                    if(a != null)
                    {
                        ret.Add(a);
                    }
                    //ret.Add(ApartmanGetter.GetApartmanByNazivIStatus(line.Split('|')[0], "AKTIVAN"));
                }
            }
            return ret;
        }

        public static Apartman GetApartmanByNazivIStatus(string naziv, string status)
        {
            Apartman a = null;
            string[] lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || string.IsNullOrEmpty(line))
                {
                    continue;
                }
                else
                {
                    string[] s = line.Split('|');
                    if (s[0] == naziv && s[14]==status)
                    {
                        if(s[18] != "OBRISAN")
                        {
                            List<SadrzajApartmana> prosledi = ApartmanGetter.GetSadrzajApartmana(s[15]);
                            List<DateTime> datumi = ApartmanGetter.GetDatume(s[16]);
                            List<string> slike = ApartmanGetter.GetSlikeApartmana(s[17]);
                            a = new Apartman(s[0], s[1], int.Parse(s[2]), int.Parse(s[3]), s[4], int.Parse(s[5]), int.Parse(s[6]), double.Parse(s[7]), new Lokacija(double.Parse(s[8]), double.Parse(s[9]), new Adresa(s[10], s[11], int.Parse(s[12]), int.Parse(s[13]))), s[14], prosledi, datumi, slike);
                        }
                        
                    }
                    else
                    {
                        continue;
                    }
                }

            }

            return a;
        }

        public static List<Apartman> GetApartmanByKorisnickoIme(string KorisnickoIme)
        {
            List<Apartman> ret = new List<Apartman>();
            string[] lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                else
                {
                    if(line.Split('|')[4] == KorisnickoIme)
                        ret.Add(ApartmanGetter.GetApartmanByNaziv(line.Split('|')[0]));
                }
            }

            for(int i = 0; i < ret.Count; i++)
            {
                if (ret[i] == null)
                    ret.Remove(ret[i]);
            }

            return ret;
        }
        
    }
}