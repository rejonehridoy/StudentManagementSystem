using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Controllers
{
    public class LocalizedPropertyController : Controller
    {
        private readonly ILocalizedPropertyService localizedPropertyService;
        private readonly ILanguageService languageService;
        private readonly StudentDbContext _context = new StudentDbContext();

        public LocalizedPropertyController(ILocalizedPropertyService localizedPropertyService, ILanguageService languageService)
        {
            this.localizedPropertyService = localizedPropertyService;
            this.languageService = languageService;
        }

        // GET: LocalizedPropertyController
        public ActionResult Index(int pg=1)
        {
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = localizedPropertyService.GetLocalProperties().Count();
            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;
            List<LocalizedProperty> localizedPropertiesList = localizedPropertyService.GetLocalProperties().Skip(recSkip).Take(pager.PageSize).ToList();
            foreach (var locProperty in localizedPropertiesList)
            {
                int languageId = locProperty.LanguageId;
                locProperty.Language = languageService.GetLanguage(languageId);
            }

            this.ViewBag.Pager = pager;
            this.ViewBag.Controller = "LocalizedProperty";

            return View(localizedPropertiesList);
        }

        // GET: LocalizedPropertyController/Details/5
        public ActionResult Details(int id)
        {
            LocalizedProperty localizedProperty = localizedPropertyService.GetLocalProperty(id);
            localizedProperty.Language = languageService.GetLanguage(localizedProperty.LanguageId);

            return View(localizedProperty);
        }

        // GET: LocalizedPropertyController/Create
        public ActionResult Create()
        {
            ViewBag.LanguageList = languageService.GetLanguages();
            return View();
        }

        // POST: LocalizedPropertyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LocalizedProperty localizedProperty)
        {
            try
            {
                localizedPropertyService.InsertLocalProperty(localizedProperty);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LocalizedPropertyController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.LanguageList = languageService.GetLanguages();
            LocalizedProperty localizedProperty = localizedPropertyService.GetLocalProperty(id);
            return View(localizedProperty);
        }

        // POST: LocalizedPropertyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, LocalizedProperty localizedProperty)
        {
            try
            {
                localizedPropertyService.UpdateLocalProperty(localizedProperty);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LocalizedPropertyController/Delete/5
        public ActionResult Delete(int id)
        {
            LocalizedProperty localizedProperty = localizedPropertyService.GetLocalProperty(id);
            return View(localizedProperty);
        }

        // POST: LocalizedPropertyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, LocalizedProperty localizedProperty)
        {
            try
            {
                localizedPropertyService.DeleteLocalProperty(localizedProperty.Id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
