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

    public class salesController : Controller
    {
        private ProductManage1Entities1 db = new ProductManage1Entities1();

        //Create Year SelectList
        private SelectList GetYearSelectList()
        {
            Dictionary<int, string> dic1 = new Dictionary<int, string>();
            dic1.Add(0, "");
            foreach (int y in Enumerable.Range(2000, 51))
            {
                dic1.Add(y, y.ToString());
            }
            return new SelectList(dic1, "Key", "Value", Session["syear"]);
        }

        //Create Month SelectList
        private SelectList GetMonthSelectList()
        {
            Dictionary<int, string> dic1 = new Dictionary<int, string>();
            dic1.Add(0, "");
            foreach (int y in Enumerable.Range(1, 12))
            {
                dic1.Add(y, y.ToString());
            }
            return new SelectList(dic1, "Key", "Value", Session["smonth"]);
        }

        //Create Day SelectList
        private SelectList GetDaySelectList()
        {
            Dictionary<int, string> dic1 = new Dictionary<int, string>();
            dic1.Add(0, "");
            foreach (int y in Enumerable.Range(1, 31))
            {
                dic1.Add(y, y.ToString());
            }
            return new SelectList(dic1, "Key", "Value", Session["sday"]);
        }

        //Get Index Select Item
        public IQueryable<sale> GetSelectedItemList()
        {
            var pd = db.sale.Include(s => s.customer).Include(s => s.product);
            if ((int)Session["syear"] > 0 && (int)Session["smonth"] > 0 && (int)Session["sday"] > 0)
            {
                DateTime dt = DateTime.Parse(((int)Session["syear"]).ToString() + "/" + ((int)Session["smonth"]).ToString() + "/" + ((int)Session["sday"]).ToString());
                pd = pd.Where(x => x.date.Year == dt.Year);
                pd = pd.Where(x => x.date.Month == dt.Month);
                pd = pd.Where(x => x.date.Day == dt.Day);
            }
            else if ((int)Session["syear"] > 0 && (int)Session["smonth"] > 0 && (int)Session["sday"] == 0)
            {
                DateTime dt = DateTime.Parse(((int)Session["syear"]).ToString() + "/" + ((int)Session["smonth"]).ToString() + "/" + "01");
                pd = pd.Where(x => x.date.Year == dt.Year);
                pd = pd.Where(x => x.date.Month == dt.Month);
            }
            else if ((int)Session["syear"] > 0 && (int)Session["smonth"] == 0 && (int)Session["sday"] == 0)
            {
                DateTime dt = DateTime.Parse(((int)Session["syear"]).ToString() + "/" + "01" + "/" + "01");
                pd = pd.Where(x => x.date.Year == dt.Year);
            }

            return pd;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Select(string year, string month, string day)
        {
            Session["syear"] = int.Parse(year);
            Session["smonth"] = int.Parse(month);
            Session["sday"] = int.Parse(day);

            return RedirectToAction("Index", new { page = 1 });
        }



        // GET: sales
        [Route("~/sales")]
        [Route("~/sales/index")]
        [Route("~/sales/page{page}")]
        public ActionResult Index(int? page)
        {
            if (page == null)
            {
                Session["syear"] = 0; Session["smonth"] = 0; Session["sday"] = 0;
            }

            int pageNumber = page ?? 1;
            if (pageNumber < 1) pageNumber = 1;
            int pageSize = 10;

            ViewBag.year = GetYearSelectList();
            ViewBag.month = GetMonthSelectList();
            ViewBag.day = GetDaySelectList();

            var sale = GetSelectedItemList();

            IPagedList<sale> sales = sale.OrderByDescending(p => p.id).ToPagedList(pageNumber, pageSize);
            return View(sales);
        }

        // GET: sales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sale sale = db.sale.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        public JsonResult GetProducts(string id)
        {

            int iid = int.Parse(id);
            product pd = db.product.Find(iid);

            ResultProduct rpd = new ResultProduct() { PName = pd.name, PStok = pd.stok, PValue = (decimal)pd.value };

            return Json(rpd, JsonRequestBehavior.AllowGet);
        }


        // GET: sales/Create1
        public ActionResult Create1(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = db.product.Find(id);
            if (product == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.product_id = product.id;
            ViewBag.pcode = product.pcode;
            ViewBag.pname = product.name;
            ViewBag.stok = product.stok;
            ViewBag.pvalue = product.value;
            ViewBag.customer_id = new SelectList(db.customer.OrderBy(x => x.name), "id", "name");

            return View();
        }


        // GET: sales/Create
        public ActionResult Create()
        {
            ViewBag.customer_id = new SelectList(db.customer.OrderBy(x=>x.name), "id", "name");
            ViewBag.product_id = new SelectList(db.product.OrderBy(x=>x.pcode), "id", "pcode");
            return View();
        }

        // POST: sales/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "product_id,customer_id,value,qnt")] sale sale)
        {
            /*
            if (ModelState.IsValid)
            {
                db.sale.Add(sale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            */

            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                sale.date = dt;
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        product p = db.product.Find(sale.product_id);
                        if (p == null)
                        {
                            tran.Rollback();
                        }
                        else
                        {
                            db.sale.Add(sale);
                            db.SaveChanges();
                            p.stok = p.stok - sale.qnt;
                            db.Entry(p).State = EntityState.Modified;
                            db.SaveChanges();
                            tran.Commit();
                        }
                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        return RedirectToAction("DeleteUserSuccess", "Home", new { message = e.Message });
                    }
                }

            }
            ViewBag.customer_id = new SelectList(db.customer, "id", "name", sale.customer_id);
            ViewBag.product_id = new SelectList(db.product, "id", "pcode", sale.product_id);
            return View(sale);
        }

        // GET: sales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sale sale = db.sale.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            ViewBag.customer_id = new SelectList(db.customer, "id", "name", sale.customer_id);
            ViewBag.product_id = new SelectList(db.product, "id", "pcode", sale.product_id);
            return View(sale);
        }

        // POST: sales/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,product_id,customer_id,value,qnt,date")] sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customer_id = new SelectList(db.customer, "id", "name", sale.customer_id);
            ViewBag.product_id = new SelectList(db.product, "id", "pcode", sale.product_id);
            return View(sale);
        }

        // GET: sales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sale sale = db.sale.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // POST: sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            /*
            sale sale = db.sale.Find(id);
            db.sale.Remove(sale);
            db.SaveChanges();
            */
            if (ModelState.IsValid)
            {
                sale sale = db.sale.Find(id);

                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        product p = db.product.Find(sale.product_id);
                        if (p == null)
                        {
                            tran.Rollback();
                        }
                        else
                        {
                            p.stok = p.stok + sale.qnt;
                            db.Entry(p).State = EntityState.Modified;
                            db.SaveChanges();
                            db.sale.Remove(sale);
                            db.SaveChanges();
                            tran.Commit();
                        }
                        return RedirectToAction("Index"); ;
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        return RedirectToAction("DeleteUserSuccess", "Home", new { message = e.Message });
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
