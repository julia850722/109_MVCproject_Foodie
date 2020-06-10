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
    public class PuchaseRequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PuchaseRequests
        public ActionResult Index(Guid? pId)
        {
            var Id = User.Identity.GetUserId();
            if (Id == null)
            {
                return PartialView(db.PuchaseRequests.OrderByDescending(p => p.Buyer.Id).ToList());

            }
            else
            {
                //var username = User.Identity.Name;
                //var user = db.Users.Where(p => p.UserName == username).FirstOrDefault();
                return PartialView(db.PuchaseRequests.Where(p => p.PurchaseOffersId == pId).ToList());
            }
            //return View(db.PuchaseRequests.ToList());
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
        public ActionResult Create(Guid? id)
        {
            ViewBag.purchaseOffersId = id;
            ViewBag.purchaseOffersSeller = db.PurchaseOffers.Where(s => s.Id == id).FirstOrDefault().Seller.UserName;
            ViewBag.purchaseOffersRestaurant = db.PurchaseOffers.Where(s => s.Id == id).FirstOrDefault().OfferedRestaurant;

            return View();
        }

        // POST: PuchaseRequests/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Content,PurchaseOffersId")] PuchaseRequest puchaseRequest)
        {
            if (ModelState.IsValid)
            {
                string Username = User.Identity.Name;
                var buyerId = User.Identity.GetUserId();
                puchaseRequest.Rname = db.PurchaseOffers.Where(s => s.Id == puchaseRequest.PurchaseOffersId).FirstOrDefault().OfferedRestaurant;
                puchaseRequest.Id = Guid.NewGuid();
                puchaseRequest.Buyer = db.Users.Where(p => p.Id == buyerId).FirstOrDefault();
                puchaseRequest.Seller = db.PurchaseOffers.Where(s => s.Id == puchaseRequest.PurchaseOffersId).FirstOrDefault().Seller;
                puchaseRequest.SubmittedTime = DateTime.Now;
                db.PuchaseRequests.Add(puchaseRequest);
                db.SaveChanges();
                return RedirectToAction("Create");
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

        public ActionResult CreateInfo(Guid? pId)
        {
            var Id = User.Identity.GetUserId();
            if (Id == null)
            {
                return PartialView(db.PuchaseRequests.OrderByDescending(p => p.Buyer.Id).ToList());

            }
            else
            {
                //var username = User.Identity.Name;
                //var user = db.Users.Where(p => p.UserName == username).FirstOrDefault();
                return PartialView(db.PuchaseRequests.Where(p => p.PurchaseOffersId == pId).ToList());
            }
            //return View(db.PuchaseRequests.ToList());
        }

        public ActionResult SellerInfo(Guid? pId)
        {
            var Id = User.Identity.GetUserId();
            if (Id == null)
            {
                return View(db.PuchaseRequests.OrderByDescending(p => p.Buyer.Id).ToList());

            }
            else
            {
                //var username = User.Identity.Name;
                //var user = db.Users.Where(p => p.UserName == username).FirstOrDefault();
                return View(db.PuchaseRequests.Where(p => p.PurchaseOffersId == pId).ToList());
            }
            //return View(db.PuchaseRequests.ToList());
        }


    }
}
