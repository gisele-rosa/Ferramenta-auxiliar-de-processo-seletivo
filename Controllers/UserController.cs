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


            //Responsavel por colocar o nome do usuario nas views User e copular as view bags dessas telas
            var nome = db.Usuarios.Where(f => f.Codigo_user == id).FirstOrDefault().Usuario;
            ViewBag.nome = nome;
            ViewBag.id_applyer = id;


            //veririca se o usuario esta candidatado em alguma vaga---------------------------------------------------
            var Applyed_Status = db.Candidaturas.Where(f => f.Codigo_usuario == id).FirstOrDefault()?.Status_candidatura;
            if (Applyed_Status != null) {
                if (Applyed_Status == 1)
                {

                    //Candidatura realizada
                    Console.WriteLine("Candidatura realizada");

                }
                else if (Applyed_Status == 2)
                {

                    //Curriculo em Analise pela equipe

                }
                else {

                    //Entrevista


                }

            }
            


  
            //Copula a tela home Status vaga = 0 SEM CANDIDATURA A NENHUMA VAGA
            var getVagasLista = db.Vagas.ToList();

            return View(getVagasLista);
        }

        [HttpGet]
        public ActionResult Apply(int id_vaga, int id_applyer)
        {
            EntitiesFAPS db = new EntitiesFAPS();

            Candidaturas cd = new Candidaturas();

            cd.Codigo_usuario = id_applyer;
            cd.Status_candidatura = 1;
            cd.Codigo_vaga = id_vaga;


            db.Candidaturas.Add(cd);
            db.SaveChanges();


            return RedirectToAction("User_home", "User", new { id = id_applyer });
        }


    }
}