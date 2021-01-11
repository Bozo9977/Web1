using PW32_2016_WebProjekat.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace ProjekatIzWeba.Models
{
    public class KomentariGetter
    {
        private static readonly string path = HostingEnvironment.MapPath("~/Files/Komentari.txt");

        public static List<Komentar> GetKomentare(string apartman)
        {
            string[] lines = File.ReadAllLines(path);
            Komentar pom = null;
            List<Komentar> ret = new List<Komentar>();
            foreach(var line in lines)
            {
                if(string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                else
                {
                    if(line.Split('|')[1] == apartman)
                    {
                        pom = new Komentar(int.Parse(line.Split('|')[0]),line.Split('|')[2], line.Split('|')[1], line.Split('|')[4], int.Parse(line.Split('|')[3]));
                        ret.Add(pom);
                    }
                    else
                    {
                        continue;
                    }
                    
                }
            }
            return ret;
        }

        public static List<Komentar> GetKomentareZaCitanje(string apartman)
        {
            string[] lines = File.ReadAllLines(path);
            Komentar pom = null;
            List<Komentar> ret = new List<Komentar>();
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                else
                {
                    if(line.Split('|')[1]==apartman && line.Split('|')[5] == "True")
                    {
                        pom = new Komentar(int.Parse(line.Split('|')[0]), line.Split('|')[2], line.Split('|')[1], line.Split('|')[4], int.Parse(line.Split('|')[3]));
                        ret.Add(pom);
                    }
                        
                }
            }
            return ret;
        }

        public static int GetBrojLinije(string id)
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

                    if (apartman[0] == id)
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
    }
}