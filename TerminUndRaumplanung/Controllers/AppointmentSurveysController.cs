﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppData;
using AppData.Models;
using TerminUndRaumplanung.Models;
using Microsoft.AspNetCore.Identity;

namespace TerminUndRaumplanung.Controllers
{
    public class AppointmentSurveysController : Controller
    {
        private readonly AppointmentContext _context;
        private IAppointmentSurvey _survey;
        private readonly UserManager<ApplicationUser> _userManager;

        //public AppointmentSurveysController(AppointmentContext context, IAppointmentSurvey survey)
        //{
        //    _context = context;
        //    _survey = survey;
        //}

        public AppointmentSurveysController(AppointmentContext context, IAppointmentSurvey survey, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _survey = survey;
            _userManager = userManager;
        }

        // GET: AppointmentSurveys
        public async Task<IActionResult> Index()
        {
            return View(
                await _context
                    .AppointmentSurveys
                    .Include(s => s.Creator)
                    .Where(s => s.Creator.Id == _userManager.GetUserId(HttpContext.User))
                    .ToListAsync()
                );
        }


        // GET: AppointmentSurveys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var survey = await _context.AppointmentSurveys
                .Include(a => a.Creator)
                .SingleOrDefaultAsync(m => m.Id == id);

            //Simon
            var model = new SurveyDetailModel
            {
                SurveyId = survey.Id,
                Subject = survey.Subject,
                Creator = survey.Creator,
                Members = survey.Members,
                Appointments = _context
                                    .Appointments
                                    .Include(a => a.Room)
                                    .Where(a => a.Survey.Id == survey.Id)
            };

            return View(model);
        }

        // GET: AppointmentSurveys/Create
        public IActionResult Create()
        {
            var model = new AppointmentSurvey{ };

            //get current User from database
            var creator = _context
                    .ApplicationUsers
                    .Include(a => a.Surveys)
                    .FirstOrDefault(a => a.Id.Contains(_userManager.GetUserId(HttpContext.User)));
            //store entities in ViewBag for displaying in view
            ViewBag.Creator = creator;
            ViewBag.Creator.FirstName = creator.FirstName;
            ViewBag.Creator.LastName = creator.LastName;
            ViewBag.Creator.Id = creator.Id;
            return View();
        }

        // POST: AppointmentSurveys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Subject,Creator,Members")] AppointmentSurvey appointmentSurvey)
        {
            appointmentSurvey.Creator = _context
                    .ApplicationUsers
                    .Include(a => a.Surveys)
                    .FirstOrDefault(a => a.Id.Contains(_userManager.GetUserId(HttpContext.User)));

            var applicationUser = appointmentSurvey.Creator;
            applicationUser.Surveys.Add(appointmentSurvey);

            ModelState.Clear();
            TryValidateModel(appointmentSurvey);

            if (ModelState.IsValid)
            {
                _context.Add(appointmentSurvey);
                _context.Update(applicationUser);
                await _context.SaveChangesAsync();
                //redirect to the detail view of this survey
                return RedirectToAction("Details", "AppointmentSurveys", new { id = appointmentSurvey.Id });
            }
            return View(appointmentSurvey);
        }

        // GET: AppointmentSurveys/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentSurvey = await _context.AppointmentSurveys.SingleOrDefaultAsync(m => m.Id == id);
            if (appointmentSurvey == null)
            {
                return NotFound();
            }
            return View(appointmentSurvey);
        }

        // POST: AppointmentSurveys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Subject,Creator,Members")] AppointmentSurvey appointmentSurvey)
        {
            if (id != appointmentSurvey.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointmentSurvey);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentSurveyExists(appointmentSurvey.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(appointmentSurvey);
        }

        // GET: AppointmentSurveys/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentSurvey = await _context.AppointmentSurveys
                .SingleOrDefaultAsync(m => m.Id == id);
            if (appointmentSurvey == null)
            {
                return NotFound();
            }

            var creator = _context
                    .AppointmentSurveys
                    .Include(a => a.Creator)
                    .FirstOrDefault(a => a.Id == id)
                    .Creator;
            //store entities in ViewBag for displaying in view
            ViewBag.Creator = creator;
            ViewBag.Creator.FirstName = creator.FirstName;
            ViewBag.Creator.LastName = creator.LastName;
            ViewBag.Creator.Id = creator.Id;

            return View(appointmentSurvey);
        }

        // POST: AppointmentSurveys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointmentSurvey = await _context.AppointmentSurveys.SingleOrDefaultAsync(m => m.Id == id);
            _context.AppointmentSurveys.Remove(appointmentSurvey);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentSurveyExists(int id)
        {
            return _context.AppointmentSurveys.Any(e => e.Id == id);
        }
    }
}
