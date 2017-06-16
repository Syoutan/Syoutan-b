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
    public class invoice_detailController : Controller
    {
        private ProfitManagement1Entities4 db = new ProfitManagement1Entities4();
        private ProductManage1Entities1 db2 = new ProductManage1Entities1();

        //Create Year SelectList
        private SelectList GetYearSelectList()
        {
            Dictionary<int, string> dic1 = new Dictionary<int, string>();
            dic1.Add(0, "");
            foreach (int y in Enumerable.Range(2000, 51))
            {
                dic1.Add(y, y.ToString());
            }
            return new SelectList(dic1, "Key", "Value", (int)DateTime.Now.Year);
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
            return new SelectList(dic1, "Key", "Value", (int)DateTime.Now.Month);
        }

        //Create Customer SelectList
        private SelectList GetCustomerSelectList()
        {
            Dictionary<int, string> dic1 = new Dictionary<int, string>();
            dic1.Add(0, "なし");
            var cut = db2.customer.OrderBy(x => x.name);
            foreach (customer c in cut)
            {
                dic1.Add(c.id, c.name);
            }
            return new SelectList(dic1, "Key", "Value", 0);
        }

        public bool NoWDelete(int iyear,int imonth)
        {
            using (var tran1 = db.Database.BeginTransaction())
            {
                try
                {
                    var invdl = db.invoice_detail.Where(x => x.date.Year == iyear).Where(x => x.date.Month == imonth);
                    foreach (var invd in invdl)
                    {
                        db.invoice_detail.Remove(invd);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    tran1.Rollback();
                    return false;
                }
                try
                {
                    var invl = db.invoice.Where(x => x.date.Year == iyear).Where(x => x.date.Month == imonth);
                    foreach (var inv in invl)
                    {
                        db.invoice.Remove(inv);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    tran1.Rollback();
                    return false;
                }
                tran1.Commit();
            }
            return true;
        }

        public bool NowCreate(int iyear, int imonth)
        {
            using (var tran1 = db.Database.BeginTransaction())
            {
                using (var tran2 = db2.Database.BeginTransaction())
                {
                    try
                    {
                        DateTime dt = new DateTime(iyear, imonth, 1);
                        var customers = db2.customer;
                        foreach (customer cu in customers)
                        {
                            invoice ninv = new invoice() { customer_id = cu.id, date = dt.Date, cname = cu.name };
                            db.invoice.Add(ninv);
                            db.SaveChanges();
                        }
                    }
                    catch(Exception e)
                    {
                        tran1.Rollback();
                        tran2.Rollback();
                        return false;
                    }
                    try
                    {
                        var salesl = db2.sale.Where(x => x.date.Year == iyear).Where(x => x.date.Month == imonth);
                        foreach (var sales in salesl)
                        {
                            var prd = db2.product.Find(sales.product_id);
                            var inv = db.invoice.Where(x => x.customer_id == sales.customer_id).Where(x=>x.date.Year == iyear).Where(x=>x.date.Month == imonth).First();
                            invoice_detail ninvd = new invoice_detail() { sale_id = sales.id, invoice_id = inv.invoice_no, pcode = prd.pcode, pname = prd.name, qnt = sales.qnt, value = sales.value, small_sum = sales.value * sales.qnt,date=sales.date.Date };
                            db.invoice_detail.Add(ninvd);
                            db.SaveChanges();
                        }
                        tran1.Commit();
                        tran2.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        tran1.Rollback();
                        tran2.Rollback();
                        return false;
                    }

                }
            }

        }


        // GET: invoice_detail
        public ActionResult Index(int? customer_id,int? iyear,int? imonth)
        {
            try
            {
                customer_id = customer_id ?? db.invoice.First().customer_id;
                iyear = iyear ?? DateTime.Now.Year;
                imonth = imonth ?? DateTime.Now.Month;
                var inv = db.invoice.Where(x => x.customer_id == customer_id).Where(x => x.date.Year == iyear).Where(x => x.date.Month == imonth).First();
                var invoice_detail = db.invoice_detail.Where(x => x.invoice_id == inv.invoice_no);
                ViewBag.customer_id = GetCustomerSelectList();
                ViewBag.iyear = GetYearSelectList();
                ViewBag.imonth = GetMonthSelectList();
                return View(invoice_detail.ToList());
            }
            catch(Exception e)
            {
                return RedirectToAction("DeleteUserSuccess", "Home", new { message = "表示エラー：検索データがない可能性があります" });
            }
        }

        // GET: invoice_detail/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            invoice_detail invoice_detail = db.invoice_detail.Find(id);
            if (invoice_detail == null)
            {
                return HttpNotFound();
            }
            return View(invoice_detail);
        }

        // GET: invoice_detail/Create
        public ActionResult Create()
        {
            ViewBag.year = GetYearSelectList();
            ViewBag.month = GetMonthSelectList();

            return View();
        }

        // POST: invoice_detail/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string customer_id,string year,string month)
        {
            int iyear = int.Parse(year);
            int imonth = int.Parse(month);

            if(NoWDelete(iyear, imonth) == false)
            {
                return RedirectToAction("DeleteUserSuccess", "Home", new { message = "削除エラー"});
            }
            if(NowCreate(iyear,imonth)==false)
            {
                return RedirectToAction("DeleteUserSuccess", "Home", new { message = "作成エラー" });
            }
            return RedirectToAction("Index");
        }

        // GET: invoice_detail/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            invoice_detail invoice_detail = db.invoice_detail.Find(id);
            if (invoice_detail == null)
            {
                return HttpNotFound();
            }
            ViewBag.invoice_detail_no = new SelectList(db.invoice, "invoice_no", "invoice_no", invoice_detail.invoice_detail_no);
            return View(invoice_detail);
        }

        // POST: invoice_detail/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "invoice_detail_no,buy_id,pcode,pname,qnt,value,small_sum,invoice_id")] invoice_detail invoice_detail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice_detail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.invoice_detail_no = new SelectList(db.invoice, "invoice_no", "invoice_no", invoice_detail.invoice_detail_no);
            return View(invoice_detail);
        }

        // GET: invoice_detail/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            invoice_detail invoice_detail = db.invoice_detail.Find(id);
            if (invoice_detail == null)
            {
                return HttpNotFound();
            }
            return View(invoice_detail);
        }

        // POST: invoice_detail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            invoice_detail invoice_detail = db.invoice_detail.Find(id);
            db.invoice_detail.Remove(invoice_detail);
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
