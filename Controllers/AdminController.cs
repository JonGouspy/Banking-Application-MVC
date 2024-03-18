using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            return View(db.Accounts.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
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
        public ActionResult Create([Bind(Include = "FirstName,LastName,Email,Id,AccountType")] Account account)
        {
            if (ValidString(account.FirstName) && ValidString(account.LastName) && ValidString(account.Email) && account.AccountType >= 1)
            {
                account.AccountNumber = GetAccountNumber(account.FirstName.ToLower(), account.LastName.ToLower());
                account.Pin = PinHash(account.FirstName.ToLower(), account.LastName.ToLower(), account.Id);
                account.Balance = 0;

                db.Accounts.Add(account);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(account);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FirstName,LastName,Email")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            if (account.Balance != 0)
            {
                return RedirectToAction("Index");
            }
            return View(account);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Transaction(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
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
