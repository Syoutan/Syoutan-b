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
    public class ProfitView1Controller : Controller
    {
        private ProfitManagement1Entities2 db = new ProfitManagement1Entities2();

        // GET: ProfitView1
        public ActionResult Index()
        {
            return View(db.ProfitView1.ToList());
        }

        // GET: ProfitView1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfitView1 profitView1 = db.ProfitView1.Find(id);
            if (profitView1 == null)
            {
                return HttpNotFound();
            }
            return View(profitView1);
        }

        // GET: ProfitView1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProfitView1/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,value,qnt,date,pname,sname,pcode")] ProfitView1 profitView1)
        {
            if (ModelState.IsValid)
            {
                db.ProfitView1.Add(profitView1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(profitView1);
        }

        // GET: ProfitView1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfitView1 profitView1 = db.ProfitView1.Find(id);
            if (profitView1 == null)
            {
                return HttpNotFound();
            }
            return View(profitView1);
        }

        // POST: ProfitView1/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,value,qnt,date,pname,sname,pcode")] ProfitView1 profitView1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profitView1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(profitView1);
        }

        // GET: ProfitView1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfitView1 profitView1 = db.ProfitView1.Find(id);
            if (profitView1 == null)
            {
                return HttpNotFound();
            }
            return View(profitView1);
        }

        // POST: ProfitView1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProfitView1 profitView1 = db.ProfitView1.Find(id);
            db.ProfitView1.Remove(profitView1);
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
