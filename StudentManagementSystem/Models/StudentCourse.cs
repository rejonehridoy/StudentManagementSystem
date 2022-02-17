using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace StudentManagementSystem.Models
{
    [Table("StudentCourse")]
    public partial class StudentCourse
    {
        [Key]
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        [InverseProperty("StudentCourses")]
        public virtual Course Course { get; set; }
        [ForeignKey(nameof(StudentId))]
        [InverseProperty("StudentCourses")]
        public virtual Student Student { get; set; }
    }

    public class StudentCourseView
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string StudentName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string CourseName { get; set; }
        public string Credit { get; set; }

    }
}
