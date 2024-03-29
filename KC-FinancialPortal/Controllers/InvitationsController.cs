﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using KC_FinancialPortal.Extensions;
using KC_FinancialPortal.Helpers;
using KC_FinancialPortal.Models;
using Microsoft.AspNet.Identity;

namespace KC_FinancialPortal.Controllers
{
    public class InvitationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();

        // GET: Invitations
        public ActionResult Index()
        {
            var invitations = db.Invitations.Include(i => i.Household);
            return View(invitations.ToList());
        }

        // GET: Invitations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // GET: Invitations/Create
        [Authorize]
        public ActionResult Create()
        {
            var houseId = db.Users.Find(User.Identity.GetUserId()).HouseholdId ?? 0;
            if (houseId == 0)
                return RedirectToAction("Login", "Account");
            var invitation = new Invitation
            {
                HouseholdId = houseId,
                TTL = 7
            };

            return View(invitation);
        }

        // POST: Invitations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "HouseholdId,TTL,RecipientEmail")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                invitation.Created = DateTime.Now;
                invitation.Code = Guid.NewGuid();
                invitation.IsValid = true;
                db.Invitations.Add(invitation);
                var userr = User.Identity.GetUserId();
                if (!roleHelper.IsDemoUser(userr))
                    db.SaveChanges();

                await invitation.EmailInvitation();

                return RedirectToAction("Index", "Home");
            }

            return View(invitation);
        }

        // GET: Invitations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invitation.HouseholdId);
            return View(invitation);
        }

        // POST: Invitations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseholdId,IsValid,Created,Subject,Body,TTL,RecipientEmail,Code")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invitation).State = EntityState.Modified;
                var userr = User.Identity.GetUserId();
                if (!roleHelper.IsDemoUser(userr))
                    db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invitation.HouseholdId);
            return View(invitation);
        }

        // GET: Invitations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // POST: Invitations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invitation invitation = db.Invitations.Find(id);
            db.Invitations.Remove(invitation);
            var userr = User.Identity.GetUserId();
            if (!roleHelper.IsDemoUser(userr))
                db.SaveChanges();
            return RedirectToAction("Index");
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
