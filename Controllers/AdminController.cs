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
            FAPSEntities db = new FAPSEntities();
            db.Vagas.Add(vagas);
            db.SaveChanges();

            return RedirectToAction("Admin_home", "Admin");
        }


        //Recebe da view admin o id da vaga que precisa alterar
        [HttpGet]
        public ActionResult listar_vaga_to_update(int id_vaga)
        {
            FAPSEntities db = new FAPSEntities();

            var vaga_to_update = db.Vagas.Where(f => f.Codigo_vaga == id_vaga).FirstOrDefault();


            return View("Alterar_vaga", vaga_to_update);
        }


        //Recebe a vaga da view e salva ela
        [HttpPost]
        public ActionResult Alterar_vaga(Vagas vaga_to_update)
        {
            FAPSEntities db = new FAPSEntities();

            //Procura a vaga a ser salva a altera item por item conforme oque veio da view
            var to_update = db.Vagas.Where(f => f.Codigo_vaga == vaga_to_update.Codigo_vaga).FirstOrDefault();
            to_update.Codigo_vaga = vaga_to_update.Codigo_vaga;
            to_update.Vaga = vaga_to_update.Vaga;
            to_update.Vaga_descricao = vaga_to_update.Vaga_descricao;

            TryUpdateModel(to_update);
            db.SaveChanges();

            return RedirectToAction("Admin_home", "Admin");
        }

        


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

            //validação usuario logado
            var user_id = Session["id_admin"];
            //Copular Log do sistema


            //Consulta no db o curriculo do candidato
            var getCurriculo = db.Curriculo.Where(f => f.codigo_user == id_candidato);

            ViewBag.nome = getCurriculo.FirstOrDefault()?.Nome + " " + getCurriculo.FirstOrDefault()?.SobreNome; ;
            ViewBag.CodigoCandidatura = db.Candidaturas.Where(f => f.Codigo_user == id_candidato).FirstOrDefault()?.Codigo_Candidatura;


            //Altera o status do curriculo para 2 = em analise pela equipe // somente se for menor que 3 pq 3 é o status da entrevista
            var status_candidatura = db.Candidaturas.Where(f => f.Codigo_user == id_candidato).FirstOrDefault()?.Status_candidatura;
            if (status_candidatura < 3) {
                var Candidatura_to_update = db.Candidaturas.Where(f => f.Codigo_user == id_candidato).FirstOrDefault();
                Candidatura_to_update.Status_candidatura = 2;
                TryUpdateModel(Candidatura_to_update);
                db.SaveChanges();
            }

            //tratamento de null exception e carregar na view o curriculo do candidato
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

            //status 3 = aprovado para entrevista
            Candidatura_to_update.Status_candidatura = 3;

            TryUpdateModel(Candidatura_to_update);
            db.SaveChanges();


            return RedirectToAction("Agendar_entrevista", "Admin", new { id_candidato });
        }



        //Carrega a view agendarmento da entrevista com as informacoes do candidato
        [HttpGet]
        public ActionResult Agendar_entrevista(int id_candidato)
        {
            int admin_id = (int)Session["id_admin"];
            
            FAPSEntities db = new FAPSEntities();

            ViewBag.Candidato = db.Curriculo.FirstOrDefault()?.Nome + " " + db.Curriculo.FirstOrDefault()?.SobreNome;

            //instancia e copula a model interview q vai ser enviada para a Agendar Entrevista
            Interview entrevista = new Interview();
            entrevista.Codigo_user = id_candidato;

            //tratamento null / preenche o entrevistador na model interview
            if (db.Usuarios.Where(f => f.Codigo_user == admin_id).FirstOrDefault()?.Usuario != null)
            {
                entrevista.Entrevistador = db.Usuarios.Where(f => f.Codigo_user == admin_id).FirstOrDefault()?.Usuario;
            }
            else {
                entrevista.Entrevistador = "nenhum";
            }

            entrevista.Data_criacao = DateTime.Now;


            //pega o codigo da vaga que esse candidato esta concorrendo
            int codigo_vaga = db.Candidaturas.Where(f => f.Codigo_user == id_candidato).FirstOrDefault().Codigo_Vaga;
            entrevista.Vaga = db.Vagas.Where(f => f.Codigo_vaga == codigo_vaga).FirstOrDefault().Vaga;


            //manda pra a propria view a model acima com as alteracoes somente faltando o preencimento da data da entrevista
            //Na view deve ter HiddenFor para cada item da model acima
            //para que esses campos não sejam editados pelo usuario e para que sejam salvos no modelo que a view vai enviar de voltar pra proxima action
            return View(entrevista);
        }




        //Agendamento da entrevista e salva a model que vem da view no db
        [HttpPost]
        public ActionResult Marcar_entrevista(Interview entrevista)
        {

            FAPSEntities db = new FAPSEntities();

            db.Interview.Add(entrevista);
            db.SaveChanges();

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


        //Lista e controla entrevistas agendadas
        public ActionResult Listar_interviews()
        {
            FAPSEntities db = new FAPSEntities();

            var getInterviewsList = db.Interview.ToList();

            return View(getInterviewsList);
        }


    }
}