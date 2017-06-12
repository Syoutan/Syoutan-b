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
    public class suppliersController : Controller
    {
        private ProductManage1Entities1 db = new ProductManage1Entities1();

        // GET: suppliers
        public ActionResult Index()
        {
            return View(db.supplier.ToList());
        }

        // GET: suppliers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplier supplier = db.supplier.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: suppliers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: suppliers/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,address")] supplier supplier)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.supplier.Add(supplier);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception e)
                {
                    return RedirectToAction("DeleteUserSuccess", "Home", new { message = e.Message });
                }
            }

            return View(supplier);
        }

        // GET: suppliers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplier supplier = db.supplier.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: suppliers/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,address")] supplier supplier)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(supplier).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception e)
                {
                    return RedirectToAction("DeleteUserSuccess", "Home", new { message = e.Message });
                }
            }
            return View(supplier);
        }

        // GET: suppliers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplier supplier = db.supplier.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                supplier supplier = db.supplier.Find(id);
                db.supplier.Remove(supplier);
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
