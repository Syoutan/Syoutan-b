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
    public class ResultProduct
    {
        public string PName { get; set; }
        public decimal PValue { get; set; }
        public int PStok { get; set; }
    }

    public class buysController : Controller
    {
        private ProductManage1Entities1 db = new ProductManage1Entities1();
        static private int _year = 0;
        static private int _month = 0;
        static private int _day = 0;

        //Create Year SelectList
        private SelectList GetYearSelectList()
        {
            Dictionary<int, string> dic1 = new Dictionary<int, string>();
            dic1.Add(0, "");
            foreach (int y  in Enumerable.Range(2000,51))
            {
                dic1.Add(y, y.ToString());
            }
            return new SelectList(dic1, "Key", "Value",buysController._year);
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
            return new SelectList(dic1, "Key", "Value", buysController._month);
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
            return new SelectList(dic1, "Key", "Value", buysController._day);
        }

        //Get Index Select Item
        public IQueryable<buy> GetSelectedItemList()
        {
            var pd = db.buy.Include(b => b.product).Include(b => b.supplier);
            if (buysController._year > 0 && buysController._month > 0 && buysController._day > 0)
            {
                DateTime dt = DateTime.Parse(buysController._year.ToString() + "/" + buysController._month.ToString() + "/" + buysController._day.ToString());
                pd = pd.Where(x => x.date.Year == dt.Year);
                pd = pd.Where(x => x.date.Month == dt.Month);
                pd = pd.Where(x => x.date.Day == dt.Day);
            }
            else if (buysController._year > 0 && buysController._month > 0 && buysController._day == 0)
            {
                DateTime dt = DateTime.Parse(buysController._year.ToString() + "/" + buysController._month.ToString() + "/" + "01");
                pd = pd.Where(x => x.date.Year == dt.Year);
                pd = pd.Where(x => x.date.Month == dt.Month);
            }
            else if (buysController._year > 0 && buysController._month == 0 && buysController._day == 0)
            {
                DateTime dt = DateTime.Parse(buysController._year.ToString() + "/" + "01" + "/" + "01");
                pd = pd.Where(x => x.date.Year == dt.Year);
            }

            return pd;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Select(string year, string month, string day )
        {
            buysController._year = int.Parse(year);
            buysController._month = int.Parse(month);
            buysController._day = int.Parse(day);

            return RedirectToAction("Index", new { page = 1 });
        }

        // GET: buys
        [Route("~/buys")]
        [Route("~/buys/index")]
        [Route("~/buys/page{page}")]
        public ActionResult Index(int? page)
        {
            if (page == null)
            {
                buysController._year = 0;buysController._month = 0;buysController._day = 0;
            }
            int pageNumber = page ?? 1;
            if (pageNumber < 1) pageNumber = 1;
            int pageSize = 10;
            ViewBag.year = GetYearSelectList();
            ViewBag.month = GetMonthSelectList();
            ViewBag.day = GetDaySelectList();

            var buy = GetSelectedItemList();

            IPagedList<buy> buys = buy.OrderByDescending(p => p.id).ToPagedList(pageNumber, pageSize);
            return View(buys);
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

        // GET: buys/Create1
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
            ViewBag.supplier_id = new SelectList(db.supplier.OrderBy(x => x.name), "id", "name");

            return View();
        }


        // GET: buys/Create
        public ActionResult Create()
        {
            ViewBag.product_id = new SelectList(db.product.OrderBy(x=>x.pcode), "id", "pcode");
            ViewBag.supplier_id = new SelectList(db.supplier.OrderBy(x=>x.name), "id", "name");
            return View();
        }

        // POST: buys/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,product_id,supplier_id,value,qnt")] buy buy)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                buy.date = dt;
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
                        catch (Exception e)
                        {
                            tran.Rollback();
                            return RedirectToAction("DeleteUserSuccess", "Home", new { message = e.Message });
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
            ViewBag.product_id = new SelectList(db.product.OrderBy(x => x.pcode), "id", "pcode", buy.product_id);
            ViewBag.supplier_id = new SelectList(db.supplier.OrderBy(x => x.name), "id", "name", buy.supplier_id);

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
