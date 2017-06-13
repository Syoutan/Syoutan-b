using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationTest3.Models;
using PagedList;
using System.Data.Entity.Infrastructure;

namespace WebApplicationTest3.Controllers
{
    public class productsController : Controller
    {
        private ProductManage1Entities1 db = new ProductManage1Entities1();

        // GET: products
        [Route("~/products")]
        [Route("~/products/index")]
        [Route("~/products/page{page}")]
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            if (pageNumber < 1) pageNumber = 1;
            int pageSize = 10;

            Dictionary<int, string> dic1 = new Dictionary<int, string>();
            dic1.Add(0, "なし");
            foreach(maker m in db.maker)
            {
                dic1.Add(m.id, m.name);
            }
            Dictionary<int, string> dic2 = new Dictionary<int, string>();
            dic2.Add(0, "なし");
            foreach (category m in db.category)
            {
                dic2.Add(m.id, m.name);
            }
            ViewBag.category_id = new SelectList(dic2,"Key","Value");
            ViewBag.maker_id = new SelectList(dic1, "Key","Value" );

            var pd = db.product.Include(p => p.category).Include(p => p.maker);
            IPagedList<product> products = pd.OrderBy(p => p.id).ToPagedList(pageNumber, pageSize);
            return View(products);
        }

        // GET: products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: products/Create
        public ActionResult Create()
        {
            ViewBag.category_id = new SelectList(db.category, "id", "name");
            ViewBag.maker_id = new SelectList(db.maker, "id", "name");
            return View();
        }

        // POST: products/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,pcode,name,value,stok,maker_id,category_id")] product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.product.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(DbUpdateException e)
                {
                    return RedirectToAction("DeleteUserSuccess", "Home", new { message = e.Message });
                }
                catch (Exception e)
                {
                    return RedirectToAction("DeleteUserSuccess", "Home", new { message = e.Message });
                }
            }

            ViewBag.category_id = new SelectList(db.category, "id", "name", product.category_id);
            ViewBag.maker_id = new SelectList(db.maker, "id", "name", product.maker_id);
            return View(product);
        }

        // GET: products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.category, "id", "name", product.category_id);
            ViewBag.maker_id = new SelectList(db.maker, "id", "name", product.maker_id);
            return View(product);
        }

        // POST: products/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,pcode,name,value,stok,maker_id,category_id")] product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException e)
                {
                    return RedirectToAction("DeleteUserSuccess", "Home", new { message = e.Message });
                }
                catch (Exception e)
                {
                    return RedirectToAction("DeleteUserSuccess", "Home", new { message = e.Message });
                }
            }
            ViewBag.category_id = new SelectList(db.category, "id", "name", product.category_id);
            ViewBag.maker_id = new SelectList(db.maker, "id", "name", product.maker_id);
            return View(product);
        }

        // GET: products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                product product = db.product.Find(id);
                db.product.Remove(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("DeleteUserSuccess", "Home", new { message = e.Message });
            }
            catch (Exception e)
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
