using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using StudentManagementSystem.Models;
using StudentManagementSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace StudentManagementSystem.Services
{
    public class CourseService : ICourseService
    {
        private IRepository<Course> courseRepository;
        private IRepository<LocalizedProperty> localPropertyRepository;
        private IRepository<Language> languageRepository;
        private IRepository<StudentCourse> studentCourseRepository;
        private IRepository<CourseTeacher> CourseTeacherRepository;
        private const string CourseTable = "Course";
        private const string CourseName = "Name";
        private const string CourseCredit = "Credit";
        private const string BaseLanguage = "English";
        public CourseService(IRepository<Course> courseRepository, IRepository<LocalizedProperty> localPropertyRepository,
            IRepository<Language> languageRepository, IRepository<StudentCourse> studentCourseRepository,
            IRepository<CourseTeacher> courseTeacherRepository)
        {
            this.courseRepository = courseRepository;
            this.localPropertyRepository = localPropertyRepository;
            this.languageRepository = languageRepository;
            this.studentCourseRepository = studentCourseRepository;
            this.CourseTeacherRepository = courseTeacherRepository;
        }
        public bool DeleteCourse(int id)
        {
            var studentcourses = studentCourseRepository.GetAll().Where(sc => sc.CourseId == id).FirstOrDefault();
            var courseTeachers = CourseTeacherRepository.GetAll().Where(ct => ct.CourseId == id).FirstOrDefault();

            if (studentcourses == null || courseTeachers == null)
            {
                courseRepository.Delete(id);
                return true;
            }
            else
            {
                return false;
            }

            
        }

        public Course GetCourse(int id)
        {
            return courseRepository.Get(id);
        }

        public Course GetLocalCourse(int id, int languageId)
        {
            Course course = courseRepository.Get(id);
            if (languageRepository.Get(languageId).Name != BaseLanguage)
            {
                // Get local value for Course Name
                LocalizedProperty lp = (localPropertyRepository.GetAll().Where(ei => ei.EntityId == course.CourseId)
                    .Where(en => en.EntityName == CourseTable)
                    .Where(epn => epn.EntityPropertryName == CourseName)
                    .Where(li => li.LanguageId == languageId).ToList().FirstOrDefault());
                if (lp != null && lp.LocalValue != null)
                {
                    course.Name = lp.LocalValue;
                }

                // Get Local Property for Course Credit
                lp = (localPropertyRepository.GetAll().Where(ei => ei.EntityId == course.CourseId)
                    .Where(en => en.EntityName == CourseTable)
                    .Where(epn => epn.EntityPropertryName == CourseCredit)
                    .Where(li => li.LanguageId == languageId).ToList().FirstOrDefault());
                if (lp != null && lp.LocalValue != null)
                {
                    course.Credit = lp.LocalValue;
                }
            }
            return course;
        }

        public IEnumerable<Course> GetCourses()
        {
            return courseRepository.GetAll();
        }

        public void InsertCourse(Course course)
        {
            courseRepository.Insert(course);
        }

        public void UpdateCourse(Course course)
        {
            courseRepository.Update(course);
        }

        public IEnumerable<Course> GetAllLocalCourses(int languageId)
        {
            var courses = this.GetCourses().ToList();

            List<Course> localcourses = new List<Course>();
            foreach (Course course in courses)
            {
                Course newCourse = GetLocalCourse(course.CourseId, languageId);
                localcourses.Add(newCourse);
            }
            return localcourses;
        }
    }
}
