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

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        void connectionString()
        {
            con.ConnectionString = "Data Source=NBGV00116;Initial Catalog=FAPS;User ID=sa;Password=root";
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
                /*int teste0 = (int)dr.GetValue(0);
                string teste1 = (string)dr.GetValue(1);
                string teste2 = (string)dr.GetValue(2);
                string teste3 = (string)dr.GetValue(3);*/

                if (dr.GetValue(3).Equals("admin"))
                {
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