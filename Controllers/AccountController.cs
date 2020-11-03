using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Faps.Controllers;
using Faps.Models;
using System.Data.SqlClient;


namespace Faps.Controllers
{
    public class AccountController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Chama a view Do registro
        public ActionResult Register()
        {
            return View();
        }

        //Salva o registro do usuario
        [HttpPost]
        public ActionResult Salvar_registro(Curriculo resume)
        {

            Usuarios user = new Usuarios();
            user.role = "user";
            user.Usuario = resume.Usuario.ToString();
            user.Senha = resume.Senha.ToString();

            FAPSEntities db = new FAPSEntities();
            db.Usuarios.Add(user);
            db.Curriculo.Add(resume);
            db.SaveChanges();

            return View("Login");
        }


            void connectionString()
        {
            con.ConnectionString = "Data Source=GI-PC;Initial Catalog=FAPS;User ID=sa;Password=root";
        }
        [HttpPost]
        public ActionResult Verify(Account acc)
        {
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Usuarios where Usuario='"+acc.Name+ "' and Senha='"+acc.Password+"'";
            dr = com.ExecuteReader();


            if (dr.Read())
            {

                if (dr.GetValue(3).Equals("admin"))
                {
                    
                    Session["id_admin"] = (int)dr.GetValue(0);

                    con.Close();
                    return RedirectToAction("Admin_home", "Admin");
                }
                else if (dr.GetValue(3).Equals("user"))
                {
                    int codigo = (int)dr.GetValue(0);
                    con.Close();
                    return RedirectToAction("User_home", "User", new { id = codigo });
                }
                else {
                    con.Close();
                    return View("Error");
                }

            }else
            {
               con.Close();
               return View("Login");
            }

        }
    }
}