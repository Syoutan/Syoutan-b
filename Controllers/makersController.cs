using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationTest3.Models;

namespace WebApplicationTest3.Controllers
{
    public class makersController : Controller
    {
        private ProductManage1Entities1 db = new ProductManage1Entities1();

        // GET: makers
        public ActionResult Index()
        {
            return View(db.maker.ToList());
        }

        // GET: makers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            maker maker = db.maker.Find(id);
            if (maker == null)
            {
                return HttpNotFound();
            }
            return View(maker);
        }

        // GET: makers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: makers/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,PostNo,address1,TEL")] maker maker)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.maker.Add(maker);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception e)
                {
                    return RedirectToAction("DeleteUserSuccess", "Home", new { message = e.Message });
                }
            }

            return View(maker);
        }

        // GET: makers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            maker maker = db.maker.Find(id);
            if (maker == null)
            {
                return HttpNotFound();
            }
            return View(maker);
        }

        // POST: makers/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,PostNo,address1,TEL")] maker maker)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(maker).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception e)
                {
                    return RedirectToAction("DeleteUserSuccess", "Home", new { message = e.Message });
                }
            }
            return View(maker);
        }

        // GET: makers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            maker maker = db.maker.Find(id);
            if (maker == null)
            {
                return HttpNotFound();
            }
            return View(maker);
        }

        // POST: makers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                maker maker = db.maker.Find(id);
                db.maker.Remove(maker);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                return RedirectToAction("DeleteUserSuccess", "Home", new { message = e.Message });
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
