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
        // Home admin
        public ActionResult Admin_home()
        {
            //validação usuario logado
            //Copular Log do Sistema
            var user_id = Session["id_admin"];


            FAPSEntities db = new FAPSEntities();
            var getVagasLista = db.Vagas.ToList();

            return View(getVagasLista);

        }


        //Carrega a view do cadastrar vagas---------------------------------------------------------------------------------------------------------------------------------------------------
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





        //Recebe da view admin o id da vaga que precisa alterar : UPDATE VAGA
        [HttpGet]
        public ActionResult Listar_vaga_to_update(int id_vaga)
        {
            FAPSEntities db = new FAPSEntities();

            var vaga_to_update = db.Vagas.Where(f => f.Codigo_vaga == id_vaga).FirstOrDefault();


            return View("Alterar_vaga", vaga_to_update);
        }


        //Recebe a vaga editada da view e salva ela
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






        //Chama a view ver candidaturas da vaga e espera receber o codigo da vaga "id"---------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult Ver_candidaturas(int id)
        {
            FAPSEntities db = new FAPSEntities();

            var getCandidaturasLista = db.Candidaturas.Where(f => f.Codigo_Vaga == id && f.Status_candidatura < 3).ToList();


            //Cogido_vaga da tabela vagas é com o "v" minusculo
            ViewBag.NomeVaga = db.Vagas.Where(f => f.Codigo_vaga == id).FirstOrDefault().Vaga;

            return View(getCandidaturasLista);

        }


        //Interrompe ou recusa o processo seletivo do candidato / deleta candidatura
        [HttpGet]
        public ActionResult Deletar_candidatura(int id_candidatura)
        {

            FAPSEntities db = new FAPSEntities();

            //pego o user antes de rodar o delete
            var c_user = db.Candidaturas.Where(linha => linha.Codigo_Candidatura == id_candidatura).FirstOrDefault().Codigo_user;

            //pego o codigo da vaga antes de rodar o delete
            var codigo_vaga = db.Candidaturas.Where(linha => linha.Codigo_user == c_user).FirstOrDefault().Codigo_Vaga;


            //valida se a candidatura tem alguma entrevista relacionada, se tiver ela precisa ser deletada
            var id_interview = db.Interview.Where(l => l.Codigo_user == c_user).FirstOrDefault()?.Codigo_entrevista;
            if (id_interview != null)
            {
                //agora podemos deletar a entrevista
                Interview i = db.Interview.Find(id_interview);
                db.Interview.Remove(i);
            }


            Candidaturas c = db.Candidaturas.Find(id_candidatura);
            db.Candidaturas.Remove(c);
            db.SaveChanges();


            return RedirectToAction("Admin_home", "Admin");

        }






        //Carrega a view Analisar_curriculo/ Backend da view Analisar_curriculo e espera o id do candidato---------------------------------------------------------------------------------
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



        //Carrega a view agendarmento da entrevista com as informacoes do candidato----------------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult Agendar_entrevista(int id_candidato)
        {
            int admin_id = (int)Session["id_admin"];
            


            FAPSEntities db = new FAPSEntities();

            //Coloca na view bag o nome do candidato confirme o nome que esta no curriculo
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


            entrevista.Data_Entrevista = DateTime.Now;

            entrevista.Status_interview = "Em aberto";

            //pega a vaga que esse candidato esta concorrendo
            entrevista.Vaga = db.Candidaturas.Where(f => f.Codigo_user == id_candidato).FirstOrDefault().Vagas.Vaga;


            //*Manda pra a propria view a MODEL "entrevista" com as alteracoes somente faltando o preencimento da data da entrevista
            //**Na view deve ter HiddenFor para cada item da model acima para que esses campos não sejam editados pelo usuario e para que sejam salvos no modelo que a view vai enviar de voltar pra proxima action
            return View(entrevista);
        }




        //Agendamento da entrevista e salva a model que vem da view no db
        [HttpPost]
        public ActionResult Marcar_entrevista(Interview entrevista)
        {

            FAPSEntities db = new FAPSEntities();


            //status 3 = aprovado para entrevista
            var Candidatura_to_update = db.Candidaturas.Where(f => f.Codigo_user == entrevista.Codigo_user).FirstOrDefault();
            Candidatura_to_update.Status_candidatura = 3;

            TryUpdateModel(Candidatura_to_update);


            db.Interview.Add(entrevista);

            db.SaveChanges();

            return RedirectToAction("Admin_home", "Admin");
        }






        //Lista e controla entrevistas agendadas--------------------------------------------------------------------------------------------------------------------------------------------------
        public ActionResult Listar_interviews()
        {
            FAPSEntities db = new FAPSEntities();

            var getInterviewsList = db.Interview.ToList();

            return View(getInterviewsList);
        }


        //Concluir Interview : Remove do sistema a entrevista e a candidatura relacionada
        [HttpGet]
        public ActionResult Concluir_interview(int id)
        {
            FAPSEntities db = new FAPSEntities();

            //Abaixo eu busco pelo id do canditato que será necessario para remover a candidatura na linha seguinte a essa
            var id_user = db.Interview.Where(f => f.Codigo_entrevista == id).FirstOrDefault()?.Codigo_user;

            //Removendo a candidatura do sistema
            var Candidatura_to_delete = db.Candidaturas.Where(f => f.Codigo_user == id_user)?.FirstOrDefault();
            db.Candidaturas.Remove(Candidatura_to_delete);

            //Mudando o status da interview para Concluido
            Interview i = db.Interview.Find(id);
            i.Status_interview = "Concluido";

            TryUpdateModel(i);

            db.SaveChanges();

            return RedirectToAction("Listar_interviews", "Admin");
        }



        //Deletar Interview
        [HttpGet]
        public ActionResult Deletar_interview(int id)
        {
            FAPSEntities db = new FAPSEntities();

            //Para deletar uma entrevista eu preciso regressar a candidatura do candidato ao status 2 (em analise)
            //Abaixo eu busco pelo id do canditato que será necessario para alterar a candidatura na linha seguinte a essa
            var id_user = db.Interview.Where(f => f.Codigo_entrevista == id).FirstOrDefault()?.Codigo_user;

            //Alteração do status da candidatura para 2
            var Candidatura_to_update = db.Candidaturas.Where(f => f.Codigo_user == id_user)?.FirstOrDefault();
            Candidatura_to_update.Status_candidatura = 2;
            TryUpdateModel(Candidatura_to_update);


            //agora podemos deletar a entrevista
            Interview i = db.Interview.Find(id);
            db.Interview.Remove(i);


            db.SaveChanges();

            return RedirectToAction("Listar_interviews", "Admin");
        }







        //Lista e controla Usuarios---------------------------------------------------------------------------------------------------------------------------------------------------------------
        public ActionResult Listar_users()
        {
            FAPSEntities db = new FAPSEntities();

            var getUserList = db.Usuarios.ToList();

            return View(getUserList);
        }


        //Recebe da view admin o id da vaga que precisa alterar : UPDATE
        [HttpGet]
        public ActionResult Listar_usuario_to_update(int id)
        {


            FAPSEntities db = new FAPSEntities();

            var user_to_update = db.Usuarios.Where(f => f.Codigo_user == id).FirstOrDefault();


            return View("Alterar_usuario", user_to_update);


        }


        //Recebe a vaga editada da view e salva ela
        [HttpPost]
        public ActionResult Alterar_usuario(Usuarios user_to_update)
        {
            FAPSEntities db = new FAPSEntities();

            //Procura a vaga a ser salva a altera item por item conforme oque veio da view
            var to_update = db.Usuarios.Where(f => f.Codigo_user == user_to_update.Codigo_user).FirstOrDefault();
            to_update.Codigo_user = user_to_update.Codigo_user;
            to_update.Usuario = user_to_update.Usuario;
            to_update.Senha = user_to_update.Senha;
            to_update.role = user_to_update.role;

            TryUpdateModel(to_update);
            db.SaveChanges();

            return RedirectToAction("Listar_users", "Admin");
        }



        //Deletar Usuario : DELETE
        [HttpGet]
        public ActionResult Deletar_user(int id)
        {
            FAPSEntities db = new FAPSEntities();

            Usuarios u = db.Usuarios.Find(id);
            db.Usuarios.Remove(u);
            db.SaveChanges();

            return RedirectToAction("Listar_users", "Admin");
        }




        //Carrega a view do cadastrar usuario : CREATE
        public ActionResult Cadastrar_usuario()
        {
            return View();
        }


        //BackEnd do cadastrar vagas (salvar)
        [HttpPost]
        public ActionResult Confirmar_usuario(Usuarios user)
        {
            FAPSEntities db = new FAPSEntities();
            db.Usuarios.Add(user);
            db.SaveChanges();

            return RedirectToAction("Listar_users", "Admin");
        }







        //Listar Curriculos-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public ActionResult Listar_curriculos()
        {
            FAPSEntities db = new FAPSEntities();

            var getCurriculoList = db.Curriculo.ToList();

            return View(getCurriculoList);
        }


        //Detalhes do curriculo
        [HttpGet]
        public ActionResult Detalhes_curriculo(int id)
        {
            FAPSEntities db = new FAPSEntities();

            //validação usuario logado
            var user_id = Session["id_admin"];
            //Copular Log do sistema


            //Consulta no db o curriculo do candidato
            var getCurriculo = db.Curriculo.Where(f => f.codigo_user == id);

            ViewBag.nome = getCurriculo.FirstOrDefault()?.Nome + " " + getCurriculo.FirstOrDefault()?.SobreNome;


            return View(getCurriculo);
        }

    }
}