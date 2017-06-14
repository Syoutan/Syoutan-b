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
        static private string _pcode="";
        static private int _maker_id = 0;
        static private int _category_id = 0;


        //Create Maker SelectList
        private SelectList GetMakerSelectList()
        {
            Dictionary<int, string> dic1 = new Dictionary<int, string>();
            dic1.Add(0, "なし");
            foreach (maker m in db.maker)
            {
                dic1.Add(m.id, m.name);
            }
            return new SelectList(dic1, "Key", "Value", productsController._maker_id.ToString());
        }

        //Create Category SelectList
        private SelectList GetCategorySelectList()
        {
            Dictionary<int, string> dic1 = new Dictionary<int, string>();
            dic1.Add(0, "なし");
            foreach (category c in db.category)
            {
                dic1.Add(c.id, c.name);
            }            
            return new SelectList(dic1, "Key", "Value", productsController._category_id.ToString());
        }

        // GET: products/Select
        public ActionResult Select(string pcode,string maker_id, string category_id)
        {
            productsController._pcode = pcode;
            try
            {
                productsController._maker_id = int.Parse(maker_id);
                productsController._category_id = int.Parse(category_id);
            }
            catch(Exception e)
            {
                productsController._maker_id = 0;
                productsController._category_id = 0;
                return RedirectToAction("DeleteUserSuccess", "Home", new { message = e.Message });
            }

            return RedirectToAction("Index",new { page = 1 });
        }

        //Get Index Select Item
        public IQueryable<product> GetSelectedItemList()
        {
            var pd = db.product.Include(p => p.category).Include(p => p.maker);
            if(productsController._pcode.Length > 0)
            {
                pd = pd.Where(x => x.pcode.StartsWith(productsController._pcode));
            }
            if (productsController._maker_id > 0)
            {
                pd = pd.Where(x => x.maker_id == productsController._maker_id);
            }
            if (productsController._category_id > 0)
            {
                pd = pd.Where(x => x.category_id == productsController._category_id);
            }

            return pd;
        }

        // GET: products
        [Route("~/products")]
        [Route("~/products/index")]
        [Route("~/products/page{page}")]
        public ActionResult Index(int? page)
        {
            if(page == null) { productsController._pcode = ""; productsController._maker_id = 0; productsController._category_id = 0; }
            int pageNumber = page ?? 1;
            if (pageNumber < 1) pageNumber = 1;
            int pageSize = 10;

            ViewBag.category_id = GetCategorySelectList();
            ViewBag.maker_id = GetMakerSelectList();
            ViewBag.pcode = productsController._pcode;
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
