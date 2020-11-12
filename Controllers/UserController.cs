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







        //Permite o usuario listar seu curriculo
        public ActionResult Listar_curriculo()
        {
            //Validação usuario logado
            //Copular Log do Sistema
            int id_usuario = (int)Session["id_user"];


            FAPSEntities db = new FAPSEntities();
            //Responsavel por colocar o nome do usuario nas views User
            var nome = db.Curriculo.Where(f => f.codigo_user == id_usuario).FirstOrDefault()?.Nome;
            ViewBag.nome = nome;



            //Consulta no db o curriculo do candidato
            var getCurriculo = db.Curriculo.Where(f => f.codigo_user == id_usuario);

            return View("Listar_curriculo_user",getCurriculo);
        }

        
        //Chama a partial view que carregada com o curriculo que sera editado
        [HttpGet]
        public ActionResult Editar_curriculo(int id)
        {
            FAPSEntities db = new FAPSEntities();

            Curriculo c = db.Curriculo.Where(f => f.codigo_curriculo == id).FirstOrDefault();

            return PartialView("_Editar_curriculo", c);
        }


        //Recebe o curriculo editado da partial acima view e salva ele
        [HttpPost]
        public ActionResult Salvar_curriculo(Curriculo c)
        {
            FAPSEntities db = new FAPSEntities();

            //Procura a vaga a ser salva a altera item por item conforme oque veio da view
            var to_update = db.Curriculo.Where(f => f.codigo_curriculo == c.codigo_curriculo).FirstOrDefault();
            to_update.codigo_curriculo = c.codigo_curriculo;
            to_update.codigo_user = c.codigo_user;

            to_update.Nome = c.Nome;
            to_update.SobreNome = c.SobreNome;
            to_update.Email = c.Email;
            to_update.Telefone = c.Telefone;
            to_update.Genero = c.Genero;
            to_update.DataNascimento = c.DataNascimento;
            to_update.Endereco = c.Endereco;
            to_update.Cidade = c.Cidade;
            to_update.Estado = c.Estado;
            to_update.CEP = c.CEP;
            to_update.Pais = c.Pais;
            to_update.Curso = c.Curso;
            to_update.Curso_status = c.Curso_status;
            to_update.TituloCargo = c.TituloCargo;
            to_update.Empresa = c.Empresa;
            to_update.Data_inicio = c.Data_inicio;
            to_update.DataTermino = c.DataTermino;
            to_update.DescricaoAtividades = c.DescricaoAtividades;


            TryUpdateModel(to_update);
            db.SaveChanges();

            return RedirectToAction("Listar_curriculo", "User");
        }


        //Salvar foto no curriculo
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            int id_usuario = (int)Session["id_user"];


            if (file != null)
            {
                FAPSEntities db = new FAPSEntities();
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                string physicalPath = Server.MapPath("~/images/" + ImageName);

                // save image in folder
                file.SaveAs(physicalPath);

                //save new record in database
                tblA newRecord = new tblA();
                newRecord.fname = Request.Form["fname"];
                newRecord.lname = Request.Form["lname"];
                newRecord.imageUrl = ImageName;
                newRecord.Cod_user = id_usuario;
                db.tblA.Add(newRecord);
                db.SaveChanges();

            }
            //Display records
            return RedirectToAction("../home/Display/");
        }



    }
}