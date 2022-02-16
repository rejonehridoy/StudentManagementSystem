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
        public ActionResult Index(int pg=1)
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
            return View(students);
        }

        // GET: StudentController/Details/5
        public ActionResult Details(int id)
        {
            Student student = _context.Students.Where(s => s.StudentId == id).FirstOrDefault();
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
                //int studentId = _context.Students.Max(s => s.StudentId);
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
                _context.Attach(student);
                _context.Entry(student).State = EntityState.Deleted;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
