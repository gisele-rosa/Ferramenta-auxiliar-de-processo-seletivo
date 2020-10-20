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
                //return View("Create");
                if (dr.GetValue(2).Equals("admin"))
                {
                    con.Close();
                    return View("../Admin/Admin");
                }
                else {
                    con.Close();
                    return View("../User/User");
                }
            }
            else
            {
                con.Close();
                return View("Error");
            }
            

            
        }
    }
}