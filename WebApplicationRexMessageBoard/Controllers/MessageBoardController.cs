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
            ViewBag.messageList = db.MessageBoardModels.ToList().OrderByDescending(m => m.CreateTime);
            return View();
        }

        // GET: MessageBoard/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            return DetailsConfirmed(id, null);
        }

        // GET: MessageBoard/Details/5
        [AllowAnonymous]
        public ActionResult DetailsConfirmed(int? id, MessageModels messageModel)
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
            var replayQuery = db.ReplyModels.Include(c => c.Message);
            var messages = replayQuery.Where(i => i.MessageBoardID == id).Select(c => c.Message).ToList();
            ViewBag.messageList = messages;
            ViewBag.MessageBoardModel = messageBoardModel;
            return View(messageModel);
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
            MessageBoardModel messageBoard = db.MessageBoardModels.Find(messageBoardModel.ID);
            if (ModelState.IsValid && messageBoard.UserID == User.Identity.GetUserId())
            {
                messageBoard.Title = messageBoardModel.Title;
                messageBoard.Content = messageBoardModel.Content;

                db.Entry(messageBoard).State = EntityState.Modified;
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
        public ActionResult DeleteHTML(int id)
        {
            MessageBoardModel messageBoardModel = DeleteConfirmed(id);
            if (messageBoardModel == null)
            {
                return HttpNotFound();
            }
            
            return RedirectToAction("Index");
        }

        // POST: MessageBoard/Delete/5
        [HttpPost, ActionName("DeleteAJAX")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAJAX(int id)
        {
            MessageBoardModel messageBoardModel = DeleteConfirmed(id);
            if (messageBoardModel == null)
            {
                return Json(new Dictionary<string, int>());
            }

            var json = new Dictionary<string, int>()
            {
                { "MessageBoardID", messageBoardModel.ID }
            };

            return Json(json);
        }

        // POST: MessageBoard/Delete/5
        private MessageBoardModel DeleteConfirmed(int id)
        {
            MessageBoardModel messageBoardModel = db.MessageBoardModels.Find(id);
            if (messageBoardModel.UserID != User.Identity.GetUserId())
            {
                return null;
            }
            db.MessageBoardModels.Remove(messageBoardModel);
            db.SaveChanges();
            return messageBoardModel;
        }

        // POST: MessageBoard/CreateMessage/5
        [HttpPost, ActionName("CreateMessage")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, [Bind(Include = "Content")] MessageModels messageModel)
        {
            if (ModelState.IsValid)
            {
                messageModel.CreateTime = DateTime.Now;
                messageModel.UserID = User.Identity.GetUserId();
                db.MessageModels.Add(messageModel);
                db.SaveChanges();

                ReplyModels replyModels = new ReplyModels()
                {
                    MessageBoardID = id,
                    MessageID = messageModel.ID
                };

                db.ReplyModels.Add(replyModels);
                db.SaveChanges();

                return RedirectToAction("Details", new { id = id });
            }

            //ViewBag.UserID = new SelectList(db.ApplicationUsers, "Id", "Email", messageBoardModel.UserID);
            return DetailsConfirmed(id, messageModel);
        }

        // GET: MessageBoard/Edit/5
        public ActionResult MessageEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageModels messageModel = db.MessageModels.Find(id);
            if (messageModel == null || messageModel.UserID != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            ReplyModels replyModels = db.ReplyModels.Where(r => r.MessageID == messageModel.ID).Single();
            ViewBag.MessageBoardID = replyModels.MessageBoardID;
            return View(messageModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MessageEdit([Bind(Include = "ID, Content")] MessageModels messageModel)
        {
            MessageModels message = db.MessageModels.Find(messageModel.ID);
            ReplyModels replyModels = db.ReplyModels.Where(r => r.MessageID == messageModel.ID).Single();
            ViewBag.MessageBoardID = replyModels.MessageBoardID;

            if (ModelState.IsValid && message.UserID == User.Identity.GetUserId())
            {
                message.Content = messageModel.Content;

                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", new { id = replyModels.MessageBoardID });
            }
            //ViewBag.UserID = new SelectList(db.ApplicationUsers, "Id", "Email", messageBoardModel.UserID);
            return View(messageModel);
        }

        // GET: MessageBoard/MessageDelete/5
        public ActionResult MessageDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageModels messageModel = db.MessageModels.Find(id);
            if (messageModel == null || messageModel.UserID != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            ReplyModels replyModels = db.ReplyModels.Where(r => r.MessageID == id).Single();
            ViewBag.MessageBoardID = replyModels.MessageBoardID;
            return View(messageModel);
        }

        // POST: MessageBoard/Delete/5
        [HttpPost, ActionName("MessageDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult MessageDeleteConfirmed(int id)
        {
            ReplyModels replyModel = MessageDeleteCionfirmed(id);
            if (replyModel == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Details", new { id = replyModel.MessageBoardID });
        }

        [HttpPost, ActionName("MessageDeleteAJAX")]
        [ValidateAntiForgeryToken]
        public ActionResult MessageDeleteCionfirmedAJAX(int id)
        {
            ReplyModels replyModel = MessageDeleteCionfirmed(id);

            if (replyModel == null)
            {
                return HttpNotFound();
            }

            Dictionary<string, int> json = new Dictionary<string, int>();
            json.Add("MessageID", replyModel.MessageID);

            return Json(json);
        }

        private ReplyModels MessageDeleteCionfirmed(int id)
        {
            MessageModels messageModel = db.MessageModels.Find(id);
            if (messageModel.UserID != User.Identity.GetUserId())
            {
                return null;
            }

            ReplyModels replyModels = db.ReplyModels.Where(r => r.MessageID == id).Single();
            db.MessageModels.Remove(messageModel);
            db.ReplyModels.Remove(replyModels);
            db.SaveChanges();

            return replyModels;
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
