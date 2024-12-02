﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Icon { get; set; } = "";

        [Column(TypeName = "nvarchar(100)")]
        public string Type { get; set; } = "Expense";

        [NotMapped]
        public string? TitleWithIcon => $"{Icon} {Title}";
    }
}