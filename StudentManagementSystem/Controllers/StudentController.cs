using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace StudentManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService studentService;
        private readonly ICourseService courseService;
        private const string SessionLanguageName = "Language";
        private const string SessionLanguageId = "LanguageId";
        

        public StudentController(IStudentService studentService, ICourseService courseService)
        {
            this.studentService = studentService;
            this.courseService = courseService;
        }

        public ActionResult Index(int pg = 1)
        {
            int languageId = HttpContext.Session.GetInt32(SessionLanguageId).Value;
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = studentService.GetStudents().Count();
            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            List<Student> students = studentService.GetAllLocalStudents(languageId).Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            this.ViewBag.Controller = "Student";
            return View(students);
        }

        // GET: StudentController/Details/5
        public ActionResult Details(int id)
        {
            int languageId = HttpContext.Session.GetInt32(SessionLanguageId).Value;
            ViewBag.CourseService = courseService;
            ViewBag.LanguageId = languageId;
            Student student = studentService.GetStudentDataWithCourses(id,languageId);
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
                studentService.InsertStudent(student);
                int InsertedId = student.StudentId;
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
            Student student = studentService.GetStudent(id);
            return View(student);
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Student student)
        {
            try
            {
                studentService.UpdateStudent(student);
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
            var languageId = HttpContext.Session.GetInt32(SessionLanguageId).Value;
            Student student = studentService.GetLocalStudent(id, languageId);
            return View(student);
        }

        // POST: StudentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Student student)
        {
            try
            {
                int languageId = HttpContext.Session.GetInt32(SessionLanguageId).Value;
                studentService.DeleteStudentWithCourses(student, languageId);
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
            List<Course> unAssignedCourses = studentService.GetUnassignedCourses(id);

            unAssignedCourses.Insert(0, new Course { CourseId = 0, Name = "Select Course", Credit = "0" });
            ViewBag.CourseList = unAssignedCourses;
            ViewBag.StudentId = id;

            return View();
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
                
                var NewstudentCourses = new StudentCourse { CourseId = SelectedCourse, StudentId = StudentId };
                studentService.InsertStudentCourse(NewstudentCourses);
                return RedirectToAction("Details", new { id = StudentId });
            }
            else
            {
                return RedirectToAction("Details", new { id = StudentId });
            }
        }

        public ActionResult DeleteCourse(int id)
        {
            int StudentId = studentService.DeleteStudentCourse(id);

            return RedirectToAction("Details", new { id = StudentId });
        }
    }
}
