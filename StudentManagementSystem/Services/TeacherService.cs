using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;
using StudentManagementSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Services
{
    public class TeacherService : ITeacherService
    {
        private IRepository<Teacher> teacherRepository;
        private IRepository<CourseTeacher> courseTeacherRepository;
        private IRepository<Course> courseRepository;
        private IRepository<Language> languageRepository;
        private IRepository<LocalizedProperty> localPropertyRepository;
        private const string TeacherTable = "Teacher";
        private const string TeacherName = "Name";
        private const string TeacherDesignation = "Designation";
        private const string TeacherAge = "Age";
        private const string TeacherSalary = "Salary";
        private const string BaseLanguage = "English";

        public TeacherService(IRepository<Teacher> teacherRepository, IRepository<CourseTeacher> courseTeacherRepository,
            IRepository<Course> courseRepository, IRepository<Language> languageRepository, IRepository<LocalizedProperty>
            localPropertyRepository)
        {
            this.teacherRepository = teacherRepository;
            this.courseTeacherRepository = courseTeacherRepository;
            this.courseRepository = courseRepository;
            this.languageRepository = languageRepository;
            this.localPropertyRepository = localPropertyRepository;
        }
        public void DeleteTeacher(int id)
        {
            teacherRepository.Delete(id);
        }

        public int DeleteTeacherCourse(int id)
        {
            CourseTeacher courseTeacher = this.GetTeacherCourse(id);
            courseTeacherRepository.Delete(courseTeacher.Id);
            return courseTeacher.TeacherId;
        }

        public void DeleteTeacherWithCourses(Teacher teacher, int languageId)
        {
            Teacher teacherDeleted = GetTeacherDataWithCourses(teacher.TeacherId, languageId);
            foreach (var sc in teacherDeleted.CourseTeachers)
            {
                courseTeacherRepository.Delete(sc.Id);
            }
            DeleteTeacher(teacher.TeacherId);
        }

        public IEnumerable<Teacher> GetAllLocalTeachers(int languageId)
        {
            var teachers = this.GetTeachers().ToList();

            List<Teacher> localTeachers = new List<Teacher>();
            foreach (Teacher teacher in teachers)
            {
                Teacher newTeacher = GetLocalTeacher(teacher.TeacherId, languageId);
                localTeachers.Add(newTeacher);
            }
            return localTeachers;
        }

        public Teacher GetLocalTeacher(int id, int languageId)
        {
            Teacher teacher = teacherRepository.Get(id);
            if (languageRepository.Get(languageId).Name != BaseLanguage)
            {
                // Get local value for Teacher Name
                LocalizedProperty lp = (localPropertyRepository.GetAll().Where(ei => ei.EntityId == teacher.TeacherId)
                    .Where(en => en.EntityName == TeacherTable)
                    .Where(epn => epn.EntityPropertryName == TeacherName)
                    .Where(li => li.LanguageId == languageId).ToList().FirstOrDefault());
                if (lp != null && lp.LocalValue != null)
                {
                    teacher.Name = lp.LocalValue;
                }

                // Get Local Property for Teacher Designation
                lp = (localPropertyRepository.GetAll().Where(ei => ei.EntityId == teacher.TeacherId)
                    .Where(en => en.EntityName == TeacherTable)
                    .Where(epn => epn.EntityPropertryName == TeacherDesignation)
                    .Where(li => li.LanguageId == languageId).ToList().FirstOrDefault());
                if (lp != null && lp.LocalValue != null)
                {
                    teacher.Designation = lp.LocalValue;
                }

            }
            return teacher;
        }

        public Teacher GetTeacher(int id)
        {
            return teacherRepository.Get(id);
        }

        public CourseTeacher GetTeacherCourse(int id)
        {
            return courseTeacherRepository.Get(id);
        }

        public Teacher GetTeacherDataWithCourses(int id, int languageId)
        {
            Teacher teacher = this.GetLocalTeacher(id, languageId);
            teacherRepository.GetContext().Entry(teacher).Collection(c => c.CourseTeachers).Query().Where(teacher => teacher.TeacherId == id).Load();
            return teacher;
        }

        public IEnumerable<Teacher> GetTeachers()
        {
            return teacherRepository.GetAll();
        }

        public List<Course> GetUnassignedCourses(int id)
        {
            List<Course> Assignedcourses = (from c in courseRepository.GetAll().ToList()
                                            join ct in courseTeacherRepository.GetAll().ToList() on c.CourseId equals ct.CourseId
                                            join t in this.GetTeachers().ToList() on ct.TeacherId equals t.TeacherId
                                            where ct.TeacherId == id
                                            select c).ToList();
            List<Course> Allcourses = (from c in courseRepository.GetAll() select c).ToList();
            return Allcourses.Except(Assignedcourses).ToList();
        }

        public void InsertTeacher(Teacher teacher)
        {
            teacherRepository.Insert(teacher);
        }

        public void InsertTeacherCourse(CourseTeacher courseTeacher)
        {
            courseTeacherRepository.Insert(courseTeacher);
        }

        public void UpdateTeacher(Teacher teacher)
        {
            teacherRepository.Update(teacher);
        }
    }
}
