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
    public class ResultProduct
    {
        public string PName { get; set; }
        public decimal PValue { get; set; }
        public int PStok { get; set; }
    }

    public class buysController : Controller
    {
        private ProductManage1Entities1 db = new ProductManage1Entities1();

        // GET: buys
        public ActionResult Index()
        {
            var buy = db.buy.Include(b => b.product).Include(b => b.supplier);
            return View(buy.ToList());
        }

        // GET: buys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            buy buy = db.buy.Find(id);
            if (buy == null)
            {
                return HttpNotFound();
            }
            return View(buy);
        }

        public JsonResult GetProducts(string id)
        {

            int iid = int.Parse(id);
            product pd = db.product.Find(iid);

            ResultProduct rpd = new ResultProduct() { PName = pd.name,PStok = pd.stok,PValue =(decimal) pd.value };

            return Json(rpd, JsonRequestBehavior.AllowGet);
        }


            // GET: buys/Create
            public ActionResult Create()
        {
            ViewBag.product_id = new SelectList(db.product, "id", "pcode");
            ViewBag.supplier_id = new SelectList(db.supplier, "id", "name");
            return View();
        }

        // POST: buys/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,product_id,supplier_id,value,qnt,date")] buy buy)
        {
            if (ModelState.IsValid)
            {
                using (var tran = db.Database.BeginTransaction())
                {
                        try
                        {
                            product p = db.product.Find(buy.product_id);
                            if (p == null)
                            {
                                tran.Rollback();
                            }
                            else
                            {
                                db.buy.Add(buy);
                                db.SaveChanges();
                                p.stok = p.stok + buy.qnt;
                                db.Entry(p).State = EntityState.Modified;
                                db.SaveChanges();
                                tran.Commit();
                            }
                            return RedirectToAction("Index");
                        }
                        catch (Exception)
                        {
                            tran.Rollback();
                            throw; 
                        }
                }

                //db.buy.Add(buy);
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }

            ViewBag.product_id = new SelectList(db.product, "id", "pcode", buy.product_id);
            ViewBag.supplier_id = new SelectList(db.supplier, "id", "name", buy.supplier_id);
            return View(buy);
        }

        // GET: buys/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            buy buy = db.buy.Find(id);
            if (buy == null)
            {
                return HttpNotFound();
            }

            ViewBag.product_id = new SelectList(db.product, "id", "pcode", buy.product_id);
            ViewBag.supplier_id = new SelectList(db.supplier, "id", "name", buy.supplier_id);
            return View(buy);
        }

        // POST: buys/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,product_id,supplier_id,value,qnt,date")] buy buy,string old_qnt)
        {
            /*
            if (ModelState.IsValid)
            {
                db.Entry(buy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            */
            int oqnt=int.Parse(old_qnt);
            if (ModelState.IsValid)
            {                

                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        product p = db.product.Find(buy.product_id);
                        if (p == null)
                        {
                            tran.Rollback();
                        }
                        else
                        {
                            p.stok = p.stok + buy.qnt - oqnt;
                            db.Entry(p).State = EntityState.Modified;
                            db.SaveChanges();
                            db.Entry(buy).State = EntityState.Modified;
                            db.SaveChanges();
                            tran.Commit();
                        }
                        return RedirectToAction("Index"); ;
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
            ViewBag.product_id = new SelectList(db.product, "id", "pcode", buy.product_id);
            ViewBag.supplier_id = new SelectList(db.supplier, "id", "name", buy.supplier_id);

            return View(buy);
        }

        // GET: buys/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            buy buy = db.buy.Find(id);
            if (buy == null)
            {
                return HttpNotFound();
            }
            return View(buy);
        }

        // POST: buys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                buy buy = db.buy.Find(id);

                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        product p = db.product.Find(buy.product_id);
                        if (p == null)
                        {
                            tran.Rollback();
                        }
                        else
                        {
                            p.stok = p.stok - buy.qnt;
                            db.Entry(p).State = EntityState.Modified;
                            db.SaveChanges();
                            db.buy.Remove(buy);
                            db.SaveChanges();
                            tran.Commit();
                        }
                        return RedirectToAction("Index"); ;
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }

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
