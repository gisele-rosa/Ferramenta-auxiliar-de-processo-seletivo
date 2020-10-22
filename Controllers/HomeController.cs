using Faps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Faps.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home_Vagas
        public ActionResult Home_Vagas()
        {
            EntitiesFAPS vagas_Entity = new EntitiesFAPS();
            var getVagasLista = vagas_Entity.Vagas.ToList();

            return View(getVagasLista);
        }
    }
}