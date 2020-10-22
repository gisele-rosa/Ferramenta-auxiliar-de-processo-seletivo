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


            /*Vagas vaga_teste = new Vagas();
            vaga_teste.Vaga = "Teste1";
            vaga_teste.Vaga_descricao = "Teste1";
            vagas_Entity.Vagas.Add(vaga_teste);
            vagas_Entity.SaveChanges();*/


            return View(getVagasLista);

        }

        public ActionResult Cadastrar_vaga()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Confirmar_vaga(Vagas vagas)
        {
            FAPSEntities2 vagas_Entity = new FAPSEntities2();
            vagas_Entity.Vagas.Add(vagas);
            vagas_Entity.SaveChanges();

            return RedirectToAction("Admin_home", "Admin");
        }


        [HttpGet]
        public ActionResult Deletar_vaga(int id)
        {
            FAPSEntities2 vagas_Entity = new FAPSEntities2();

            Vagas v = vagas_Entity.Vagas.Find(id);
            vagas_Entity.Vagas.Remove(v);
            vagas_Entity.SaveChanges();


            return RedirectToAction("Admin_home", "Admin");
        }


    }
}