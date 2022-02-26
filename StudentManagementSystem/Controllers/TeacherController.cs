using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ITeacherService teacherService;
        private readonly ICourseService courseService;
        private const string SessionLanguageName = "Language";
        private const string SessionLanguageId = "LanguageId";

        private readonly StudentDbContext _context = new StudentDbContext();

        public TeacherController(ITeacherService teacherService, ICourseService courseService)
        {
            this.teacherService = teacherService;
            this.courseService = courseService;
        }


        // GET: TeacherController
        public ActionResult Index(int pg=1)
        {
            int languageId = HttpContext.Session.GetInt32(SessionLanguageId).Value;
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = teacherService.GetTeachers().Count();
            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            List<Teacher> teachers = teacherService.GetAllLocalTeachers(languageId).Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            this.ViewBag.Controller = "Teacher";
            return View(teachers);
        }

        // GET: TeacherController/Details/5
        public ActionResult Details(int id)
        {
            int languageId = HttpContext.Session.GetInt32(SessionLanguageId).Value;
            ViewBag.CourseService = courseService;
            ViewBag.LanguageId = languageId;
            Teacher teacher = teacherService.GetTeacherDataWithCourses(id, languageId);

            //ViewBag.Context = _context;
            //Teacher teacher = _context.Teachers.Where(s => s.TeacherId == id).FirstOrDefault();
            //_context.Entry(teacher).Collection(c => c.CourseTeachers).Query().Where(teacher => teacher.TeacherId == id).Load();
            return View(teacher);
        }

        // GET: TeacherController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TeacherController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Teacher teacher)
        {
            try
            {
                //_context.Teachers.Add(teacher);
                //_context.SaveChanges();
                //int id = teacher.TeacherId;

                teacherService.InsertTeacher(teacher);
                int InsertedId = teacher.TeacherId;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TeacherController/Edit/5
        public ActionResult Edit(int id)
        {
            Teacher teacher = teacherService.GetTeacher(id);
            return View(teacher);
        }

        // POST: TeacherController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Teacher teacher)
        {
            try
            {
                //_context.Attach(teacher);
                //_context.Entry(teacher).State = EntityState.Modified;
                //_context.SaveChanges();
                teacherService.UpdateTeacher(teacher);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TeacherController/Delete/5
        public ActionResult Delete(int id)
        {
            //Teacher teacher = _context.Teachers.Where(s => s.TeacherId == id).FirstOrDefault();
            var languageId = HttpContext.Session.GetInt32(SessionLanguageId).Value;
            Teacher teacher = teacherService.GetLocalTeacher(id, languageId);
            return View(teacher);
        }

        // POST: TeacherController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Teacher teacher)
        {
            try
            {
                //// Getting all data from CourseTeacher table to remove for that specific Teacher
                //int TeacherId = teacher.TeacherId;
                //var courseTeachers = from ct in _context.CourseTeachers
                //                     where ct.TeacherId == TeacherId
                //                     select ct;

                //// Removing all courses for that student
                //foreach (var ct in courseTeachers)
                //{
                //    _context.Entry(ct).State = EntityState.Deleted;
                //}
                //_context.SaveChanges();


                ////finallly removing student details from Student table
                //_context.Teachers.Remove(teacher);
                //_context.SaveChanges();

                int languageId = HttpContext.Session.GetInt32(SessionLanguageId).Value;
                teacherService.DeleteTeacherWithCourses(teacher, languageId);
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
            //List<Course> Assignedcourses = (from c in _context.Courses
            //                                join ct in _context.CourseTeachers on c.CourseId equals ct.CourseId
            //                                join t in _context.Teachers on ct.TeacherId equals t.TeacherId
            //                                where ct.TeacherId == id
            //                                select c).ToList();

            //List<Course> Allcourses = (from c in _context.Courses select c).ToList();
            //List<Course> unAssignedCourses = Allcourses.Except(Assignedcourses).ToList();
            //unAssignedCourses.Insert(0, new Course { CourseId = 0, Name = "Select Course", Credit = "0" });
            //ViewBag.CourseList = unAssignedCourses;
            //ViewBag.TeacherId = id;

            //var courseteacher = _context.CourseTeachers.Where(t => t.TeacherId == id).FirstOrDefault();


            List<Course> unAssignedCourses = teacherService.GetUnassignedCourses(id);

            unAssignedCourses.Insert(0, new Course { CourseId = 0, Name = "Select Course", Credit = "0" });
            ViewBag.CourseList = unAssignedCourses;
            ViewBag.TeacherId = id;
            return View();
        }

        [HttpPost]
        public ActionResult AddCourse(CourseTeacher courseTeacher)
        {
            if (courseTeacher.CourseId == 0)
            {
                ModelState.AddModelError("", "Select Course");
            }
            int TeacherId = courseTeacher.Id;
            if (ModelState.IsValid)
            {
                //int SelectedCourseId = courseTeacher.CourseId;

                //var NewcourseTeacher = new CourseTeacher { CourseId = SelectedCourseId, TeacherId = TeacherId };
                //_context.CourseTeachers.Add(NewcourseTeacher);
                //_context.SaveChanges();

                int SelectedCourse = courseTeacher.CourseId;

                var NewTeacherCourses = new CourseTeacher { CourseId = SelectedCourse, TeacherId = TeacherId };
                teacherService.InsertTeacherCourse(NewTeacherCourses);
                return RedirectToAction("Details", new { id = TeacherId });
            }
            else
            {
                return RedirectToAction("Details", new { id = TeacherId });
            }
        }

        public ActionResult DeleteCourse(int id)
        {
            //CourseTeacher courseTeacher = _context.CourseTeachers.Where(ct => ct.Id == id).FirstOrDefault();
            //int TeacherId = courseTeacher.TeacherId;
            //_context.Attach(courseTeacher);
            //_context.Entry(courseTeacher).State = EntityState.Deleted;
            //_context.SaveChanges();

            int TeacherId = teacherService.DeleteTeacherCourse(id);
            return RedirectToAction("Details", new { id = TeacherId });
        }
    }
}
