using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace PW32_2016_WebProjekat.Models
{
    public class SadrzajApartmanaFileWritter
    {
        private static readonly string path = HostingEnvironment.MapPath("~/Files/SadrzajiApartmana.txt");

        public static void ZapisiSadrzaj(SadrzajApartmana sadrzaj)
        {
                File.AppendAllText(path, sadrzaj.ToString());
        }

        public static SadrzajApartmana GetSadrzajById(string id)
        {
            string[] lines = File.ReadAllLines(path);
            SadrzajApartmana ret = null;

            foreach(var line in lines)
            {
                if(string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                if (id == line.Split('|')[0])
                {
                    if(line.Split('|')[2] != "OBRISAN")
                        ret = new SadrzajApartmana(line.Split('|')[0], line.Split('|')[1]);
                }
            }

            return ret;
        }

        public static List<SadrzajApartmanaModel> GetSavSadrzaj()
        {
            List<SadrzajApartmanaModel> ret = new List<SadrzajApartmanaModel>();
            string[] lines = File.ReadAllLines(path);
            SadrzajApartmanaModel sam = null;

            foreach (var line in lines)
            {
                if(string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                else
                {
                    if(line.Split('|')[2] != "OBRISAN")
                    {
                        sam = new SadrzajApartmanaModel(line.Split('|')[0], line.Split('|')[1]);
                        ret.Add(sam);
                    }
                    
                }
            }

            return ret;
        }

        public static void ZapisiIzmenjenSadrzaj(string idStarog, SadrzajApartmana sadrzaj)
        {
            string[] lines = File.ReadAllLines(path);
            for(int i =0;i<lines.Length; i++)
            {
                if(lines[i].Split('|')[0] == idStarog)
                {
                    var pom = sadrzaj.ToString();
                    lines[i] = pom;
                }
            }

            File.WriteAllLines(path, lines);
        }

        public static void ObrisiSadrzaj(string id)
        {
            string[] lines = File.ReadAllLines(path);
            
            for(int i =0; i < lines.Length; i++)
            {
                if(lines[i].Split('|')[0] == id)
                {
                    lines[i] += "OBRISAN";
                }
            }

            File.WriteAllLines(path, lines);
        }



    }
}