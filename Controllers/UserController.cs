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
        public ActionResult User_home(int id)
        {
            EntitiesFAPS db = new EntitiesFAPS();


            var nome = db.Usuarios.Where(f => f.Codigo_user == id).FirstOrDefault().Usuario;

            ViewBag.nome = nome;
            ViewBag.id_applyer = id;

            var getVagasLista = db.Vagas.ToList();
            return View(getVagasLista);
        }

        [HttpGet]
        public ActionResult Apply(int id_vaga, int id_applyer)
        {
            EntitiesFAPS db = new EntitiesFAPS();


            //var nome = db.Usuarios.Where(f => f.Codigo_user == id).FirstOrDefault().Usuario;


            //var getVagasLista = db.Vagas.ToList();
            return View();
        }

    }
}