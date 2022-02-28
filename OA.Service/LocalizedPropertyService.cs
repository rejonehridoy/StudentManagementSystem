using StudentManagementSystem.Models;
using StudentManagementSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Services
{
    public class LocalizedPropertyService : ILocalizedPropertyService
    {
        private IRepository<LocalizedProperty> localizedPropertyRepository;

        public LocalizedPropertyService(IRepository<LocalizedProperty> localizedPropertyRepository)
        {
            this.localizedPropertyRepository = localizedPropertyRepository;
        }
        public void DeleteLocalProperty(int id)
        {
            localizedPropertyRepository.Delete(id);
        }

        public IEnumerable<LocalizedProperty> GetLocalProperties()
        {
            return localizedPropertyRepository.GetAll();
        }

        public LocalizedProperty GetLocalProperty(int id)
        {
            return localizedPropertyRepository.Get(id);
        }

        public void InsertLocalProperty(LocalizedProperty localizedProperty)
        {
            localizedPropertyRepository.Insert(localizedProperty);
        }

        public void UpdateLocalProperty(LocalizedProperty localizedProperty)
        {
            localizedPropertyRepository.Update(localizedProperty);
        }
    }
}
