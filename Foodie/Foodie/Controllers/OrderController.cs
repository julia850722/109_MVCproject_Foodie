using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AngleSharp.Common;
using Foodie.Models;
using Microsoft.AspNet.Identity;

namespace Foodie.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Order
        public ActionResult Index()
        {
            //return View(db.OrderModels.OrderBy(p=>p.Created).ToList());

            var username = User.Identity.Name;
            var Id = User.Identity.GetUserId();

            if (Id == null || username == "mc@mc.com")
            {
                return PartialView(db.OrderModels.OrderByDescending(p => p.Created).ToList());
            }
            else
            {
                var user = db.Users.Where(p => p.UserName == username).FirstOrDefault();
                return PartialView(db.OrderModels.Where(p => p.User.UserName == username).OrderByDescending(p => p.Created).ToList());
            }
        }

        // GET: Order/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderModels orderModels = db.OrderModels.Find(id);
            if (orderModels == null)
            {
                return HttpNotFound();
            }
            return View(orderModels);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Meal,ABC,Number,Price,Tips,Created,EndFlag")] OrderModels orderModels, string item)
        {
            ViewBag.item = item;
            if (ModelState.IsValid)
            {
                int total_Price = 0;
                int UnitPrice = 0;
                string meal = orderModels.Meal;
                string username = User.Identity.Name;
                orderModels.User = db.Users.Where(p => p.UserName == username).FirstOrDefault();
                orderModels.Created = DateTime.Now;
                
                var query = from b in db.MenuModels
                            select b;
                foreach (var UnitItem in query)
                {
                    if (UnitItem.ProductName == meal)
                    {
                        UnitPrice = UnitItem.UnitPrice;
                        break;
                    }
                }

                if (item == "A")//+套餐
                {
                    orderModels.ABC = "A";
                    total_Price = (UnitPrice + 55) * orderModels.Number;
                }
                else if (item == "B")
                {
                    orderModels.ABC = "B";
                    total_Price = (UnitPrice + 55) * orderModels.Number;
                }
                else if (item == "C")
                {
                    orderModels.ABC = "C";
                    total_Price = (UnitPrice + 65) * orderModels.Number;
                }
                else if (item == "D")
                {
                    orderModels.ABC = "D";
                    total_Price = (UnitPrice + 68) * orderModels.Number;
                }
                else if (item == "E")
                {
                    orderModels.ABC = "E";
                    total_Price = (UnitPrice + 79) * orderModels.Number;
                }
                else if (item == "")
                {
                    total_Price = UnitPrice * orderModels.Number;
                }

                orderModels.Price = total_Price;
                orderModels.Id = Guid.NewGuid();
                db.OrderModels.Add(orderModels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(orderModels);
        }

        // GET: Order/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderModels orderModels = db.OrderModels.Find(id);
            if (orderModels == null)
            {
                return HttpNotFound();
            }
            return View(orderModels);
        }

        // POST: Order/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Meal,ABC,Number,Price,Tips,Created,EndFlag")] OrderModels orderModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create");
            }
            return View(orderModels);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderModels orderModels = db.OrderModels.Find(id);
            if (orderModels == null)
            {
                return HttpNotFound();
            }
            return View(orderModels);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            OrderModels orderModels = db.OrderModels.Find(id);
            db.OrderModels.Remove(orderModels);
            db.SaveChanges();
            return RedirectToAction("Create");
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
