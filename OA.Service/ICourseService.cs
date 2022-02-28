using StudentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Services
{
    public interface ICourseService
    {
        IEnumerable<Course> GetCourses();
        IEnumerable<Course> GetAllLocalCourses(int languageId);
        Course GetLocalCourse(int id, int languageId);
        Course GetCourse(int id);
        void InsertCourse(Course course);
        void UpdateCourse(Course course);
        bool DeleteCourse(int id);
    }
}
