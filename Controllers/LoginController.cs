using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankingApp.Models;

namespace BankingApp.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorise(User user)
        {
            using (DBModels db = new DBModels())
            {
                var userDetail = db.Users.Where(x => x.FirstName == user.FirstName && x.LastName == user.LastName && x.Pin == user.Pin).FirstOrDefault();

                if (userDetail == null)
                {
                    user.LoginErrorMessage = "Wrong name or password";

                    return View("Index", user);
                }
                else
                {
                    Session["FirstName"] = user.FirstName;
                    Session["LastName"] = user.LastName;
                    Session["Id"] = user.Id;


                    if (userDetail.UserType == (byte)Models.User.EUserType.Administrator)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult LogOut()
        {
            int? id;

            if (Session["Id"] == null)
            {
                id = null;
            }
            else
            {
                id = (int)Session["Id"];
            }
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}