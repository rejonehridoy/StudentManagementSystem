using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.CodeAnalysis.Host;
using Microsoft.Extensions.Logging;
using StudentManagementSystem.Models;
using StudentManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILanguageService languageService;
        private const string SessionLanguageName = "Language";
        private const string SessionLanguageId = "LanguageId";

        public HomeController(ILogger<HomeController> logger, ILanguageService languageService)
        {
            this.languageService = languageService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            string language = HttpContext.Session.GetString(SessionLanguageName);
            if (language != null && language != "")
            {
                ViewBag.LanguageName =  language;
            }
            else
            {
                ViewBag.LanguageName = "English";
                HttpContext.Session.SetString(SessionLanguageName, "English");
                HttpContext.Session.SetInt32(SessionLanguageId, 1);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult SelectLanguage()
        {
            ViewBag.LanguageList = languageService.GetLanguages();
            return View();
        }

        [HttpPost]
        public IActionResult SelectLanguage(Language language)
        {
            HttpContext.Session.SetString(SessionLanguageName, languageService.GetLanguage(language.LanguageId).Name);
            HttpContext.Session.SetInt32(SessionLanguageId, language.LanguageId);
            return RedirectToAction(nameof(Index));
        }
    }
}
