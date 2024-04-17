using AdminGA.Data;
using AdminGA.Entity.Student;
using AdminGA.Models.Student;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminGA.Controllers
{
    public class MainAdmissionController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        // Constructor to inject the DbContext
        public MainAdmissionController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ActionResult Index()
        {
            var (classes, mediums, preAdmissions) = GetData();
            SetViewData(classes, mediums, preAdmissions);
            return View();
        }
        // GET: MainAdmissionController
        public ActionResult FromPreAdmission(MainAdmissionViewModel model)
        {
            
            return View(model);
        }

        // GET: MainAdmissionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MainAdmissionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MainAdmissionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MainAdmissionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult PreAdmissionDetails(string uniqueSeries)
        {
            // Retrieve data based on the uniqueSeries
            var data = _dbContext.GA_STDPREADMISSION.FirstOrDefault(x => x.STD_UNIQUECODE == uniqueSeries);
            if (data == null)
            {
                // Handle case where data is not found
                return Json(new { error = "Data not found" }); // Return JSON with error message
            }

            // Populate a view model with the retrieved data
            var model = new MainAdmissionViewModel
            {
                FirstName = data.STD_FNAME,
                MiddleName = data.STD_FANAME,
                LastName = data.STD_LNAME,
                ClassID = data.STD_CLID,
                MediumID = data.STD_MEDID,
                MobileNumber = data.STD_MOBILENO,
                Fees = (int)data.STD_AFEES
            };

            var (classes, mediums, preAdmissions) = GetData();
            SetViewData(classes, mediums, preAdmissions);
            ViewBag.MainAdmissionViewModel = model;

            // Return JSON with the populated view model
            return View("FromPreAdmission", model);
        }



        private (List<GA_STDCLASS>, List<GA_STDMEDIUM>, List<GA_STDPREADMISSION>) GetData()
        {
            var classes = _dbContext.GA_STDCLASS.ToList();
            var mediums = _dbContext.GA_STDMEDIUM.ToList();
            var preAdmissions = _dbContext.GA_STDPREADMISSION
                 .OrderByDescending(x => x.STD_ENTRYDATE)
                 .ToList();

            return (classes, mediums, preAdmissions);
        }

        private void SetViewData(List<GA_STDCLASS> classes, List<GA_STDMEDIUM> mediums, List<GA_STDPREADMISSION> preAdmissions)
        {
            ViewBag.Classes = classes;
            ViewBag.Mediums = mediums;
            ViewBag.PreAdmissions = preAdmissions;
        }
    }
}
