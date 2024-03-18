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
        public ActionResult Authorise(Account account)
        {
            using (DBModels db = new DBModels())
            {
                var accountDetail = db.Accounts.Where(x => x.FirstName == account.FirstName && x.LastName == account.LastName && x.Pin == account.Pin).FirstOrDefault();

                if (accountDetail == null)
                {
                    account.LoginErrorMessage = "Wrong name or password";

                    return View("Index", account);
                }
                else
                {
                    Session["FirstName"] = account.FirstName;
                    Session["LastName"] = account.LastName;
                    Session["Id"] = account.Id;


                    if (accountDetail.AccountType == (short)Account.EAccountType.Admin)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
        }

        public ActionResult LogOut()
        {
            int? id;

            if (Session["Id"] == null)
                id = null;
            else
                id = (int)Session["Id"];

            Session.Abandon();

            return RedirectToAction("Index", "Login");
        }
    }
}