using Microsoft.AspNet.Identity;
using System;
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
    [Authorize]
    public class MessageBoardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MessageBoard
        [AllowAnonymous]
        public ActionResult Index()
        {
            //var messageBoardModels = db.MessageBoardModels.Include(m => m.User);
            return View(db.MessageBoardModels.ToList());
        }

        // GET: MessageBoard/Details/5
        [AllowAnonymous]
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
            //ViewBag.UserID = new SelectList(db.MessageBoardModels.User);
            return View();
        }

        // POST: MessageBoard/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Content")] MessageBoardModel messageBoardModel)
        {
            if (ModelState.IsValid)
            {
                messageBoardModel.CreateTime = DateTime.Now;
                messageBoardModel.UserID = User.Identity.GetUserId();
                db.MessageBoardModels.Add(messageBoardModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.UserID = new SelectList(db.ApplicationUsers, "Id", "Email", messageBoardModel.UserID);
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
            if (messageBoardModel == null || messageBoardModel.UserID != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            //ViewBag.UserID = new SelectList(db.ApplicationUsers, "Id", "Email", messageBoardModel.UserID);
            return View(messageBoardModel);
        }

        // POST: MessageBoard/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Content")] MessageBoardModel messageBoardModel)
        {
            if (ModelState.IsValid && messageBoardModel.UserID == User.Identity.GetUserId())
            {
                messageBoardModel.CreateTime = DateTime.Now;
                messageBoardModel.UserID = User.Identity.GetUserId();
                db.Entry(messageBoardModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.UserID = new SelectList(db.ApplicationUsers, "Id", "Email", messageBoardModel.UserID);
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
            if (messageBoardModel == null || messageBoardModel.UserID != User.Identity.GetUserId())
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
            if (messageBoardModel.UserID != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
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
