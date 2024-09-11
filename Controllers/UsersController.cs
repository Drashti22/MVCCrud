using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcPractical.Models;

namespace MvcPractical.Controllers
{
    public class UsersController : Controller
    {
        private MvcPracticalEntities db = new MvcPracticalEntities();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Country).Include(u => u.State);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        //Get states by selected country id
        public JsonResult GetStatesByCountryId(int countryId)
        {
            var states = db.States.Where(s => s.CountryId == countryId).ToList();
            var statesList = states.Select(s => new SelectListItem { Value = s.ID.ToString(), Text = s.StateName });
            return Json(statesList, JsonRequestBehavior.AllowGet);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            var countries = db.Countries.ToList();
            var countryList = new List<SelectListItem>
            {
                new SelectListItem { Value = "-1", Text = "Select Country", Disabled = true, Selected= true }
            };
            countryList.AddRange(countries.Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.CountryName
            }));

            var knownLanguages = new List<SelectListItem>
            {
                new SelectListItem { Value = "English", Text = "English" },
                new SelectListItem { Value = "Hindi", Text = "Hindi" },
                new SelectListItem {Value = "Gujarati", Text = "Gujarati"}
            };
            ViewBag.CountryList = countryList;
            ViewBag.KnownLanguages = knownLanguages;
            //ViewBag.StateList = new SelectList(db.States.Where(s => s.CountryId == user.CountryId).ToList(), "ID", "StateName", user.StateId);
            ViewBag.StateList = new SelectList(Enumerable.Empty<SelectListItem>(), "Value", "Text");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user, HttpPostedFileBase Photo)
        {
            var existingUser = db.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email already exists. Please choose a different email.");
                ViewBag.KnownLanguages = new List<SelectListItem>
            {
                new SelectListItem { Value = "English", Text = "English" },
                new SelectListItem { Value = "Hindi", Text = "Hindi" },
                new SelectListItem {Value = "Gujarati", Text = "Gujarati"}
            };

                ViewBag.CountryId = new SelectList(db.Countries, "ID", "CountryName", user.CountryId);
                ViewBag.StateList = new SelectList(db.States.Where(s => s.CountryId == user.CountryId).ToList(), "ID", "StateName", user.StateId);
                if (Photo != null && Photo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Photo.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                    Photo.SaveAs(path);
                    user.PhotoUrl = "~/Uploads/" + fileName; // Save the file URL in the database
                    ModelState["PhotoUrl"].Errors.Clear();
                }
                return View(user);
            }
            if (Photo != null && Photo.ContentLength > 0)
            {
                var fileName = Path.GetFileName(Photo.FileName);
                var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                Photo.SaveAs(path);
                user.PhotoUrl = "~/Uploads/" + fileName; // Save the file URL in the database
                ModelState["PhotoUrl"].Errors.Clear();
            }
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KnownLanguages = new List<SelectListItem>
            {
                new SelectListItem { Value = "English", Text = "English" },
                new SelectListItem { Value = "Hindi", Text = "Hindi" },
                new SelectListItem {Value = "Gujarati", Text = "Gujarati"}
            };

            ViewBag.CountryId = new SelectList(db.Countries, "ID", "CountryName", user.CountryId);
            //ViewBag.StateId = new SelectList(db.States, "ID", "StateName", user.StateId);
            ViewBag.StateList = new SelectList(db.States.Where(s => s.CountryId == user.CountryId).ToList(), "ID", "StateName", user.StateId);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.KnownLanguages = new List<SelectListItem>
            {
                new SelectListItem { Value = "English", Text = "English" },
                new SelectListItem { Value = "Hindi", Text = "Hindi" },
                new SelectListItem { Value = "Gujarati", Text = "Gujarati" }
            };
            ViewBag.CountryList = new SelectList(db.Countries.ToList(), "ID", "CountryName");
            ViewBag.StateList = new SelectList(db.States.Where(s => s.CountryId == user.CountryId).ToList(), "ID", "StateName", user.StateId);
            user.Gender = user.Gender.Trim();
            //user.PhotoUrl = user.PhotoUrl != null ? user.PhotoUrl : CurrentPhotoUrl;
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        private void CheckPhoto(User user, HttpPostedFileBase Photo)
        {
            //if (Photo != null && Photo.ContentLength > 0)
            //{
            //    var fileName = Path.GetFileName(Photo.FileName);
            //    var path = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
            //    Photo.SaveAs(path);

            //    user.PhotoUrl = "~/Uploads/" + fileName;
            //    ModelState["PhotoUrl"].Errors.Clear();
            //}

            if (user.ImageFile != null && user.ImageFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(user.ImageFile.FileName);
                var path = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                user.ImageFile.SaveAs(path);

                user.PhotoUrl = "~/Uploads/" + fileName;
                ModelState["PhotoUrl"].Errors.Clear();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user, HttpPostedFileBase PhotoUrl)
        {
            CheckPhoto(user, PhotoUrl);

            ViewBag.KnownLanguages = new List<SelectListItem>
            {
                new SelectListItem { Value = "English", Text = "English" },
                new SelectListItem { Value = "Hindi", Text = "Hindi" },
                new SelectListItem { Value = "Gujarati", Text = "Gujarati" }
            };

            ViewBag.CountryId = new SelectList(db.Countries, "ID", "CountryName", user.CountryId);
            ViewBag.StateList = new SelectList(db.States.Where(s => s.CountryId == user.CountryId).ToList(), "ID", "StateName", user.StateId);

            if (ModelState.IsValid)
            {
                var existingUser = db.Users.AsNoTracking().FirstOrDefault(u => u.ID == user.ID);
                var existingEmail = db.Users.FirstOrDefault(u => u.Email == user.Email && u.ID != user.ID);
                if (existingEmail != null)
                {
                    ModelState.AddModelError("Email", "Email already exists. Please choose a different email.");
                    return View(user);
                }
                else
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(user);
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
