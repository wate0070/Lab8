using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab5.Models.DataAccess;
using Microsoft.AspNetCore.Http;


namespace Lab5.Controllers
{
    public class AcademicRecordsController : Controller
    {
        private readonly StudentRecordContext _context;

        public AcademicRecordsController(StudentRecordContext context)
        {
            _context = context;
        }

        // GET: AcademicRecords
        public async Task<IActionResult> Index(string? sorter)
        {
            string order = "";
            if (sorter == null)
            {
                sorter = "student";
            }
            if (HttpContext.Session.GetString("sorter") == sorter)
            {
                if(HttpContext.Session.GetString("order")==null || HttpContext.Session.GetString("order") == "desc")
                {
                    order = "asc";
                    HttpContext.Session.SetString("order", order);
                }
                else
                {
                    order = "desc";
                    HttpContext.Session.SetString("order", order);
                }
            }
            else
            {
                HttpContext.Session.SetString("sorter", sorter);
            }
            if(sorter == "grade")
            {
                IOrderedQueryable<AcademicRecord> set;
                if (order == "desc")
                {
                    set = _context.AcademicRecord.Include(a => a.CourseCodeNavigation).Include(i => i.Student).OrderByDescending(i => i.Grade);
                }
                else
                {
                    set = _context.AcademicRecord.Include(a => a.CourseCodeNavigation).Include(i => i.Student).OrderBy(i => i.Grade);
                }
                return View(await set.ToListAsync());
            }
            else if(sorter == "course")
            {
                IOrderedQueryable<AcademicRecord> set;
                if (order == "desc")
                {
                    set = _context.AcademicRecord.Include(a => a.CourseCodeNavigation).Include(i => i.Student).OrderByDescending(i => i.CourseCode);
                }
                else
                {
                    set = _context.AcademicRecord.Include(a => a.CourseCodeNavigation).Include(i => i.Student).OrderBy(i => i.CourseCode);
                }
                return View(await set.ToListAsync());
            }
            else
            {
               
                IOrderedQueryable<AcademicRecord> set;
                if (order == "desc")
                {
                    set = _context.AcademicRecord.Include(a => a.CourseCodeNavigation).Include(i => i.Student).OrderByDescending(i => i.StudentId);
                }
                else
                {
                    set = _context.AcademicRecord.Include(a => a.CourseCodeNavigation).Include(i => i.Student).OrderBy(i => i.StudentId);
                }
                return View(await set.ToListAsync());
            }
            var studentRecordContext = _context.AcademicRecord.Include(a => a.CourseCodeNavigation).Include(a => a.Student);
            return View(await studentRecordContext.ToListAsync());
        }

        // GET: AcademicRecords/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academicRecord = await _context.AcademicRecord
                .Include(a => a.CourseCodeNavigation)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (academicRecord == null)
            {
                return NotFound();
            }

            return View(academicRecord);
        }

        // GET: AcademicRecords/Create
        public IActionResult Create()
        {
            List<object> courses = new List<object>();
            foreach (Course course in _context.Course)
            {
                courses.Add(new { Code = course.Code, Title = course.Code + "-" + course.Title });
            }

            List<object> students = new List<object>();
            foreach (Student student in _context.Student)
            {
                students.Add(new { Id = student.Id, Name = student.Id + "-" + student.Name });
            }

            ViewData["CourseCode"] = new SelectList(courses, "Code", "Title");
            ViewData["StudentId"] = new SelectList(students, "Id", "Name");
            return View();
        }

        // POST: AcademicRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseCode,StudentId,Grade")] AcademicRecord academicRecord)
        {
            var selectedAccount = (from account in _context.AcademicRecord where account.StudentId == academicRecord.StudentId && account.CourseCode == academicRecord.CourseCode select account).FirstOrDefault();
            if (selectedAccount != null)
            {
                ModelState.AddModelError("", "The Student Already Exists Within The System.");
            }
            else {
                if (ModelState.IsValid)
                {
                    _context.Add(academicRecord);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            List<object> courses = new List<object>();
            foreach (Course course in _context.Course)
            {
                courses.Add(new { Code = course.Code, Title = course.Code + "-" + course.Title });
            }

            List<object> students = new List<object>();
            foreach (Student student in _context.Student)
            {
                students.Add(new { Id = student.Id, Name = student.Id + "-" + student.Name });
            }

          
            ViewData["CourseCode"] = new SelectList(courses, "Code", "Title", academicRecord.CourseCode);
            ViewData["StudentId"] = new SelectList(students, "Id", "Name", academicRecord.StudentId);
            return View(academicRecord);
        }

        // GET: AcademicRecords/Edit/5
        public async Task<IActionResult> Edit(string studentId, string courseCode)
        {
            if (studentId == null || courseCode == null)
            {
                return NotFound();
            }

            var academicRecord = _context.AcademicRecord
                .Include(a => a.CourseCodeNavigation)
                .Include(a => a.Student)
                .Where(a=> a.StudentId == studentId && a.CourseCode == courseCode)
                .FirstOrDefault();
            if (academicRecord == null)
            {
                return NotFound();
            }
            ViewData["CourseCode"] = academicRecord.CourseCode;
            ViewData["CourseTitle"] = academicRecord.CourseCodeNavigation.Title;
            ViewData["StudentId"] = academicRecord.StudentId;
            ViewData["StudentName"] = academicRecord.Student.Name;
            return View(academicRecord);
        }
        // POST: AcademicRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("CourseCode,StudentId,Grade")] AcademicRecord academicRecord)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(academicRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcademicRecordExists(academicRecord.StudentId))
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
            ViewData["CourseCode"] = new SelectList(_context.Course, "Code", "Code", academicRecord.CourseCode);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Id", academicRecord.StudentId);
            return View(academicRecord);
        }
        // Get Academic Records
        [HttpGet]
        public async Task<IActionResult> EditAll(string? sorter) 
        {
            if(sorter == null)
            {
                sorter = "student";
            }
            string order = "";
            if (HttpContext.Session.GetString("sorter") == sorter)
            {
                if (HttpContext.Session.GetString("order") == null || HttpContext.Session.GetString("order") == "desc")
                {
                    order = "asc";
                    HttpContext.Session.SetString("order", order);
                }
                else
                {
                    order = "desc";
                    HttpContext.Session.SetString("order", order);
                }
            }
            else
            {
                HttpContext.Session.SetString("sorter", sorter);
            }
            if (sorter == "grade")
            {
                IOrderedQueryable<AcademicRecord> set;
                if (order == "desc")
                {
                    set = _context.AcademicRecord.Include(a => a.CourseCodeNavigation).Include(i => i.Student).OrderByDescending(i => i.Grade);
                }
                else
                {
                    set = _context.AcademicRecord.Include(a => a.CourseCodeNavigation).Include(i => i.Student).OrderBy(i => i.Grade);
                }
                return View(await set.ToArrayAsync());
            }
            else if (sorter == "course")
            {
                IOrderedQueryable<AcademicRecord> set;
                if (order == "desc")
                {
                    set = _context.AcademicRecord.Include(a => a.CourseCodeNavigation).Include(i => i.Student).OrderByDescending(i => i.CourseCode);
                }
                else
                {
                    set = _context.AcademicRecord.Include(a => a.CourseCodeNavigation).Include(i => i.Student).OrderBy(i => i.CourseCode);
                }
                return View(await set.ToArrayAsync());
            }
            else
            {
             
                IOrderedQueryable<AcademicRecord> set;
                if (order == "desc")
                {
                    set = _context.AcademicRecord.Include(a => a.CourseCodeNavigation).Include(i => i.Student).OrderByDescending(i => i.StudentId);
                }
                else
                {
                    set = _context.AcademicRecord.Include(a => a.CourseCodeNavigation).Include(i => i.Student).OrderBy(i => i.StudentId);
                }
                return View(await set.ToArrayAsync());
            }
            var list = _context.AcademicRecord.Include(a => a.CourseCodeNavigation).Include(a => a.Student);
            return View(await list.ToArrayAsync());
        }
        //Post Academic Records
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAll(AcademicRecord[] academicRecords)
        {
            if(ModelState.IsValid)
                {
                foreach(AcademicRecord academicrecord in academicRecords)
                {
                    _context.Update(academicrecord);
                    await _context.SaveChangesAsync();
                }
                
                }
            return Redirect("/AcademicRecords");
        }

       

        // GET: AcademicRecords/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academicRecord = await _context.AcademicRecord
                .Include(a => a.CourseCodeNavigation)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (academicRecord == null)
            {
                return NotFound();
            }

            return View(academicRecord);
        }

        // POST: AcademicRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var academicRecord = await _context.AcademicRecord.FindAsync(id);
            _context.AcademicRecord.Remove(academicRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AcademicRecordExists(string id)
        {
            return _context.AcademicRecord.Any(e => e.StudentId == id);
        }
    }
}
