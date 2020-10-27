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
        // Home admin
        public ActionResult Admin_home()
        {
            FAPSEntities vagas_Entity = new FAPSEntities();
            var getVagasLista = vagas_Entity.Vagas.ToList();

            return View(getVagasLista);

        }

        //Carrega a view do cadastrar vagas
        public ActionResult Cadastrar_vaga()
        {
            return View();
        }

        //BackEnd do cadastrar vagas (salvar)
        [HttpPost]
        public ActionResult Confirmar_vaga(Vagas vagas)
        {
            FAPSEntities vagas_Entity = new FAPSEntities();
            vagas_Entity.Vagas.Add(vagas);
            vagas_Entity.SaveChanges();

            return RedirectToAction("Admin_home", "Admin");
        }

        //Chama a view ver candidaturas da vaga
        [HttpGet]
        public ActionResult Ver_candidaturas(int id)
        {
            FAPSEntities db = new FAPSEntities();

            var getCandidaturasLista = db.Candidaturas.Where(f => f.Codigo_Vaga == id).ToList();

            /*var Lista = from a in db.Usuarios.Where(f => f. == id)
                        from b in numbersB
                        from c in numbersC
                        where a == b && c == b
                        select new { a, b ,c};*/


            return View(getCandidaturasLista);

        }


        //Deletar vagas
        [HttpGet]
        public ActionResult Deletar_vaga(int id)
        {
            FAPSEntities vagas_Entity = new FAPSEntities();

            Vagas v = vagas_Entity.Vagas.Find(id);
            vagas_Entity.Vagas.Remove(v);
            vagas_Entity.SaveChanges();


            return RedirectToAction("Admin_home", "Admin");
        }


    }
}