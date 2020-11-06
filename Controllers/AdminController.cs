using Faps.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Faps.Controllers
{
    public class AdminController : Controller
    {
        // Home admin esperando o id do admin
        public ActionResult Admin_home()
        {
            var user_id = Session["id_admin"];


            FAPSEntities db = new FAPSEntities();
            var getVagasLista = db.Vagas.ToList();

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

        //Atualizar vagas






        //Deletar vagas, espera o id da vaga ou codigo_vaga
        [HttpGet]
        public ActionResult Deletar_vaga(int id)
        {
            FAPSEntities vagas_Entity = new FAPSEntities();

            Vagas v = vagas_Entity.Vagas.Find(id);
            vagas_Entity.Vagas.Remove(v);
            vagas_Entity.SaveChanges();


            return RedirectToAction("Admin_home", "Admin");
        }




        //Chama a view ver candidaturas da vaga e espera receber o codigo da vaga "id"
        [HttpGet]
        public ActionResult Ver_candidaturas(int id)
        {
            FAPSEntities db = new FAPSEntities();

            var getCandidaturasLista = db.Candidaturas.Where(f => f.Codigo_Vaga == id).ToList();


            //Cogido_vaga da tabela vagas é com o "v" minusculo
            ViewBag.NomeVaga = db.Vagas.Where(f => f.Codigo_vaga == id).FirstOrDefault().Vaga;

            return View(getCandidaturasLista);

        }


        //Carrega a view Analisar_curriculo/ Backend da view Analisar_curriculo e espera o id do candidato
        [HttpGet]
        public ActionResult Analisar_curriculo(int id_candidato)
        {

            FAPSEntities db = new FAPSEntities();

            var getCurriculo = db.Curriculo.Where(f => f.codigo_user == id_candidato);

            var user_id = Session["id_admin"];

            ViewBag.nome = getCurriculo.FirstOrDefault()?.Nome + " " + getCurriculo.FirstOrDefault()?.SobreNome; ;
            ViewBag.CodigoCandidatura = db.Candidaturas.Where(f => f.Codigo_user == id_candidato).FirstOrDefault()?.Codigo_Candidatura;

            //tratamento de null exception
            if (getCurriculo.Any())
            {
                return View(getCurriculo);
            }
            else
            {
                //não posso retornar nulo pra view, ela exige o codigo da vaga da qual esse null curriculo seria
                return RedirectToAction("Ver_candidaturas", "Admin", new { id = db.Candidaturas.Where(linha => linha.Codigo_user == id_candidato).FirstOrDefault().Codigo_Vaga });
            }


        }

        //Aprova a candidatura chama a view de agendamento da entrevista
        public ActionResult Aprovar_candidatura(int id_candidato)
        {
            FAPSEntities db = new FAPSEntities();

            var Candidatura_to_update = db.Candidaturas.Where(f => f.Codigo_user == id_candidato).FirstOrDefault();

            //status 2 = aprovado para entrevista
            Candidatura_to_update.Status_candidatura = 2;

            TryUpdateModel(Candidatura_to_update);
            db.SaveChanges();


            return View("Agendar_entrevista");
        }

        //Agendamento da entrevista
        [HttpPost]
        public ActionResult Agendar_entrevista(Interview entrevista)
        {
            FAPSEntities db = new FAPSEntities();
           /* vagas_Entity.Vagas.Add(vagas);
            vagas_Entity.SaveChanges();*/

            return RedirectToAction("Admin_home", "Admin");
        }




        //Interrompe ou recusa o processo seletivo do candidato
        [HttpGet]
        public ActionResult Deletar_candidatura(int id_candidatura)
        {

            FAPSEntities db = new FAPSEntities();

            //pego o user antes de rodar o delete
            var c_user = db.Candidaturas.Where(linha => linha.Codigo_Candidatura == id_candidatura).FirstOrDefault().Codigo_user;

            //pego o codigo da vaga antes de rodar o delete
            var codigo_vaga = db.Candidaturas.Where(linha => linha.Codigo_user == c_user).FirstOrDefault().Codigo_Vaga;


            Candidaturas c = db.Candidaturas.Find(id_candidatura);
            db.Candidaturas.Remove(c);
            db.SaveChanges();


            return RedirectToAction("Admin_home", "Admin");

        }



    }
}