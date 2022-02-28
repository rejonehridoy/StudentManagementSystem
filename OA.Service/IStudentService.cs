using StudentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Services
{
    public interface IStudentService
    {
        IEnumerable<Student> GetStudents();
        Student GetStudent(int id);
        Student GetLocalStudent(int id, int languageId);
        IEnumerable<Student> GetAllLocalStudents(int languageId);
        StudentCourse GetStudentCourse(int id);
        Student GetStudentDataWithCourses(int id, int languageId);
        List<Course> GetUnassignedCourses(int id);
        int DeleteStudentCourse(int id);
        void DeleteStudentWithCourses(Student student, int languageId);
        void InsertStudent(Student student);
        void InsertStudentCourse(StudentCourse studentCourse);
        void UpdateStudent(Student student);
        void DeleteStudent(int id);
    }
}
