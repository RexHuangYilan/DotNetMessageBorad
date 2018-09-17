﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationRexMessageBoard.Models;

namespace WebApplicationRexMessageBoard
{
    public class MessageBoardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MessageBoard
        public ActionResult Index()
        {
            var messageBoardModels = db.MessageBoardModels.Include(m => m.User);
            return View(messageBoardModels.ToList());
        }

        // GET: MessageBoard/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageBoardModel messageBoardModel = db.MessageBoardModels.Find(id);
            if (messageBoardModel == null)
            {
                return HttpNotFound();
            }
            return View(messageBoardModel);
        }

        // GET: MessageBoard/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.ApplicationUsers, "Id", "Email");
            return View();
        }

        // POST: MessageBoard/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Content,CreateTime,UserID")] MessageBoardModel messageBoardModel)
        {
            if (ModelState.IsValid)
            {
                db.MessageBoardModels.Add(messageBoardModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.ApplicationUsers, "Id", "Email", messageBoardModel.UserID);
            return View(messageBoardModel);
        }

        // GET: MessageBoard/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageBoardModel messageBoardModel = db.MessageBoardModels.Find(id);
            if (messageBoardModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.ApplicationUsers, "Id", "Email", messageBoardModel.UserID);
            return View(messageBoardModel);
        }

        // POST: MessageBoard/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Content,CreateTime,UserID")] MessageBoardModel messageBoardModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(messageBoardModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.ApplicationUsers, "Id", "Email", messageBoardModel.UserID);
            return View(messageBoardModel);
        }

        // GET: MessageBoard/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageBoardModel messageBoardModel = db.MessageBoardModels.Find(id);
            if (messageBoardModel == null)
            {
                return HttpNotFound();
            }
            return View(messageBoardModel);
        }

        // POST: MessageBoard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MessageBoardModel messageBoardModel = db.MessageBoardModels.Find(id);
            db.MessageBoardModels.Remove(messageBoardModel);
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
