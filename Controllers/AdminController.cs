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
            EntitiesFAPS vagas_Entity = new EntitiesFAPS();
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
            EntitiesFAPS vagas_Entity = new EntitiesFAPS();
            vagas_Entity.Vagas.Add(vagas);
            vagas_Entity.SaveChanges();

            return RedirectToAction("Admin_home", "Admin");
        }

        //Chama a view ver candidaturas da vaga
        [HttpGet]
        public ActionResult Ver_candidaturas(int id)
        {
            EntitiesFAPS db = new EntitiesFAPS();

            var getCandidaturasLista = db.Candidaturas.Where(f => f.Codigo_vaga == id).ToList();
            return View(getCandidaturasLista);

        }


        //Deletar vagas
        [HttpGet]
        public ActionResult Deletar_vaga(int id)
        {
            EntitiesFAPS vagas_Entity = new EntitiesFAPS();

            Vagas v = vagas_Entity.Vagas.Find(id);
            vagas_Entity.Vagas.Remove(v);
            vagas_Entity.SaveChanges();


            return RedirectToAction("Admin_home", "Admin");
        }


    }
}