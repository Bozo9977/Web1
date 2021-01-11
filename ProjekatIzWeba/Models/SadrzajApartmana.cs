using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PW32_2016_WebProjekat.Models
{
    public class SadrzajApartmanaModel
    {
        public string Id { get; set; }
        public string Naziv { get; set; }

        public SadrzajApartmanaModel(string id, string naziv)
        {
            Id = id;
            Naziv = naziv;
        }
    }
    public class SadrzajApartmana
    {

        public string Id { get; set; }
        public List<string> Naziv { get; set; }

        public SadrzajApartmana(string id, string naziv)
        {

            Id = id;
            Naziv = new List<string>();
            
            string[] zapisi = naziv.Split(',','.'); 
            foreach(var item in zapisi)
            {
                Naziv.Add(item);
            }
        }

        

        public override string ToString()
        {
            return $"\n{Id}|{string.Join(",",Naziv.ToArray())}|";
        }
    }
}