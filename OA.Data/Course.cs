using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace StudentManagementSystem.Models
{
    [Table("Course")]
    public partial class Course
    {
        public Course()
        {
            CourseTeachers = new HashSet<CourseTeacher>();
            StudentCourses = new HashSet<StudentCourse>();
        }

        [Key]
        public int CourseId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Credit { get; set; }

        [InverseProperty(nameof(CourseTeacher.Course))]
        public virtual ICollection<CourseTeacher> CourseTeachers { get; set; }
        [InverseProperty(nameof(StudentCourse.Course))]
        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
