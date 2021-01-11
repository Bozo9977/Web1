using PW32_2016_WebProjekat.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;

namespace ProjekatIzWeba.Controllers
{
    public class RegistracijaController : ApiController
    {
        [HttpPost]
        [Route("api/registracija/Registracija")]
        public string Registracija(Gost gost)
        {
            string path = HostingEnvironment.MapPath("~/Files/Korisnici.txt");

            if (!File.Exists(path))
            {
                return "fajle ne postoji";
            }
            else
            {
                if (ValidirajPostojanje(gost.KorisnickoIme, path))
                {
                    return "Korisnik sa tim imenom vec postoji.";

                }
                else
                {
                    WriteToFile(gost);
                    return "korisnik uspesno dodat";
                }
            }

        }

        private bool ValidirajPostojanje(string username, string path)
        {
            bool postoji = false;

            string[] zapisano = File.ReadAllLines(path);

            string[] splitovano;
            foreach (var line in zapisano)
            {
                if (string.IsNullOrWhiteSpace(line) || string.IsNullOrEmpty(line))
                {
                    continue;
                }
                splitovano = line.Split('|');

                if (splitovano[2] == username)
                {
                    postoji = true;
                }
            }

            return postoji;
        }
        private void WriteToFile(Gost gost)
        {
            string path = HostingEnvironment.MapPath("~/Files/Korisnici.txt");


            if (File.Exists(path))
            {
                File.AppendAllText(path, gost.ToString());
            }
            else
            {
                File.WriteAllText(path, gost.ToString());
            }
        }
    }
}
