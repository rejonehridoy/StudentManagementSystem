using StudentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Services
{
    public interface ITeacherService
    {
        IEnumerable<Teacher> GetTeachers();
        Teacher GetTeacher(int id);
        Teacher GetLocalTeacher(int id, int languageId);
        IEnumerable<Teacher> GetAllLocalTeachers(int languageId);
        CourseTeacher GetTeacherCourse(int id);
        Teacher GetTeacherDataWithCourses(int id, int languageId);
        List<Course> GetUnassignedCourses(int id);
        int DeleteTeacherCourse(int id);
        void DeleteTeacherWithCourses(Teacher teacher, int languageId);
        void InsertTeacher(Teacher teacher);
        void InsertTeacherCourse(CourseTeacher courseTeacher);
        void UpdateTeacher(Teacher teacher);
        void DeleteTeacher(int id);
    }
}
