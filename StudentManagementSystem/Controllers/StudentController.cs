using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace StudentManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        // GET: StudentController
        private readonly StudentDbContext _context = new StudentDbContext();

        public StudentController(StudentDbContext context)
        {
            _context = context;
        }
        public ActionResult Index(int pg = 1)
        {
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = _context.Students.Count();
            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            List<Student> students = _context.Students.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            this.ViewBag.Controller = "Student";
            return View(students);
        }

        // GET: StudentController/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.Context = _context;
            Student student = _context.Students.Where(s => s.StudentId == id).FirstOrDefault();
            _context.Entry(student).Collection(c => c.StudentCourses).Query().Where(student => student.StudentId == id).Load();

            return View(student);
        }

        // GET: StudentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student student)
        {
            try
            {
                Debug.WriteLine("New added Studentid: " + student.StudentId);
                _context.Students.Add(student);
                _context.SaveChanges();
                int id = student.StudentId;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StudentController/Edit/5
        public ActionResult Edit(int id)
        {
            Student student = _context.Students.Where(s => s.StudentId == id).FirstOrDefault();
            return View(student);
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Student student)
        {
            try
            {
                _context.Attach(student);
                _context.Entry(student).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StudentController/Delete/5
        public ActionResult Delete(int id)
        {
            Student student = _context.Students.Where(s => s.StudentId == id).FirstOrDefault();
            return View(student);
        }

        // POST: StudentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Student student)
        {
            try
            {
                // Getting all data from StudentCourses table to remove for that specific student
                int studentId = student.StudentId;
                var studentcourses = from sc in _context.StudentCourses
                                     where sc.StudentId == studentId
                                     select sc;

                // Removing all courses for that student
                foreach (var sc in studentcourses)
                {
                    _context.Entry(sc).State = EntityState.Deleted;
                }
                _context.SaveChanges();


                //finallly removing student details from Student table
                _context.Students.Remove(student);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View();
            }
            
        }

        [HttpGet]
        public ActionResult AddCourse(int id)
        {
            List<Course> Assignedcourses = (from c in _context.Courses
                                    join sc in _context.StudentCourses on c.CourseId equals sc.CourseId
                                    join s in _context.Students on sc.StudentId equals s.StudentId
                                    where sc.StudentId == id
                                    select c).ToList();

            List<Course> Allcourses = (from c in _context.Courses select c).ToList();
            List<Course> unAssignedCourses = Allcourses.Except(Assignedcourses).ToList();
            unAssignedCourses.Insert(0, new Course { CourseId = 0, Name = "Select Course", Credit = "0" });
            ViewBag.CourseList = unAssignedCourses;
            ViewBag.StudentId = id;

            var studentcourse = _context.StudentCourses.Where(s => s.StudentId == id).FirstOrDefault();
            return View(studentcourse);
        }

        [HttpPost]
        public ActionResult AddCourse(StudentCourse studentCourse)
        {
            if(studentCourse.CourseId == 0)
            {
                ModelState.AddModelError("", "Select Course");
            }
            int StudentId = studentCourse.Id;
            if (ModelState.IsValid)
            {
                int SelectedCourse = studentCourse.CourseId;
                
                var studentCourses = new StudentCourse { CourseId = SelectedCourse, StudentId = StudentId };
                _context.StudentCourses.Add(studentCourses);
                _context.SaveChanges();
                return RedirectToAction("Details", new { id = StudentId });
            }
            else
            {
                return RedirectToAction("Details", new { id = StudentId });
            }


        }

        public ActionResult DeleteCourse(int id)
        {
            StudentCourse studentCourse = _context.StudentCourses.Where(sc => sc.Id == id).FirstOrDefault();
            int StudentId = studentCourse.StudentId;
            _context.Attach(studentCourse);
            _context.Entry(studentCourse).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("Details", new { id = StudentId });
        }
    }
}
