using StudentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Services
{
    public interface ILanguageService
    {
        IEnumerable<Language> GetLanguages();
        Language GetLanguage(int id);
        void InsertLanguage(Language language);
        void UpdateLanguage(Language language);
        void DeleteLanguage(int id);
    }
}
