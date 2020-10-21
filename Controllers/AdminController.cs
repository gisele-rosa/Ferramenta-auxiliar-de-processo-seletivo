using Faps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Faps.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Admin_home()
        {
            FAPSEntities2 vagas_Entity = new FAPSEntities2();
            var getVagasLista = vagas_Entity.Vagas.ToList();
            return View(getVagasLista);
        }

        public ActionResult Cadastrar_vaga()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Confirmar_vaga()
        {
            return View();
        }
    }
}