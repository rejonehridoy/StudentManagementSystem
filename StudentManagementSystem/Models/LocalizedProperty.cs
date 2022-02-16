using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace StudentManagementSystem.Models
{
    [Table("LocalizedProperty")]
    public partial class LocalizedProperty
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(400)]
        public string EntityName { get; set; }
        [Required]
        [StringLength(400)]
        public string EntityPropertryName { get; set; }
        [Required]
        public string LocalValue { get; set; }
        public int LanguageId { get; set; }
        public int EntityId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        [InverseProperty("LocalizedProperties")]
        public virtual Language Language { get; set; }
    }
}
