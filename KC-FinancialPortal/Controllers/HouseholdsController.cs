using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using KC_FinancialPortal.Models;
using Microsoft.AspNet.Identity;
using KC_FinancialPortal.Helpers;
using KC_FinancialPortal.Extensions;

namespace KC_FinancialPortal.Controllers
{
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();

        // GET: Households
        public ActionResult Index()
        {
            return View(db.Households.ToList());
        }

        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Greeting")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Households.Add(household);
                household.Created = DateTime.Now;
                household.Users.Add(db.Users.Find(User.Identity.GetUserId()));
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(household);
        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Greeting,Created")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> LeaveAsync()
        {
            var userId = User.Identity.GetUserId();

            //Determine Role
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            var user = db.Users.Find(userId);

            switch(myRole)
            {
                case "Head Of Household":
                    var inhabitants = db.Users.Where(u => u.HouseholdId == user.HouseholdId).Count();
                    if(inhabitants > 1)
                    {
                        TempData["Message"] = $"You are unable to leave the Household at this time. There are still <b>{inhabitants}</b> people in this Household.";
                    }
                    //User is removed from household
                    user.HouseholdId = null;
                    //Household is deleted
                    Household household = db.Households.Find(user.HouseholdId);
                    db.Households.Remove(household);
                    db.SaveChanges();

                    //Role removed from user
                    roleHelper.RemoveUserFromRole(userId, "Head Of Household");
                    await ControllerContext.HttpContext.RefreshAuthentication(user);

                    return RedirectToAction("Index", "Home");

                case "Member":
                default:
                    user.HouseholdId = null;
                    db.SaveChanges();
                    roleHelper.RemoveUserFromRole(userId, "Member");
                    await ControllerContext.HttpContext.RefreshAuthentication(user);
                    return RedirectToAction("Index", "Home");
            }
        }

        //Get
        public ActionResult Configure(int id)
        {
            var configVM = new ConfigureViewModel
            {
                HouseholdId = id
            };

            return View(configVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //Post
        public ActionResult Configure(ConfigureViewModel model)
        {
            if(ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();

                var newAccount = new BankAccount
                {
                    HouseholdId = model.HouseholdId,
                    Name = model.BankName,
                    AccountType = model.AccountType,
                    StartingBalance = model.StartingBalance,
                    CurrentBalance = model.CurrentBalance,
                    Created = DateTime.Now,
                    OwnerId = userId
                };

                db.BankAccounts.Add(newAccount);

                var newBudget = new Budget
                {
                    HouseholdId = model.HouseholdId,
                    Name = model.BudgetName,
                    TargetAmount = model.BudgetTarget,
                    Created = DateTime.Now,
                    OwnerId = userId
                };

                db.Budgets.Add(newBudget);

                var newBudgetItem = new BudgetItem
                {
                    BudgetId = newBudget.Id,
                    Name = model.BudgetItemName,
                    TargetAmount = model.BudgetTarget,
                    Created = DateTime.Now
                };

                db.BudgetItems.Add(newBudgetItem);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
