using ProjekatIzWeba.Models;
using PW32_2016_WebProjekat.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;

namespace ProjekatIzWeba.Controllers
{
    public class AdminController : ApiController
    {
        private static string username;
        private static string apartmanZaIzmenu;
        private static string sadrzajZaIzmenu;
        private static readonly string path = HostingEnvironment.MapPath("~/Files/Apartmani.txt");

        [HttpGet]
        [Route("api/admin/PrikaziRegistrovane")]
        public List<KorisnikModel> PrikaziRegistrovane()
        {
            return KorisnikGetter.GetKorisnike();
        }

        [HttpGet]
        [Route("api/admin/BlokirajKorisnika")]
        public string BlokirajKorisnika(string KorisnickoIme)
        {
            string putanjaBloka = HostingEnvironment.MapPath("~/Files/Blokirani.txt");
            List<string> blokirani = File.ReadAllLines(putanjaBloka).ToList();

            if (blokirani.Contains(KorisnickoIme))
            {
                File.WriteAllLines(putanjaBloka, blokirani.ToArray());
                return "Korisnik je vec blokiran";
            }
            else
            {
                blokirani.Add(KorisnickoIme);
                File.WriteAllLines(putanjaBloka, blokirani);
                return "Korisnik blokiran";
            }
            
        }

        [HttpGet]
        [Route("api/admin/OdblokirajKorisnika")]
        public string OdblokirajKorisnika(string KorisnickoIme)
        {
            string putanjaBloka = HostingEnvironment.MapPath("~/Files/Blokirani.txt");
            string[] blokirani = File.ReadAllLines(putanjaBloka);
            
            bool bioBlokiran = false;

            foreach(var item in blokirani)
            {
                if (item == KorisnickoIme)
                    bioBlokiran = true;
            }

            if (!bioBlokiran)
                return "Korisnik nije bio blokiran.";

            List<string> blokirani1 = new List<string>();

            for (int i = 0; i < blokirani.Length; i++)
            {
                if (blokirani[i] != KorisnickoIme)
                    blokirani1.Add(blokirani[i]);
            }

            File.WriteAllLines(putanjaBloka, blokirani1.ToArray());
            return "Korisnik odblokiran";
        }

        [HttpPost]
        [Route("api/admin/dodajDomacina")]
        public string DodajDomacina(Domacin domacin)
        {
            if (KorisnikGetter.GetKorisnikByIme(domacin.KorisnickoIme) != null)
            {
                return "korisnik sa unetim korisnickim imenom vec postoji";
            }
            else
            {
                KorisnikFileWriter.DodajKorisnika(domacin);
                return "korisnik uspesno dodat";
            }
        }

        [HttpPost]
        [Route("api/admin/GetData")]
        public Korisnik GetData(Korisnik korisnik)
        {
            Korisnik gost = KorisnikGetter.GetKorisnik(korisnik.KorisnickoIme, korisnik.Lozinka);
            username = gost.KorisnickoIme;
            return gost;
        }

        [HttpPost]
        [Route("api/admin/Izmeni")]
        public IHttpActionResult Izmeni(Gost korisnik)
        {
            if (username == korisnik.KorisnickoIme || KorisnikGetter.GetKorisnikByIme(korisnik.KorisnickoIme) == null)
            {
                Korisnik gost = KorisnikGetter.GetKorisnikByIme(username);
                int brLinije = KorisnikGetter.GetBrojLinije(username);
                gost.Ime = korisnik.Ime;
                gost.Uloga = Uloge.ADMINISTRATOR;
                gost.Prezime = korisnik.Prezime;
                gost.KorisnickoIme = korisnik.KorisnickoIme;
                gost.Lozinka = korisnik.Lozinka;
                username = korisnik.KorisnickoIme;
                KorisnikFileWriter.Zapisi(brLinije, gost);
                
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/admin/PrikaziApartmane")]
        public List<Apartman> PrikaziApartmane()
        {
            List<Apartman> apartmani = ApartmanGetter.GetApartmane();
            return apartmani;
        }

        [HttpGet]
        [Route("api/admin/PrikaziSortiraneApartmane")]
        public List<Apartman> PrikaziSortiraneApartmane(string Poredak)
        {
            List<Apartman> pom = ApartmanGetter.GetApartmane();
            List<Apartman> ret = new List<Apartman>();
            if (Poredak == "Opadajuci")
                ret = pom.OrderByDescending(x => x.CenaPoNocenju).ToList();
            else
                ret = pom.OrderBy(x => x.CenaPoNocenju).ToList();
            return ret;
        }

        [HttpPost]
        [Route("api/admin/DobaviApartman")]
        public Apartman DobaviApartman(Apartman apartmanNaziv)
        {
            Apartman ret = ApartmanGetter.GetApartmanByNaziv(apartmanNaziv.Naziv);
            apartmanZaIzmenu = apartmanNaziv.Naziv;
            return ret;
        }

        [HttpPost]
        [Route("api/admin/IzmeniApartman")]
        public string IzmeniApartman(Apartman zaIzmenu)
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
            apartman.SadrzajApartmana = zaIzmenu.SadrzajApartmana;
            int brLinije = ApartmanGetter.GetBrojLinije(apartmanZaIzmenu);
            
            ApartmanFileWritter.ZapisiIzmenjen(brLinije, apartman);
            RezervacijaFileWritter.IzmeniNazivApartmana(apartmanZaIzmenu, apartman.Naziv);
            KomentarFileWritter.IzmeniNazivApartmana(apartmanZaIzmenu, apartman.Naziv);
            return "uspesno";

        }

        [HttpPost]
        [Route("api/admin/ObrisiApartman")]
        public string ObrisiApartman(Apartman zaBrisanje)
        {
            Apartman apartman = ApartmanGetter.GetApartmanByNaziv(zaBrisanje.Naziv);
            int brLinije = ApartmanGetter.GetBrojLinije(zaBrisanje.Naziv);
            ApartmanFileWritter.ObrisiApartman(brLinije);
            return "Apartman obrisan uspesno.";

        }

        [HttpPost]
        [Route("api/admin/DodajSadrzaj")]
        public string DodajSadrzaj(SadrzajApartmanaModel obj)
        {

            SadrzajApartmana sa = new SadrzajApartmana(obj.Id, obj.Naziv);

            if (SadrzajApartmanaFileWritter.GetSadrzajById(sa.Id) == null)
            {
                if (!string.IsNullOrEmpty(obj.Id) && !string.IsNullOrWhiteSpace(obj.Id))
                {
                    SadrzajApartmanaFileWritter.ZapisiSadrzaj(sa);
                    return "Sadrzaj uspesno dodat.";
                }
                else
                {
                    return "Kreirani sadrzaj mora imati id i naziv";
                }
            }
            else
            {
                return "Sadrzaj sa tim id-em vec postoji.";
            }

        }
        

        [HttpPost]
        [Route("api/admin/DobaviApartmanoveDetalje")]
        public Apartman DobaviApartmanoveDetalje(Apartman apartmanNaziv)
        {
            Apartman ret = ApartmanGetter.GetApartmanByNaziv(apartmanNaziv.Naziv);
            //apartmanZaIzmenu = apartmanNaziv.Naziv;
            //List<Komentar> komentari = KomentariGetter.GetKomentareZaCitanje(apartmanNaziv.Naziv);
            List<Komentar> komentari = KomentariGetter.GetKomentare(apartmanNaziv.Naziv);
            ret.Komentari = komentari;
            return ret;
        }

        [HttpPost]
        [Route("api/admin/DobaviRezervacije")]
        public List<Rezervacija> DobaviRezervacije(Korisnik KorisnickoIme)
        {
            return RezervacijaGetter.GetRezervacijeAdmin();
        }

        [HttpGet]
        [Route("api/admin/DobaviSortiraneRezervacije")]
        public List<Rezervacija> DobaviSortiraneRezervacije(string PoredakR)
        {
            List<Rezervacija> pom = RezervacijaGetter.GetRezervacijeAdmin();
            List<Rezervacija> ret = new List<Rezervacija>();
            if (PoredakR == "Opadajuci")
                ret = pom.OrderByDescending(x => x.UkupnaCena).ToList();
            else
                ret = pom.OrderBy(x => x.UkupnaCena).ToList();

            return ret;
        }

        [HttpGet]
        [Route("api/admin/DobaviFiltriraneRezervacije")]
        public List<Rezervacija> DobaviFiltriraneRezervacije(string Filter)
        {
            List<Rezervacija> pom = RezervacijaGetter.GetRezervacijeAdmin();
            List<Rezervacija> ret = new List<Rezervacija>();
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

        [HttpGet]
        [Route("api/admin/DobaviSadrzaje")]
        public List<SadrzajApartmanaModel> DobaviSadrzaje()
        {
            return SadrzajApartmanaFileWritter.GetSavSadrzaj();
        }

        [HttpGet]
        [Route("api/admin/LoadSadrzaj")]
        public SadrzajApartmana LoadSadrzaj(string Id)
        {
            sadrzajZaIzmenu = Id;
            return SadrzajApartmanaFileWritter.GetSadrzajById(Id);
        }

        [HttpPost]
        [Route("api/admin/IzmeniSadrzaj")]
        public string IzmeniSadrzaj(SadrzajApartmana sadrzaj)
        {
            if(string.IsNullOrEmpty(sadrzaj.Id) || string.IsNullOrWhiteSpace(sadrzaj.Id) || sadrzaj.Naziv.Count==0 )
            {
                return "Id i naziv ne smeju biti prazni.";
            }
            else
            {
                SadrzajApartmanaFileWritter.ZapisiIzmenjenSadrzaj(sadrzajZaIzmenu, sadrzaj);
                return "Uspesno izmenjen";
            }
        }

        [HttpGet]
        [Route("api/admin/ObrisiSadrzaj")]
        public string ObrisiSadrzaj(string sadrzajId)
        {
            SadrzajApartmanaFileWritter.ObrisiSadrzaj(sadrzajId);
            return $"Sadrzaj {sadrzajId} uspesno obrisan.";
        }

        [HttpGet]
        [Route("api/admin/GetFiltriraneApartmane")]
        public List<Apartman> GetFiltriraneApartmane(string Filter)
        {
            List<Apartman> pom = ApartmanGetter.GetApartmane();
            List<Apartman> ret = new List<Apartman>();
            //List<SadrzajApartmanaModel> sadrzaj = SadrzajApartmanaFileWritter.GetSavSadrzaj();

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
                foreach(var item in pom)
                {
                    if (item.SadrzajApartmana.SingleOrDefault(x => x.Id == Filter) != null)
                        ret.Add(item);
                }
            }

            return ret;
        }

        [HttpGet]
        [Route("api/admin/PretragaKorisnika")]
        public List<KorisnikModel> PretragaKorisnika(string ElementPretrage)
        {
            List<KorisnikModel> pom = KorisnikGetter.GetKorisnike();
            List<KorisnikModel> ret = new List<KorisnikModel>();
            if (ElementPretrage == "MUSKI")
                ret = pom.FindAll(x => x.Pol == "MUSKI");
            else if (ElementPretrage == "ZENSKI")
                ret = pom.FindAll(x => x.Pol == "ZENSKI");
            else if (ElementPretrage == "ADMINISTRATOR")
                ret = pom.FindAll(x => x.Uloga == "ADMINISTRATOR");
            else if (ElementPretrage == "GOST")
                ret = pom.FindAll(x => x.Uloga == "GOST");
            else if (ElementPretrage == "DOMACIN")
                ret = pom.FindAll(x => x.Uloga == "DOMACIN");
            else
                ret.Add(pom.Find(x => x.KorisnickoIme == ElementPretrage));

            for(int i =0; i < ret.Count; i++)
            {
                if (ret[i] == null)
                    ret.Remove(ret[i]);
            }

            return ret;

        }

        [HttpGet]
        [Route("api/admin/PretraziRez")]
        public List<Rezervacija> PretraziRez(string ElementPretrage)
        {
            List<Rezervacija> pom = RezervacijaGetter.GetRezervacije(ElementPretrage);
            for(int i=0; i<pom.Count; i++)
            {
                if (pom[i] == null)
                    pom.Remove(pom[i]);
            }
            
            return pom;
        }


        [HttpPost]
        [Route("api/admin/PretraziApartmane")]
        public List<Apartman> PretraziApartmane(ApartmanPretragaModel zaPretragu)
        {
            List<Apartman> ret = ApartmanGetter.GetApartmane();

            if (!string.IsNullOrWhiteSpace(zaPretragu.Grad) && !string.IsNullOrEmpty(zaPretragu.Grad))
                ret = ret.FindAll(x => x.Lokacija.Adresa.NaseljenoMesto == zaPretragu.Grad);

            if (zaPretragu.BrSobaOD != 0)
                ret = ret.FindAll(x => x.BrSoba >= zaPretragu.BrSobaOD);

            if (zaPretragu.BrSobaDO != 0)
                ret = ret.FindAll(x => x.BrSoba <= zaPretragu.BrSobaDO);

            if (zaPretragu.BrOsobaOD != 0)
                ret = ret.FindAll(x => x.BrGostiju >= zaPretragu.BrOsobaOD);

            if(!string.IsNullOrWhiteSpace(zaPretragu.Dolazak) && !string.IsNullOrWhiteSpace(zaPretragu.Odlazak))
            {
                DateTime dolazak = DateTime.ParseExact(zaPretragu.Dolazak, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                //ret = ret.FindAll(x => x.DatumiZaIzdavanje.Contains(dolazak));

                DateTime odlazak = DateTime.ParseExact(zaPretragu.Odlazak, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                //ret = ret.FindAll(x => x.DatumiZaIzdavanje.Contains(dolazak));

                int brojDana = (int)(odlazak - dolazak).TotalDays;

                if (brojDana > 0)
                {
                    for (int i = 0; i <brojDana; i++)
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


        [HttpGet]
        [Route("api/admin/DodajPraznike")]
        public string DodajPraznike(string Praznici, string Prepisi)
         {
            string putanjaDatumi = HostingEnvironment.MapPath("~/Files/Praznici.txt");
            if (string.IsNullOrWhiteSpace(Praznici))
                return "Niste uneli nijedan praznik.";

            if(Prepisi== "ponisti")
            {
                File.WriteAllText(putanjaDatumi, Praznici);
                return "Praznici zamenjeni";
            }
            else
            {
                string datumi = File.ReadAllText(putanjaDatumi);
                datumi += $",{Praznici}";
                File.WriteAllText(putanjaDatumi, datumi);
                return "Novi praznici dodati";
            }
        }


    }
}
