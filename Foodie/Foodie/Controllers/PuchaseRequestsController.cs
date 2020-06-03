using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Foodie.Models;

namespace Foodie.Controllers
{
    public class PuchaseRequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PuchaseRequests
        public ActionResult Index(string searchString)
        {
            var Restaurants = db.PurchaseOffers.Where(p => p.OfferedRestaurant.Contains(searchString));
            if(!String.IsNullOrEmpty(searchString))
            {
                return View(Restaurants);
            }
            return View(Restaurants);
        }

        // GET: PuchaseRequests/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PuchaseRequest puchaseRequest = db.PuchaseRequests.Find(id);
            if (puchaseRequest == null)
            {
                return HttpNotFound();
            }
            return View(puchaseRequest);
        }

        // GET: PuchaseRequests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PuchaseRequests/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RestaurantName")] PuchaseRequest puchaseRequest)
        {
            if (ModelState.IsValid)
            {
                puchaseRequest.Id = Guid.NewGuid();
                db.PuchaseRequests.Add(puchaseRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(puchaseRequest);
        }

        // GET: PuchaseRequests/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PuchaseRequest puchaseRequest = db.PuchaseRequests.Find(id);
            if (puchaseRequest == null)
            {
                return HttpNotFound();
            }
            return View(puchaseRequest);
        }

        // POST: PuchaseRequests/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RestaurantName")] PuchaseRequest puchaseRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(puchaseRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(puchaseRequest);
        }

        // GET: PuchaseRequests/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PuchaseRequest puchaseRequest = db.PuchaseRequests.Find(id);
            if (puchaseRequest == null)
            {
                return HttpNotFound();
            }
            return View(puchaseRequest);
        }

        // POST: PuchaseRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PuchaseRequest puchaseRequest = db.PuchaseRequests.Find(id);
            db.PuchaseRequests.Remove(puchaseRequest);
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
