using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace StudentManagementSystem.Models
{
    [Table("Student")]
    public partial class Student
    {
        public Student()
        {
            StudentCourses = new HashSet<StudentCourse>();
        }

        [Key]
        public int StudentId { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
        [Column(TypeName = "date")]
        [DisplayName("Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        [StringLength(50)]
        public string Address { get; set; }
        [StringLength(15)]
        public string Contact { get; set; }

        [InverseProperty(nameof(StudentCourse.Student))]
        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
