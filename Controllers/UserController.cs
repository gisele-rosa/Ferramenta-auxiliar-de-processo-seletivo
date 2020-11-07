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
            //validação usuario logado
            //Copular Log do Sistema
            int id_usuario = (int)Session["id_user"];


            FAPSEntities db = new FAPSEntities();

            //Responsavel por colocar o nome do usuario nas views User
            var nome = db.Curriculo.Where(f => f.codigo_user == id_usuario).FirstOrDefault().Nome;
            ViewBag.nome = nome;
            ViewBag.id_applyer = id_usuario;


            //veririca se o usuario esta candidatado em alguma vaga---------------------------------------------------
            var Applyed_Status = db.Candidaturas.Where(f => f.Codigo_user == id_usuario).FirstOrDefault()?.Status_candidatura;
            if (Applyed_Status != null) {
                if (Applyed_Status == 1)
                {

                    //Candidatura realizada
                    return RedirectToAction("User_home_1", "User");

                }
                else if (Applyed_Status == 2)
                {

                    //Curriculo em Analise pela equipe
                    return RedirectToAction("User_home_2", "User");

                }
                else {

                    //Entrevista
                    return RedirectToAction("User_home_3", "User");
                }

            }
            

            //Copula a tela home Status vaga = 0 SEM CANDIDATURA A NENHUMA VAGA
            var getVagasLista = db.Vagas.ToList();

            return View(getVagasLista);
        }




        //view Candidatura realizada STATUS CANDIDATURA = 1
        public ActionResult User_home_1()
        {
            //validação usuario logado
            //Copular Log do Sistema
            int id_usuario = (int)Session["id_user"];


            FAPSEntities db = new FAPSEntities();

            //Responsavel por colocar o nome do usuario nas views User
            var nome = db.Curriculo.Where(f => f.codigo_user == id_usuario).FirstOrDefault()?.Nome;
            ViewBag.nome = nome;

            //Responsavel por colocar o nome do usuario nas views User
            var candidatura = db.Candidaturas.Where(f => f.Codigo_user == id_usuario).FirstOrDefault()?.Vagas.Vaga;
            ViewBag.vaga = candidatura;


            return View();
        }



        //view CANDIDATURA EM ANALISE == STATUS CANDIDATURA = 2
        public ActionResult User_home_2()
        {
            //validação usuario logado
            //Copular Log do Sistema
            int id_usuario = (int)Session["id_user"];

            FAPSEntities db = new FAPSEntities();

            //Responsavel por colocar o nome do usuario nas views User
            var nome = db.Curriculo.Where(f => f.codigo_user == id_usuario).FirstOrDefault()?.Nome;
            ViewBag.nome = nome;

            //Responsavel por colocar o nome do usuario nas views User
            var candidatura = db.Candidaturas.Where(f => f.Codigo_user == id_usuario).FirstOrDefault()?.Vagas.Vaga;
            ViewBag.vaga = candidatura;


            return View();
        }




        //view CANDIDATURA EM ANALISE == STATUS CANDIDATURA = 2
        public ActionResult User_home_3()
        {
            //validação usuario logado
            //Copular Log do Sistema
            int id_usuario = (int)Session["id_user"];

            FAPSEntities db = new FAPSEntities();

            //Responsavel por colocar o nome do usuario nas views User
            var nome = db.Curriculo.Where(f => f.codigo_user == id_usuario).FirstOrDefault()?.Nome;
            ViewBag.nome = nome;

            //Responsavel por colocar o nome do usuario nas views User
            var candidatura = db.Candidaturas.Where(f => f.Codigo_user == id_usuario).FirstOrDefault()?.Vagas.Vaga;
            ViewBag.vaga = candidatura;

            var DataEntrevista = db.Interview.Where(f => f.Codigo_user == id_usuario).FirstOrDefault()?.Data_Entrevista;
            ViewBag.dataEntrevista = DataEntrevista;


            return View();
        }




        //Aplica para a vaga, recebe o id da vaga e o id do usuario que esta aplicando para a vaga
        [HttpGet]
        public ActionResult Apply(int id_vaga, int id_applyer)
        {

            FAPSEntities db = new FAPSEntities();

            Candidaturas cd = new Candidaturas();

            cd.Codigo_user = id_applyer;
            //STATUS DA CANDIDATURA == 1 OU SEJA "CANDIDATOU SE COM SUCESSO PARA A VAGA"
            cd.Status_candidatura = 1;
            cd.Codigo_Vaga = id_vaga;

            db.Candidaturas.Add(cd);
            db.SaveChanges();

            //retorna a para a home e carrega ela com o id do usuario
            return RedirectToAction("User_home", "User", new { id = id_applyer });
        }


    }
}