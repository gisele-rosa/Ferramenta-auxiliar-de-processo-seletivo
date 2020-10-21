using Faps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Faps.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult User_home()
        {
            FAPSEntities2 vagas_Entity = new FAPSEntities2();
            var getVagasLista = vagas_Entity.Vagas.ToList();
            return View(getVagasLista);
        }
    }
}