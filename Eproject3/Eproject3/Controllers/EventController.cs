using Microsoft.AspNetCore.Mvc;
using Eproject3.Data;
using Eproject3.Models.ViewModels;

namespace Eproject3.Controllers
{
    public class EventController : Controller
    {
        private readonly IDAL _dal;
        private readonly eProject3Context db;


        public EventController(IDAL dal, eProject3Context db) 
        {
            _dal = dal;
            this.db = db;

        }

        // GET: Event
        public IActionResult Index()
        {
            var model = _dal.GetEvents().ToList();
            if (TempData["Alert"] != null)
            {
                ViewData["Alert"] = TempData["Alert"];
            }
            return View(model);
        }

        // GET: Event/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = _dal.GetEvent((int)id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        //GET: Event/Create

        public IActionResult Creates()
        {
            return View(new EventViewModel(db.Labs.ToList()));
        }

        // POST: Event/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventViewModel vm, IFormCollection form)
        {
            try
            {
                _dal.CreateEvent(form);
                TempData["Alert"] = "Success! You created a new event for: " + form["Event.Name"];
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["Alert"] = "An error occurred: " + ex.Message;
                return View(vm);
            }
        }

        // GET: Event/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = _dal.GetEvent((int)id);
            if (@event == null)
            {
                return NotFound();
            }
            var vm = new EventViewModel(@event, db.Labs.Find((int)id));
            return View(vm);
        }

        // POST: Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection form)
        {
            try
            {
                _dal.UpdateEvent(form);
                TempData["Alert"] = "Success! You modified an event for: " + form["Event.Name"];
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["Alert"] = "An error occurred: " + ex.Message;
                var vm = new EventViewModel(_dal.GetEvent(id), db.Labs.Find(id));
                return View(vm);
            }
        }

        // GET: Event/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var @event = _dal.GetEvent((int)id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _dal.DeleteEvent(id);
            TempData["Alert"] = "You deleted an event.";
            return RedirectToAction(nameof(Index));
        }
    }
}