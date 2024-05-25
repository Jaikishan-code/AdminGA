using AdminGA.Data;
using AdminGA.Entity.Student;
using AdminGA.Models.Student;
using AdminGA.Validation.Student;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminGA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PreAdmissionController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public PreAdmissionController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ActionResult Index()
        {
            var (classes, mediums, preAdmissions) = GetData();
            SetViewData(classes, mediums, preAdmissions);
            return View();
        }

        // GET: PreAdmissionController/Details/5
        public ActionResult Details(int id)
        {
            var studentDetails = GetStudentDetailsFromDataSource(id);

            return Json(studentDetails);
        }

        // GET: PreAdmissionController/Create
        public ActionResult Create()
        {
            var (classes, mediums, preAdmissions) = GetData();
            SetViewData(classes, mediums, preAdmissions);
            return View();
        }

        // POST: PreAdmissionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PreadmissionModel student)
        {
            // Validate the student model using FluentValidation
            var validationErrors = ValidateStudent(student);

            // If validation fails, return validation errors
            if (validationErrors.Any())
                return Json(new { isValid = false, validationErrors });

            try
            {
                // Process unique series for the selected class
                var (unqSeriesInfo, uniqueCode) = ProcessUniqueSeries(student.ClassID);

                // Get class and medium information
                var (selectedClass, selectedMedium) = GetClassAndMedium(student);

                // Create a new student entity
                var newStudent = CreateNewStudentEntity(student, uniqueCode, selectedClass, selectedMedium, student.ClassID, student.MediumID);

                // Add the new student to the database and save changes
                _dbContext.GA_STDPREADMISSION.Add(newStudent);
                _dbContext.SaveChanges();

                // Return success and unique code if admission is processed successfully
                return Json(new { isValid = true, uniqueCode });
            }
            catch (Exception ex)
            {
                // Return an error message if an exception occurs during admission processing
                return Json(new { isValid = false, errorMessage = "An error occurred while processing the admission." });
            }
        }        

        [HttpGet]
        public IActionResult GetRecordUniqueSeries(string uniqueCode)
        {
            try
            {
                // Fetch the student record based on the unique code
                var student = _dbContext.GA_STDPREADMISSION
                    .FirstOrDefault(x => x.STD_UNIQUECODE == uniqueCode);

                if (student != null)
                {
                    // Return relevant data, including status ID
                    return Json(new
                    {
                        stsId = student.STD_STSID,
                        studentData = new
                        {
                            name = $"{student.STD_FNAME} {student.STD_LNAME}",
                            uniqueCode = student.STD_UNIQUECODE,
                            className = student.STD_CLASS,
                            medium = student.STD_MEDIUM,
                            fees = student.STD_AFEES,
                            mobileNumber = student.STD_MOBILENO,
                            entryDate = student.STD_ENTRYDATE.ToString("dd-MMM-yy")
                            // Add more properties as needed
                        }
                    });
                }
                else
                {
                    // Handle the case when the student record is not found
                    return Json(new { error = "Student not found." });
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error message
                return Json(new { error = $"Error occurred: {ex.Message}" });
            }
        }
        private IQueryable<GA_STDPREADMISSION> ApplySearchConditions(IQueryable<GA_STDPREADMISSION> query, string studentCode, string studentName, string phone, Guid selectedClass, int feesStatus)
        {
            var Class = _dbContext.GA_STDCLASS
                .Where(c => c.STD_CLID == selectedClass)
                .Select(c => c.STD_CLNAME)
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(studentCode))
                query = query.Where(x => x.STD_UNIQUECODE.Contains(studentCode));

            if (!string.IsNullOrEmpty(studentName))
                query = query.Where(x => (x.STD_FNAME + " " + x.STD_LNAME).Contains(studentName));

            if (!string.IsNullOrEmpty(phone))
                query = query.Where(x => x.STD_MOBILENO.Contains(phone));

            if (!string.IsNullOrEmpty(Class))
                query = query.Where(x => x.STD_CLASS.ToUpper() == Class.ToUpper());

            if (feesStatus > 0)
                query = query.Where(x => x.STD_STSID == feesStatus);

            return query;
        }

        public JsonResult GetMediumsByClass(Guid classId)
        {
            var className = _dbContext.GA_STDCLASS
                                .Where(x => x.STD_CLID == classId)
                                .Select(b => b.STD_CLNAME.Trim().ToLower())
                                .FirstOrDefault();

            bool is11Or12 = className == "11th std" || className == "12th std";

            // Fetch all mediums
            var mediums = _dbContext.GA_STDMEDIUM.ToList();

            if (!is11Or12)
            {

                mediums = mediums.Where(m => m.MED_MEDIUM.ToLower() != "commerce" &&
                                              m.MED_MEDIUM.ToLower() != "science" &&
                                              m.MED_MEDIUM.ToLower() != "sp" &&
                                              m.MED_MEDIUM.ToLower() != "math").ToList();
            }
            else
            {
                mediums = mediums.Where(m => m.MED_MEDIUM.ToLower() != "hindi" &&
                                              m.MED_MEDIUM.ToLower() != "english" &&
                                              m.MED_MEDIUM.ToLower().Trim() != "semi english").ToList();
            }



            return Json(mediums);
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

        private List<string> ValidateStudent(PreadmissionModel student)
        {
            PreadmissionModelValidator validator = new PreadmissionModelValidator();
            ValidationResult result = validator.Validate(student);
            return result.Errors.Select(e => e.ErrorMessage).ToList();
        }
        private (dynamic, string) ProcessUniqueSeries(Guid selectedClass)
        {
            dynamic unqSeriesInfo = _dbContext.GA_STDUNQSERIES
                .Where(s => s.UNQ_STDCLID == selectedClass)
                .FirstOrDefault()
                ?? throw new InvalidOperationException("Unique series information not found for the selected class.");

            unqSeriesInfo.UNQ_SERIES++;
            var uniqueCode = $"{unqSeriesInfo.UNQ_CODE}{unqSeriesInfo.UNQ_SERIES}";

            return (unqSeriesInfo, uniqueCode);
        }
        private (string, string) GetClassAndMedium(PreadmissionModel student)
        {
            var selectedClass = _dbContext.GA_STDCLASS
                                  .Where(c => c.STD_CLID == student.ClassID)
                                  .Select(c => c.STD_CLNAME)
                                  .FirstOrDefault();

            var selectedMedium = _dbContext.GA_STDMEDIUM
                                  .Where(c => c.MED_MID == student.MediumID)
                                  .Select(c => c.MED_MEDIUM)
                                  .FirstOrDefault();

            return (selectedClass, selectedMedium);
        }

        private GA_STDPREADMISSION CreateNewStudentEntity(PreadmissionModel student, string uniqueCode, string selectedClass, string selectedMedium, Guid ClassId, Guid MediumId)
        {
            return new GA_STDPREADMISSION
            {
                STD_FNAME = student.FirstName,
                STD_LNAME = student.LastName,
                STD_AFEES = student.Fees,
                STD_FANAME = student.MiddleName,
                STD_MOBILENO = student.MobileNumber,
                STD_MEDIUM = selectedMedium,
                STD_STSID = 10,
                STD_ENTRYDATE = DateTime.Now,
                STD_CLASS = selectedClass,
                STD_UNIQUECODE = uniqueCode,
                STD_CLID = ClassId,
                STD_MEDID = MediumId
            };
        }
        
        private PreadmissionModel GetStudentDetailsFromDataSource(int studentId)
        {
            var studentEntity = _dbContext.GA_STDPREADMISSION
           .Where(s => s.STD_ID == studentId)
           .FirstOrDefault();
            if (studentEntity != null)
            {


                var studentDetails = new PreadmissionModel();

                studentDetails.FirstName = studentEntity.STD_FNAME;
                studentDetails.MiddleName = studentEntity.STD_FANAME;
                studentDetails.LastName = studentEntity.STD_LNAME;
                studentDetails.MediumID = studentEntity.STD_MEDID;
                studentDetails.ClassID = studentEntity.STD_CLID;
                studentDetails.MobileNumber = studentEntity.STD_MOBILENO;
                studentDetails.Fees = (int)studentEntity.STD_AFEES;
                studentDetails.PreAdmissionCode = studentEntity.STD_UNIQUECODE;

                var (selectedClass, selectedMedium) = GetClassAndMedium(studentDetails);
                studentDetails.Class = selectedClass;
                studentDetails.Medium = selectedMedium;


                return studentDetails;
            }
            return null;

        }


    }
}
