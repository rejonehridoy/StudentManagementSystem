using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Controllers
{
    public class TeacherController : Controller
    {
        private readonly StudentDbContext _context = new StudentDbContext();

        public TeacherController(StudentDbContext context)
        {
            _context = context;
        }
        // GET: TeacherController
        public ActionResult Index()
        {
            List<Teacher> teachers = _context.Teachers.ToList();
            return View(teachers);
        }

        // GET: TeacherController/Details/5
        public ActionResult Details(int id)
        {
            Teacher teacher = _context.Teachers.Where(s => s.TeacherId == id).FirstOrDefault();
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
                _context.Teachers.Add(teacher);
                _context.SaveChanges();
                int id = teacher.TeacherId;
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
            Teacher teacher = _context.Teachers.Where(s => s.TeacherId == id).FirstOrDefault();
            return View(teacher);
        }

        // POST: TeacherController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Teacher teacher)
        {
            try
            {
                _context.Attach(teacher);
                _context.Entry(teacher).State = EntityState.Modified;
                _context.SaveChanges();
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
            Teacher teacher = _context.Teachers.Where(s => s.TeacherId == id).FirstOrDefault();
            return View();
        }

        // POST: TeacherController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Teacher teacher)
        {
            try
            {
                _context.Attach(teacher);
                _context.Entry(teacher).State = EntityState.Deleted;
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
