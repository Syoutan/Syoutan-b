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


        //Create Maker SelectList
        private SelectList GetMakerSelectList()
        {
            Dictionary<int, string> dic1 = new Dictionary<int, string>();
            dic1.Add(0, "なし");
            var mks = db.maker.OrderBy(x => x.name);
            foreach (maker m in mks)
            {
                dic1.Add(m.id, m.name);
            }
            return new SelectList(dic1, "Key", "Value", ((int)Session["maker_id"]).ToString());
        }

        //Create Category SelectList
        private SelectList GetCategorySelectList()
        {
            Dictionary<int, string> dic1 = new Dictionary<int, string>();
            dic1.Add(0, "なし");
            var cgs = db.category.OrderBy(x => x.name);
            foreach (category c in cgs)
            {
                dic1.Add(c.id, c.name);
            }
            return new SelectList(dic1, "Key", "Value", ((int)Session["category_id"]).ToString());
        }

        //Create Value1 SelectList
        private SelectList GetValue1List()
        {
            Dictionary<decimal, string> dic1 = new Dictionary<decimal, string>(){
                { 0,"0" },{ 10000, "10000" },{ 50000,"50000" }, { 100000,"100000" },{ 200000,"200000" },{ 300000,"300000" }};

            return new SelectList(dic1, "Key", "Value", ((decimal)Session["value1"]).ToString());
        }
        //Create Value2 SelectList
        private SelectList GetValue2List()
        {
            Dictionary<decimal, string> dic1 = new Dictionary<decimal, string>(){
                { 0,"" },{ 10000, "10000" },{ 50000,"50000" }, { 100000,"100000" },{ 200000,"200000" },{ 300000,"300000" },{ 500000,"500000" }};

            return new SelectList(dic1, "Key", "Value", ((decimal)Session["value2"]).ToString());
        }



        // POST: products/Select
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Select(string pcode,string name,string maker_id, string category_id,string value1,string value2)
        {
            Session["pcode"] = pcode;
            Session["name"] = name;
            try
            {
                Session["maker_id"] = int.Parse(maker_id);
                Session["category_id"] = int.Parse(category_id);
                Session["value1"] = decimal.Parse(value1);
                Session["value2"] = decimal.Parse(value2);
            }
            catch(Exception e)
            {
                Session["maker_id"] = 0;
                Session["category_id"] = 0;
                Session["value1"] = 0;
                Session["value2"] = 0;

                return RedirectToAction("DeleteUserSuccess", "Home", new { message = e.Message });
            }

            return RedirectToAction("Index",new { page = 1 });
        }

        //Get Index Select Item
        public IQueryable<product> GetSelectedItemList()
        {
            IQueryable<product> pd = db.product.Include(p => p.category).Include(p => p.maker);
            if(((string)Session["pcode"]).Length > 0)
            {
                string pcode = (string)Session["pcode"];
                pd = pd.Where(x => x.pcode.StartsWith(pcode));
            }
            if (((string)Session["name"]).Length > 0)
            {
                string name = (string)Session["name"];
                pd = pd.Where(x => x.name.Contains(name));
            }
            if ((int)Session["maker_id"] > 0)
            {
                int mid = (int)Session["maker_id"];
                pd = pd.Where(x => x.maker_id == mid);
            }
            if ((int)Session["category_id"] > 0)
            {
                int cid = (int)Session["category_id"];
                pd = pd.Where(x => x.category_id == cid);
            }
            if((decimal)Session["value1"] > 0)
            {
                decimal v1 = (decimal)Session["value1"];
                pd = pd.Where(x => x.value >= v1);
            }
            if ((decimal)Session["value2"] > 0)
            {
                decimal v2 = (decimal)Session["value2"];
                pd = pd.Where(x => x.value <= v2);
            }
            return pd;
        }

        // GET: products
        [Route("~/products")]
        [Route("~/products/index")]
        [Route("~/products/page{page}")]
        public ActionResult Index(int? page)
        {
            if (page == null)
            {
                Session["pcode"] = ""; Session["name"] = "";
                Session["maker_id"] = 0; Session["category_id"] = 0; Session["value1"] = (decimal)0; Session["value2"] = (decimal)0;
            }

            int pageNumber = page ?? 1;
            if (pageNumber < 1) pageNumber = 1;
            int pageSize = 7;

            ViewBag.category_id = GetCategorySelectList();
            ViewBag.maker_id = GetMakerSelectList();
            ViewBag.pcode = (string)Session["pcode"];
            ViewBag.name = (string)Session["name"];
            ViewBag.value1 = GetValue1List();
            ViewBag.value2 = GetValue2List();


            var pd = GetSelectedItemList();

            IPagedList<product> products = pd.OrderBy(p => p.pcode).ToPagedList(pageNumber, pageSize);
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
                    return RedirectToAction("Index",new { page = 1 });
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
                    return RedirectToAction("Index",new { page = 1 });
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
                return RedirectToAction("Index",new { page = 1 });
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
