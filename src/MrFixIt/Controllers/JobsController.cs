using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MrFixIt.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MrFixIt.Controllers
{
    public class JobsController : Controller
    {
        private MrFixItContext db = new MrFixItContext();

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(db.Jobs.Include(i => i.Worker).ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Job job)
        {
            db.Jobs.Add(job);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Claim(int id)
        {
            var thisItem = db.Jobs.FirstOrDefault(items => items.JobId == id);
            return View(thisItem);
        }

        [HttpPost]
        public IActionResult AClaim(Job job)
        {
           
            job.Worker = db.Workers.FirstOrDefault(i => i.UserName == User.Identity.Name);
            db.Entry(job).State = EntityState.Modified;
            db.SaveChanges();
            return Json(job);
        }

        [HttpPost]
        public IActionResult JobStatus(int JobId)
        {
            
            var thisJob = db.Jobs.FirstOrDefault(j => j.JobId == JobId);
            if(thisJob.Pending)
            {
                thisJob.Pending = false;
            }
            else
            {
                thisJob.Pending = true;
            }
            db.Entry(thisJob).State = EntityState.Modified;
            db.SaveChanges();
            return Json(thisJob);
        }

        [HttpPost]
        public IActionResult JobComplete(int JobId)
        {
            Debug.WriteLine("here is your ID " + JobId);
            var thisJob = db.Jobs.FirstOrDefault(j => j.JobId == JobId);
            if (thisJob.Completed)
            {
                thisJob.Completed = false;
            }
            else
            {
                thisJob.Completed = true;
            }
            db.Entry(thisJob).State = EntityState.Modified;
            db.SaveChanges();
            return Json(thisJob);
        }
    }
}
