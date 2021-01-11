using PW32_2016_WebProjekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using System.IO;
using ProjekatIzWeba.Models;
using System.Globalization;

namespace ProjekatIzWeba.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        [Route("api/login/LogIn")]
        public string LogIn(Korisnik log)
        {
            string putanjaBloka = HostingEnvironment.MapPath("~/Files/Blokirani.txt");
            string[] blokirani = File.ReadAllLines(putanjaBloka);

            for(int i =0; i < blokirani.Length; i++)
            {
                if (blokirani[i] == log.KorisnickoIme)
                    return "Privremeno ste blokirani.";
            }

            string path = HostingEnvironment.MapPath("~/Files/Korisnici.txt");
            if (!File.Exists(path))
            {
                return "Login.html";
            }
            else
            {
                Korisnik korisnik = KorisnikGetter.GetKorisnik(log.KorisnickoIme, log.Lozinka);



                if (korisnik == null)
                {
                    return "Login.html";
                }
                else if (korisnik.Uloga == Uloge.GOST)
                {
                    return "Gost.html";
                }
                else if (korisnik.Uloga == Uloge.ADMINISTRATOR)
                {
                    return "Admin.html";
                }
                else
                {
                    return "Domacin.html";
                }
            }
        }



        [HttpPost]
        [Route("api/login/PretraziApartmane")]
        public List<Apartman> PretraziApartmane(ApartmanPretragaModel zaPretragu)
        {
            List<Apartman> ret = ApartmanGetter.GetAktivneApartmane();

            if (!string.IsNullOrWhiteSpace(zaPretragu.Grad) && !string.IsNullOrEmpty(zaPretragu.Grad))
                ret = ret.FindAll(x => x.Lokacija.Adresa.NaseljenoMesto == zaPretragu.Grad);

            if (zaPretragu.BrSobaOD != 0)
                ret = ret.FindAll(x => x.BrSoba >= zaPretragu.BrSobaOD);

            if (zaPretragu.BrSobaDO != 0)
                ret = ret.FindAll(x => x.BrSoba <= zaPretragu.BrSobaDO);

            if (zaPretragu.BrOsobaOD != 0)
                ret = ret.FindAll(x => x.BrGostiju >= zaPretragu.BrOsobaOD);

            if (!string.IsNullOrWhiteSpace(zaPretragu.Dolazak) && !string.IsNullOrWhiteSpace(zaPretragu.Odlazak))
            {
                DateTime dolazak = DateTime.ParseExact(zaPretragu.Dolazak, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                //ret = ret.FindAll(x => x.DatumiZaIzdavanje.Contains(dolazak));

                DateTime odlazak = DateTime.ParseExact(zaPretragu.Odlazak, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                //ret = ret.FindAll(x => x.DatumiZaIzdavanje.Contains(dolazak));

                int brojDana = (int)(odlazak - dolazak).TotalDays;

                if (brojDana > 0)
                {
                    for (int i = 0; i < brojDana; i++)
                    {
                        DateTime pom = dolazak.AddDays(i);
                        ret = ret.FindAll(x => x.DatumiZaIzdavanje.Contains(pom));
                    }
                }
                else
                {
                    ret = new List<Apartman>();
                }


            }
            return ret;

        }



    }
}
