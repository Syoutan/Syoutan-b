﻿using System;
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
    public class categoriesController : Controller
    {
        private ProductManage1Entities1 db = new ProductManage1Entities1();

        // GET: categories
        public ActionResult Index()
        {
            return View(db.category.ToList());
        }

        // GET: categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = db.category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: categories/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name")] category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.category.Add(category);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception e)
                {
                    return RedirectToAction("DeleteUserSuccess", "Home", new { message = e.Message });
                }
            }

            return View(category);
        }

        // GET: categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = db.category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: categories/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name")] category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception e)
                {
                    return RedirectToAction("DeleteUserSuccess", "Home", new { message = e.Message });
                }
            }
            return View(category);
        }

        // GET: categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = db.category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                category category = db.category.Find(id);
                db.category.Remove(category);
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
