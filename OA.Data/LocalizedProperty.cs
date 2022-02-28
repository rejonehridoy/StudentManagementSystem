using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Entity Name")]
        public string EntityName { get; set; }
        [Required]
        [StringLength(400)]
        [DisplayName("Entity Property Name")]
        public string EntityPropertryName { get; set; }
        [Required]
        [DisplayName("Local Value")]
        public string LocalValue { get; set; }
        [DisplayName("Language")]
        public int LanguageId { get; set; }
        public int EntityId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        //[InverseProperty("LocalizedProperties")]
        public virtual Language Language { get; set; }
    }
}
