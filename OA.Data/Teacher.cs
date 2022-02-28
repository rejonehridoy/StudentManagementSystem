using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace StudentManagementSystem.Models
{
    [Table("Teacher")]
    public partial class Teacher
    {
        public Teacher()
        {
            CourseTeachers = new HashSet<CourseTeacher>();
        }

        [Key]
        public int TeacherId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Designation { get; set; }
        public int Age { get; set; }
        public int? Salary { get; set; }

        [InverseProperty(nameof(CourseTeacher.Teacher))]
        public virtual ICollection<CourseTeacher> CourseTeachers { get; set; }
    }
}
