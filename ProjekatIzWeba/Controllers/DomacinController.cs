using Newtonsoft.Json.Linq;
using ProjekatIzWeba.Models;
using PW32_2016_WebProjekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;

namespace ProjekatIzWeba.Controllers
{
    public class DomacinController : ApiController
    {
        private readonly string path = HostingEnvironment.MapPath("~/Files/Apartmani.txt");
        private static string username;
        private static string apartmanZaIzmenu;

        [HttpPost]
        [Route("api/domacin/GetData")]
        public Korisnik GetData(Korisnik korisnik)
        {
            Korisnik domacin = KorisnikGetter.GetKorisnikByIme(korisnik.KorisnickoIme);
            username = domacin.KorisnickoIme;
            return domacin;
        }

        [HttpPost]
        [Route("api/domacin/Izmeni")]
        public string Izmeni(Domacin domacin)
        {
            Korisnik k = KorisnikGetter.GetKorisnikByIme(username);
            int brLinije = KorisnikGetter.GetBrojLinije(username);
            if (username == domacin.KorisnickoIme || KorisnikGetter.GetKorisnikByIme(domacin.KorisnickoIme) == null)
            {
                
                k.Ime = domacin.Ime;
                k.Prezime = domacin.Prezime;
                k.KorisnickoIme = domacin.KorisnickoIme;
                k.Lozinka = domacin.Lozinka;
                k.Pol = domacin.Pol;
                KorisnikFileWriter.Zapisi(brLinije, k);
                //RezervacijaFileWritter.IzmeniKorisnika(username, domacin.KorisnickoIme);
                ApartmanFileWritter.IzmeniKorisnickoIme(username, k.KorisnickoIme);
                username = domacin.KorisnickoIme;
                return "Korisnik promenjen.";
            }
            else
            {
                return "Korisnik sa tim imenom vec postoji.";
            }


        }

        [HttpPost]
        [Route("api/domacin/dodajApartman")]
        public string DodajApartman(JObject a)
        {
            ApartmanModel apartmanM = null;
            apartmanM = a.ToObject<ApartmanModel>();

            Apartman apartman = new Apartman(apartmanM);

            if (ApartmanGetter.GetApartmanByNaziv(apartman.Naziv) != null)
            {
                return "Apartman sa tim nazivom vec postoji.";
            }
            else
            {
                ApartmanFileWritter.ZapisiApartman(apartman);

                return "Apartman uspesno dodat.";
            }
        }

        [HttpGet]
        [Route("api/domacin/DobaviSadrzaj")]
        public List<SadrzajApartmanaModel> DobaviSadrzaj()
        {
            return SadrzajApartmanaFileWritter.GetSavSadrzaj();
        }

        [HttpPost]
        [Route("api/domacin/DobaviRezervacije")]
        public List<Rezervacija> GetRezervacijeDomacina(Korisnik domacin)
        {
            List<Apartman> apartmaniDomacina = ApartmanGetter.GetApartmanByKorisnickoIme(domacin.KorisnickoIme);
            List<Rezervacija> ret = new List<Rezervacija>();

            

            for (int i = 0; i < apartmaniDomacina.Count; i++)
            {
                if (apartmaniDomacina[i] == null)
                    apartmaniDomacina.Remove(apartmaniDomacina[i]);
            }
            foreach(var item in apartmaniDomacina)
            {
                ret.AddRange(RezervacijaGetter.GetRezervacijeDomacina(item.Naziv));
            }

            return ret;
        }

        [HttpGet]
        [Route("api/domacin/PretraziRez")]
        public List<Rezervacija> PretraziRez(string ElementPretrage, string Domacin)
        {
            List<Apartman> apartmaniDomacina = ApartmanGetter.GetApartmanByKorisnickoIme(Domacin);
            List<Rezervacija> ret = new List<Rezervacija>();

            for(int i= 0; i<apartmaniDomacina.Count; i++)
            {
                if (apartmaniDomacina[i] == null)
                    apartmaniDomacina.Remove(apartmaniDomacina[i]);
            }

            foreach(var item in apartmaniDomacina)
            {
                ret.AddRange(RezervacijaGetter.GetRezervacijeDomacina(item.Naziv));
            }
            


            for (int i = 0; i < ret.Count; i++)
            {
                if (ret[i] == null)
                    ret.Remove(ret[i]);
            }

            //for(int j =0; j < ret.Count; j++)
            //{
            //    if (ret[j].Gost != ElementPretrage)
            //        ret.Remove(ret[j]);
            //    else
            //    {
            //        continue;
            //    }
            //}

            List<Rezervacija> pom = ret.FindAll(x => x.Gost == ElementPretrage);

            return pom;
        }


        [HttpGet]
        [Route("api/domacin/DobaviSortiraneRezervacije")]
        public List<Rezervacija> DobaviSortiraneRezervacije(string PoredakR, string KorisnickoIme)
        {
            List<Apartman> apartmaniDomacina = ApartmanGetter.GetApartmanByKorisnickoIme(KorisnickoIme);
            List<Rezervacija> pom = new List<Rezervacija>();
            List<Rezervacija> ret = new List<Rezervacija>();

            for (int i = 0; i < apartmaniDomacina.Count; i++)
            {
                if (apartmaniDomacina[i] == null)
                    apartmaniDomacina.Remove(apartmaniDomacina[i]);
            }
            foreach (var item in apartmaniDomacina)
            {
                pom.AddRange(RezervacijaGetter.GetRezervacijeDomacina(item.Naziv));
            }

            if (PoredakR == "Opadajuci")
                ret = pom.OrderByDescending(x => x.UkupnaCena).ToList();
            else
                ret = pom.OrderBy(x => x.UkupnaCena).ToList();
            return ret;
        }

        [HttpGet]
        [Route("api/domacin/DobaviFiltriraneRezervacije")]
        public List<Rezervacija> DobaviFiltriraneRezervacije(string Filter, string KorisnickoIme)
        {
            List<Apartman> apartmaniDomacina = ApartmanGetter.GetApartmanByKorisnickoIme(KorisnickoIme);
            List<Rezervacija> pom = new List<Rezervacija>();
            List<Rezervacija> ret = new List<Rezervacija>();

            foreach (var item in apartmaniDomacina)
            {
                pom.AddRange(RezervacijaGetter.GetRezervacijeDomacina(item.Naziv));
            }

            if (Filter == "KREIRANA")
                ret = pom.FindAll(x => x.Status == StatusRezervacije.KREIRANA);
            else if (Filter == "ODBIJENA")
                ret = pom.FindAll(x => x.Status == StatusRezervacije.ODBIJENA);
            else if (Filter == "ODUSTANAK")
                ret = pom.FindAll(x => x.Status == StatusRezervacije.ODUSTANAK);
            else if (Filter == "PRIHVACENA")
                ret = pom.FindAll(x => x.Status == StatusRezervacije.PRIHVACENA);
            else if (Filter == "ZAVRSENA")
                ret = pom.FindAll(x => x.Status == StatusRezervacije.ZAVRSENA);

            return ret;

        }

        [HttpPost]
        [Route("api/domacin/DobaviApartman")]
        public Apartman DobaviApartman(Apartman apartmanNaziv)
        {
            Apartman ret = ApartmanGetter.GetApartmanByNaziv(apartmanNaziv.Naziv);
            apartmanZaIzmenu = apartmanNaziv.Naziv;
            return ret;
        }

        [HttpPost]
        [Route("api/domacin/RezervacijaOdbi")]
        public string RezervacijaOdbi(Rezervacija rez)
        {
            if (RezervacijaGetter.GetRezervacijaById(rez.Id) != null)
            {
                Rezervacija zapis = RezervacijaGetter.GetRezervacijaById(rez.Id);
                zapis.Status = StatusRezervacije.ODBIJENA;
                int linija = RezervacijaFileWritter.GetBrojLinije(zapis.Id);
                RezervacijaFileWritter.ZapisiIzmenjen(linija, zapis);
                return "Rezervacija odbijena.";
            }
            else
            {
                return "Nismo uspeli da nadjemo vasu rezervaciju.";
            }
        }

        [HttpPost]
        [Route("api/domacin/RezervacijaPrihvati")]
        public string RezervacijaPrihvati(Rezervacija rez)
        {
            if (RezervacijaGetter.GetRezervacijaById(rez.Id) != null)
            {
                Rezervacija zapis = RezervacijaGetter.GetRezervacijaById(rez.Id);
                zapis.Status = StatusRezervacije.PRIHVACENA;
                int linija = RezervacijaFileWritter.GetBrojLinije(zapis.Id);
                RezervacijaFileWritter.ZapisiIzmenjen(linija, zapis);
                return "Rezervacija prihvacena.";
            }
            else
            {
                return "Nismo uspeli da nadjemo vasu rezervaciju.";
            }
        }

        [HttpPost]
        [Route("api/domacin/RezervacijaZavrsi")]
        public string RezervacijaZavrsi(Rezervacija rez)
        {
            if (RezervacijaGetter.GetRezervacijaById(rez.Id) != null)
            {
                Rezervacija zapis = RezervacijaGetter.GetRezervacijaById(rez.Id);
                zapis.Status = StatusRezervacije.ZAVRSENA;
                int linija = RezervacijaFileWritter.GetBrojLinije(zapis.Id);
                RezervacijaFileWritter.ZapisiIzmenjen(linija, zapis);
                return "Rezervacija zavrsena.";
            }
            else
            {
                return "Nismo uspeli da nadjemo vasu rezervaciju.";
            }
        }

        [HttpGet]
        [Route("api/domacin/PrikaziKomentare")]
        public List<Komentar> PrikaziKomentare(string apartman)
        {
            return KomentariGetter.GetKomentare(apartman);
        }

        [HttpPost]
        [Route("api/domacin/ValidirajKomentare")]
        public string ValidirajKomentare(string[] validniKomentari)
        {
            foreach(var item in validniKomentari)
            {
                KomentarFileWritter.OmoguciCitanje(item);
            }
            return "Uspesno!";
        }

        
        [HttpPost]
        [Route("api/domacin/IzmeniApartman")]
        public string IzmeniApartman(Apartman zaIzmenu)
        {
            if (ApartmanGetter.GetApartmanByNaziv(zaIzmenu.Naziv) == null)
            {
                Apartman apartman = ApartmanGetter.GetApartmanByNaziv(apartmanZaIzmenu);
                apartman.BrGostiju = zaIzmenu.BrGostiju;
                apartman.Tip = zaIzmenu.Tip;
                apartman.BrSoba = zaIzmenu.BrSoba;
                apartman.CenaPoNocenju = zaIzmenu.CenaPoNocenju;
                apartman.Naziv = zaIzmenu.Naziv;
                apartman.Status = zaIzmenu.Status;
                apartman.Prijava = zaIzmenu.Prijava;
                apartman.Odjava = zaIzmenu.Odjava;
                if(zaIzmenu.SadrzajApartmana.Count != 0)
                    apartman.SadrzajApartmana = zaIzmenu.SadrzajApartmana;
                if (zaIzmenu.DatumiZaIzdavanje.Count != 0)
                {
                    apartman.DatumiZaIzdavanje = zaIzmenu.DatumiZaIzdavanje;
                }
                if (zaIzmenu.Slike.Count != 0)
                {
                    apartman.Slike = zaIzmenu.Slike;
                }
                int brLinije = ApartmanGetter.GetBrojLinije(apartmanZaIzmenu);

                ApartmanFileWritter.ZapisiIzmenjen(brLinije, apartman);
                RezervacijaFileWritter.IzmeniNazivApartmana(apartmanZaIzmenu, apartman.Naziv);
                KomentarFileWritter.IzmeniNazivApartmana(apartmanZaIzmenu, apartman.Naziv);
                return "uspesno";
            }
            else
            {
                return "Apartman sa tim nazivom vec postoji.";
            }
            

        }


        [HttpGet]
        [Route("api/domacin/PrikaziApartmane")]
        public List<Apartman> PrikaziApartmane(string KorisnickoIme)
        {
            List<Apartman> apartmani = ApartmanGetter.GetApartmanByKorisnickoIme(KorisnickoIme);
            return apartmani;
        }

        [HttpGet]
        [Route("api/domacin/PrikaziSortiraneApartmane")]
        public List<Apartman> PrikaziSortiraneApartmane(string Poredak, string KorisnickoIme)
        {
            List<Apartman> pom = ApartmanGetter.GetApartmanByKorisnickoIme(KorisnickoIme);
            List<Apartman> ret = new List<Apartman>();

            for (int i = 0; i < pom.Count; i++)
            {
                if (pom[i].Status == StatusApartmana.NEAKTIVAN)
                    pom.Remove(pom[i]);
            }

            if (Poredak == "Opadajuci")
                ret = pom.OrderByDescending(x => x.CenaPoNocenju).ToList();
            else
                ret = pom.OrderBy(x => x.CenaPoNocenju).ToList();
            return ret;
        }

        [HttpGet]
        [Route("api/domacin/GetFiltriraneApartmane")]
        public List<Apartman> GetFiltriraneApartmane(string Filter, string KorisnickoIme)
        {
            List<Apartman> pom = ApartmanGetter.GetApartmanByKorisnickoIme(KorisnickoIme);
            List<Apartman> ret = new List<Apartman>();

            if (Filter == "CEO")
                ret = pom.FindAll(x => x.Tip == TipApartmana.CEO);
            else if (Filter == "SOBA")
                ret = pom.FindAll(x => x.Tip == TipApartmana.SOBA);
            else if (Filter == "AKTIVAN")
                ret = pom.FindAll(x => x.Status == StatusApartmana.AKTIVAN);
            else if (Filter == "NEAKTIVAN")
                ret = pom.FindAll(x => x.Status == StatusApartmana.NEAKTIVAN);
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


        
    }
}
