using ProjekatIzWeba.Models;
using PW32_2016_WebProjekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Globalization;
using System.Web.Http;
using System.IO;

namespace ProjekatIzWeba.Controllers
{
    public class GostController : ApiController
    {
        private static string username;
        private readonly string path = HostingEnvironment.MapPath("~/Files/Korisnici.txt");


        [HttpPost]
        [Route("api/gost/GetData")]
        public Korisnik GetData(Korisnik korisnik)
        {
            Korisnik gost = KorisnikGetter.GetKorisnik(korisnik.KorisnickoIme, korisnik.Lozinka);
            username = gost.KorisnickoIme;
            return gost;
        }

        [HttpPost]
        [Route("api/gost/Izmeni")]
        public IHttpActionResult Izmeni(Gost korisnik)
        {
            if (username == korisnik.KorisnickoIme || KorisnikGetter.GetKorisnikByIme(korisnik.KorisnickoIme) == null)
            {
                Korisnik gost = KorisnikGetter.GetKorisnikByIme(username);
                int brLinije = KorisnikGetter.GetBrojLinije(username);
                gost.Ime = korisnik.Ime;
                gost.Prezime = korisnik.Prezime;
                gost.KorisnickoIme = korisnik.KorisnickoIme;
                gost.Lozinka = korisnik.Lozinka;
                gost.Pol = korisnik.Pol;
                
                KorisnikFileWriter.Zapisi(brLinije, gost);
                RezervacijaFileWritter.IzmeniKorisnika(username, gost.KorisnickoIme);
                KomentarFileWritter.IzmeniKorisnickoIme(username, gost.KorisnickoIme);
                username = korisnik.KorisnickoIme;
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/gost/PrikaziAktivneApartmane")]
        public List<Apartman> PrikaziAktivneApartmane()
        {
            List<Apartman> apartmani = ApartmanGetter.GetAktivneApartmane();
            string putanjaBloka = HostingEnvironment.MapPath("~/Files/Blokirani.txt");
            List<string> blokirani = File.ReadAllLines(putanjaBloka).ToList();

            foreach(var item in blokirani)
            {
                apartmani.RemoveAll(x => x.Domacin == item);
            }

            return apartmani;
        }

        [HttpGet]
        [Route("api/gost/PrikaziSortiraneApartmane")]
        public List<Apartman> PrikaziSortiraneApartmane(string Poredak)
        {
            List<Apartman> pom = ApartmanGetter.GetAktivneApartmane();
            List<Apartman> ret = new List<Apartman>();
            if (Poredak == "Opadajuci")
                ret = pom.OrderByDescending(x => x.CenaPoNocenju).ToList();
            else
                ret = pom.OrderBy(x => x.CenaPoNocenju).ToList();

            return ret;
        }

        [HttpGet]
        [Route("api/gost/GetFiltriraneApartmane")]
        public List<Apartman> GetFiltriraneApartmane(string Filter)
        {
            List<Apartman> pom = ApartmanGetter.GetAktivneApartmane();
            List<Apartman> ret = new List<Apartman>();

            if (Filter == "CEO")
                ret = pom.FindAll(x => x.Tip == TipApartmana.CEO);
            else if (Filter == "SOBA")
                ret = pom.FindAll(x => x.Tip == TipApartmana.SOBA);
            else
            {
                foreach (var item in pom)
                {
                    if (item.SadrzajApartmana.SingleOrDefault(x => x.Id == Filter) != null)
                        ret.Add(item);
                }
            }

            return ret;
        }

        [HttpGet]
        [Route("api/gost/DobaviSortiraneRezervacije")]
        public List<Rezervacija> DobaviSortiraneRezervacije(string PoredakR, string KorisnickoIme)
        {
            List<Rezervacija> pom = RezervacijaGetter.GetRezervacije(KorisnickoIme);
            List<Rezervacija> ret = new List<Rezervacija>();
            if (PoredakR == "Opadajuci")
                ret = pom.OrderByDescending(x => x.UkupnaCena).ToList();
            else
                ret = pom.OrderBy(x => x.UkupnaCena).ToList();
            return ret;
        }

        [HttpPost]
        [Route("api/gost/DobaviSadrzaj")]
        public List<SadrzajApartmana> DobaviSadrzaj(Apartman apartman)
        {
            return ApartmanGetter.GetApartmanByNaziv(apartman.Naziv).SadrzajApartmana;
        }


        [HttpPost]
        [Route("api/gost/DobaviDatume")]
        public List<string> DobaviDatume(ApartmanModel apartman)
        {
            List<string> datumi = ApartmanGetter.GetDatumeApartmana(apartman.Naziv);
            return datumi;
        }

        [HttpPost]
        [Route("api/gost/NapraviRezervaciju")]
        public string NapraviRezervaciju(RezervacijaModel rezervacija)
        {
            DateTime rezDate = DateTime.ParseExact(rezervacija.PocetniDatum, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Rezervacija rez = new Rezervacija(rezervacija);
            int broj = Rezervacija.GetBrojObjekta();
            rez.Id = broj + 1;
            Apartman apartman = ApartmanGetter.GetApartmanByNazivIStatus(rezervacija.RezervisaniApartman, "AKTIVAN");

            DateTime pomocni = rezDate.Date;
            



            for(int i=0; i < rezervacija.BrojNocenja; i++)
            {
                if (!apartman.DatumiZaIzdavanje.Contains(rezDate.Date.AddDays(i)))
                    return "Apartman nije u ponudi za odabrani period.";
            }

            //prepis cene

            rez.UkupnaCena = 0;
            string putanjaDatumi = HostingEnvironment.MapPath("~/Files/Praznici.txt");
            string datumi = File.ReadAllText(putanjaDatumi);
            string[] prazniciPom = datumi.Split(',');
            List<DateTime> praznici = new List<DateTime>();
            foreach(var datum in prazniciPom)
            {
                praznici.Add(DateTime.ParseExact(datum, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            }

            for (int i =0; i<rezervacija.BrojNocenja; i++)
            {
                pomocni = rezDate.AddDays(i);
                if(pomocni.DayOfWeek == DayOfWeek.Saturday || pomocni.DayOfWeek == DayOfWeek.Sunday || pomocni.DayOfWeek==DayOfWeek.Friday)
                {
                    rez.UkupnaCena += 0.9 * apartman.CenaPoNocenju;
                    continue;
                }
                else if (praznici.Contains(pomocni))
                {
                    rez.UkupnaCena += 1.05 * apartman.CenaPoNocenju;
                }
                else
                {
                    rez.UkupnaCena += apartman.CenaPoNocenju;
                }
            }

            RezervacijaFileWritter.UpisiRezervaciju(rez);
            return "zapisano";
        }

        [HttpPost]
        [Route("api/gost/DobaviRezervacije")]
        public List<Rezervacija> DobaviRezervacije(Korisnik gost)
        {
            return RezervacijaGetter.GetRezervacije(gost.KorisnickoIme);
        }

        [HttpPost]
        [Route("api/gost/RezervacijaOdustani")]
        public string RezervacijaOdustani(Rezervacija rez)
        {
            if(RezervacijaGetter.GetRezervacijaById(rez.Id) != null)
            {
                Rezervacija zapis = RezervacijaGetter.GetRezervacijaById(rez.Id);
                zapis.Status = StatusRezervacije.ODUSTANAK;
                int linija = RezervacijaFileWritter.GetBrojLinije(zapis.Id);
                RezervacijaFileWritter.ZapisiIzmenjen(linija, zapis);
                return "Odustanak potvrdjen.";
            }
            else
            {
                return "Nismo uspeli da nadjemo vasu rezervaciju.";
            }
        }

        [HttpPost]
        [Route("api/gost/OstaviKomentar")]
        public string OstaviKomentar(Komentar komentar)
        {
            if (KomentarFileWritter.ProveriStatus(komentar.PostavioGost))
            {
                int broj = Komentar.GetBrojObjekta();
                komentar.Id = broj + 1;
                KomentarFileWritter.OstaviKomentar(komentar);
                return "Komentar memorisan.";
            }
            else
            {
                return "Trenutno vam nije dozvoljeno da ostavljate komentare.";
            }
            
        }


        [HttpPost]
        [Route("api/gost/DobaviApartman")]
        public Apartman DobaviApartman(Apartman apartmanNaziv)
        {
            Apartman ret = ApartmanGetter.GetApartmanByNaziv(apartmanNaziv.Naziv);
            //apartmanZaIzmenu = apartmanNaziv.Naziv;
            List<Komentar> komentari = KomentariGetter.GetKomentareZaCitanje(apartmanNaziv.Naziv);
            ret.Komentari = komentari;
            return ret;
        }
    }
}
