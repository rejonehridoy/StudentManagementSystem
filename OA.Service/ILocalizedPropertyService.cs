using StudentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Services
{
    public interface ILocalizedPropertyService
    {
        IEnumerable<LocalizedProperty> GetLocalProperties();
        LocalizedProperty GetLocalProperty(int id);
        void InsertLocalProperty(LocalizedProperty localizedProperty);
        void UpdateLocalProperty(LocalizedProperty localizedProperty);
        void DeleteLocalProperty(int id);
    }
}
