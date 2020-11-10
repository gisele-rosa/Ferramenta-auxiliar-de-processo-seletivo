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
            var nome = db.Curriculo.Where(f => f.codigo_user == id_usuario).FirstOrDefault()?.Nome;
            ViewBag.nome = nome;
            ViewBag.id_applyer = id_usuario;

            //pega o status da candidatura do usuario
            var Applyed_Status = db.Candidaturas.Where(f => f.Codigo_user == id_usuario).FirstOrDefault()?.Status_candidatura;


            //verifica se o usuario tem algum curriculo cadastrado
            if (db.Curriculo.Where(f => f.codigo_user == id_usuario).Any())
            {

                //veririca se o usuario esta candidatado em alguma vaga---------------------------------------------------
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
                else if (Applyed_Status == 3)
                {
                    //Entrevista
                    return RedirectToAction("User_home_3", "User");
                }
                else
                {
                    //Copula a tela home Status vaga = 0 SEM CANDIDATURA A NENHUMA VAGA
                    var getVagasLista = db.Vagas.ToList();

                    return View(getVagasLista);
                }

            }
            else
            {
                return RedirectToAction("Cadastro_curriculo", "User");

            }

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






        //Chama a view de cadastro do curriculo SOMENTE PARA USUARIOS SEM CURRICULO CADASTRADO - SOMENTE USUARIO CADASTRADO PELO ADMIN
        public ActionResult Cadastro_curriculo()
        {
            return View();
        }

        //Salva o cadastro do usuario
        [HttpPost]
        public ActionResult Salvar_registro(Curriculo resume)
        {
            //Validação usuario logado
            //Copular Log do Sistema
            int id_usuario = (int)Session["id_user"];


            FAPSEntities db = new FAPSEntities();

            resume.codigo_user = id_usuario;
            resume.Usuario = db.Usuarios.Where(f => f.Codigo_user == id_usuario).FirstOrDefault()?.Usuario;
            resume.Senha = db.Usuarios.Where(f => f.Codigo_user == id_usuario).FirstOrDefault()?.Senha;

            db.Curriculo.Add(resume);
            db.SaveChanges();

            return RedirectToAction("User_home", "User");
        }

    }
}