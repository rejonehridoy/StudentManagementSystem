using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace StudentManagementSystem.Models
{
    [Table("CourseTeacher")]
    public partial class CourseTeacher
    {
        [Key]
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        [InverseProperty("CourseTeachers")]
        public virtual Course Course { get; set; }
        [ForeignKey(nameof(TeacherId))]
        [InverseProperty("CourseTeachers")]
        public virtual Teacher Teacher { get; set; }
    }
}
