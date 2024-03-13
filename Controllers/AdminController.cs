using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using BankingApp.Models;

namespace BankingApp.Controllers
{
    public class AdminController : Controller
    {
        private DBModels db = new DBModels();

        public bool ValidString(string str)
        {
            return str != null || !str.IsEmpty();
        }

        public int GetNextId()
        {
            return db.Database.ExecuteSqlCommand("SELECT IDENT_CURRENT('User') + 1");
        }

        public string GetAccountNumber(string firstName, string lastName)
        {
            int a = firstName[0] - 'a' + 1;
            int b = lastName[0] - 'a' + 1;

            return $"{firstName[0]}{lastName[0]}-{firstName.Length + lastName.Length}-{a}-{b}";
        }

        public string PinHash(string firstName, string lastName, int id)
        {
            int value = (firstName[0] - 'a' + 1) * 100 + lastName[0] - 'a' + 1 + id;

            return value.ToString("D4");
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Email,Pin,AccountNumber,UserType")] User user)
        {
            if (ValidString(user.FirstName) && ValidString(user.LastName) && ValidString(user.Email))
            {
                user.Id = GetNextId();
                user.Pin = PinHash(user.FirstName.ToLower(), user.LastName.ToLower(), user.Id);
                user.AccountNumber = GetAccountNumber(user.FirstName.ToLower(), user.LastName.ToLower());
                user.UserType = (byte)Models.User.EUserType.Customer;

                Account currentAccount = new Account();
                currentAccount.UserId = user.Id;
                currentAccount.AccountType = (byte)Account.EAccountType.Current;
                currentAccount.Balance = 0f;

                Account savingAccount = new Account();
                savingAccount.UserId = user.Id;
                savingAccount.AccountType = (byte)Account.EAccountType.Saving;
                savingAccount.Balance = 0f;

                db.Users.Add(user);
                db.Accounts.Add(currentAccount);
                db.Accounts.Add(savingAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,Pin,AccountNumber,UserType")] User user)
        {
            if (ValidString(user.FirstName) && ValidString(user.LastName) && ValidString(user.Email))
            {
                user.Pin = PinHash(user.FirstName.ToLower(), user.LastName.ToLower(), user.Id);
                user.AccountNumber = GetAccountNumber(user.FirstName.ToLower(), user.LastName.ToLower());

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            var accounts = db.Accounts.Where(x => x.UserId == id).ToArray();

            foreach (var account in accounts)
            {
                Account temp = db.Accounts.Find(account.Id);

                if (temp != null)
                {
                    db.Accounts.Remove(temp);
                }
            }

            db.Users.Remove(user);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CreateAccount(int? id, byte? accountType)
        {
            if (id == null || accountType == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            Account account = new Account();
            account.UserId = user.Id;
            account.AccountType = (byte)accountType;
            account.Balance = 0f;
            db.Accounts.Add(account);
            db.SaveChanges();
            return RedirectToAction("Details", "Admin", user);
        }

        public ActionResult DeleteAccount(int? accountId)
        {
            if (accountId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Account account = db.Accounts.Find(accountId);

            if (account == null)
            {
                return HttpNotFound();
            }

            int userId = account.UserId;

            db.Accounts.Remove(account);
            db.SaveChanges();

            return RedirectToAction("Details", "Admin", db.Users.Find(userId));
        }

        public ActionResult Transaction(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult LogOut()
        {
            return RedirectToAction("LogOut", "Login");
        }
    }
}
