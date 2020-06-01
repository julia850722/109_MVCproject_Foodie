using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Foodie.Models;
using Microsoft.AspNet.Identity;

namespace Foodie.Controllers
{
    [Authorize]
    public class PurchaseOffersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PurchaseOffers
        public ActionResult Index(String Id)
        {
            if (Id == null)
            {
                return PartialView(db.PurchaseOffers.OrderByDescending(p => p.DeadLineTime).ToList());
                //return View(db.PurchaseOffers.ToList());
            }
            else
            {
                var username = User.Identity.Name;
                var user = db.Users.Where(p => p.UserName == username).FirstOrDefault();
                return PartialView(db.PurchaseOffers.Where(p => p.Seller.Id == user.Id).ToList());
            }
        }

        // GET: PurchaseOffers/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOffer purchaseOffer = db.PurchaseOffers.Find(id);
            if (purchaseOffer == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOffer);
        }

        // GET: PurchaseOffers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PurchaseOffers/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DeadLineTime,OfferedRestaurant,OfferTime")] PurchaseOffer purchaseOffer)
        {
            if (ModelState.IsValid)
            {
                string sellername = User.Identity.Name;
                var sellerid = User.Identity.GetUserId(); 
                purchaseOffer.Id = Guid.NewGuid();
                purchaseOffer.DeadLineTime = DateTime.Now.AddMinutes(purchaseOffer.OfferTime);
                purchaseOffer.SellerName = db.Users.Where(p => p.Id == sellerid).FirstOrDefault().UserName;
                db.PurchaseOffers.Add(purchaseOffer);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            return View(purchaseOffer);
        }

        // GET: PurchaseOffers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOffer purchaseOffer = db.PurchaseOffers.Find(id);
            if (purchaseOffer == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOffer);
        }

        // POST: PurchaseOffers/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DeadLineTime,OfferedRestaurant")] PurchaseOffer purchaseOffer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchaseOffer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create");
            }
            return View(purchaseOffer);
        }

        // GET: PurchaseOffers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOffer purchaseOffer = db.PurchaseOffers.Find(id);
            if (purchaseOffer == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOffer);
        }

        // POST: PurchaseOffers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PurchaseOffer purchaseOffer = db.PurchaseOffers.Find(id);
            db.PurchaseOffers.Remove(purchaseOffer);
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
