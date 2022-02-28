using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;
using StudentManagementSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementSystem.Services
{
    public class StudentService : IStudentService
    {
        private IRepository<Student> studentRepository;
        private IRepository<StudentCourse> studentCourseRepository;
        private IRepository<Course> courseRepository;
        private IRepository<Language> languageRepository;
        private IRepository<LocalizedProperty> localPropertyRepository;
        private const string StudentTable = "Student";
        private const string StudentName = "Name";
        private const string StudentAddress = "Address";
        private const string StudentDateOfBirth = "DateOfBirth";
        private const string StudentContact = "Contact";
        private const string BaseLanguage = "English";

        public StudentService(IRepository<Student> studentRepository, IRepository<StudentCourse> studentCourseRepository, 
            IRepository<Course> courseRepository, IRepository<Language> languageRepository, IRepository<LocalizedProperty>
            localPropertyRepository)
        {
            this.studentRepository = studentRepository;
            this.studentCourseRepository = studentCourseRepository;
            this.courseRepository = courseRepository;
            this.languageRepository = languageRepository;
            this.localPropertyRepository = localPropertyRepository;
        }

        public void DeleteStudent(int id)
        {
            studentRepository.Delete(id);
        }

        public int DeleteStudentCourse(int id)
        {
            StudentCourse studentCourse =  this.GetStudentCourse(id);
            studentCourseRepository.Delete(studentCourse.Id);
            return studentCourse.StudentId;

        }

        public void DeleteStudentWithCourses(Student student, int languageId)
        {
            Student studentDeleted = GetStudentDataWithCourses(student.StudentId, languageId);
            foreach (var sc in studentDeleted.StudentCourses)
            {
                studentCourseRepository.Delete(sc.Id);
            }
            DeleteStudent(student.StudentId);
        }

        public Student GetStudent(int id)
        {
            return studentRepository.Get(id);
        }

        public StudentCourse GetStudentCourse(int id)
        {
            return studentCourseRepository.Get(id);
        }

        public Student GetStudentDataWithCourses(int id, int languageId)
        {
            Student student = this.GetLocalStudent(id, languageId);
            studentRepository.GetContext().Entry(student).Collection(c => c.StudentCourses).Query().Where(student => student.StudentId == id).Load();
            return student;
        }

        public Student GetLocalStudent(int id, int languageId)
        {
            Student student = studentRepository.Get(id);
            if (languageRepository.Get(languageId).Name != BaseLanguage)
            {
                // Get local value for Student Name
                LocalizedProperty lp = (localPropertyRepository.GetAll().Where(ei => ei.EntityId == student.StudentId)
                    .Where(en => en.EntityName == StudentTable)
                    .Where(epn => epn.EntityPropertryName == StudentName)
                    .Where(li => li.LanguageId == languageId).ToList().FirstOrDefault());
                if (lp != null && lp.LocalValue != null)
                {
                    student.Name = lp.LocalValue;
                }

                // Get Local Property for Student Address
                lp = (localPropertyRepository.GetAll().Where(ei => ei.EntityId == student.StudentId)
                    .Where(en => en.EntityName == StudentTable)
                    .Where(epn => epn.EntityPropertryName == StudentAddress)
                    .Where(li => li.LanguageId == languageId).ToList().FirstOrDefault());
                if (lp != null && lp.LocalValue != null)
                {
                    student.Address = lp.LocalValue;
                }

                // Get Local Property for Student Contact
                lp = (localPropertyRepository.GetAll().Where(ei => ei.EntityId == student.StudentId)
                    .Where(en => en.EntityName == StudentTable)
                    .Where(epn => epn.EntityPropertryName == StudentContact)
                    .Where(li => li.LanguageId == languageId).ToList().FirstOrDefault());
                if (lp != null && lp.LocalValue != null)
                {
                    student.Contact = lp.LocalValue;
                }

            }
            return student;
        }

        public IEnumerable<Student> GetStudents()
        {
            return studentRepository.GetAll();
        }

        public void InsertStudent(Student student)
        {
            studentRepository.Insert(student);
        }

        public void InsertStudentCourse(StudentCourse studentCourse)
        {
            studentCourseRepository.Insert(studentCourse);
        }

        public void UpdateStudent(Student student)
        {
            studentRepository.Update(student);
        }


        public List<Course> GetUnassignedCourses(int id)
        {
            List<Course> Assignedcourses = (from c in courseRepository.GetAll().ToList()
                                            join sc in studentCourseRepository.GetAll().ToList() on c.CourseId equals sc.CourseId
                                            join s in this.GetStudents().ToList() on sc.StudentId equals s.StudentId
                                            where sc.StudentId == id
                                            select c).ToList();
            List<Course> Allcourses = (from c in courseRepository.GetAll() select c).ToList();
            return Allcourses.Except(Assignedcourses).ToList();
        }

        public IEnumerable<Student> GetAllLocalStudents(int languageId)
        {
            var students = this.GetStudents().ToList();

            List<Student> localStudents = new List<Student>();
            foreach (Student student in students)
            {
                Student newStudent = GetLocalStudent(student.StudentId, languageId);
                localStudents.Add(newStudent);
            }
            return localStudents;
        }
    }
}
