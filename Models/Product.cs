using System.ComponentModel.DataAnnotations;

namespace Expense_Tracker.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative number.")]
        public int StockQuantity { get; set; }

        public string Category { get; set; }
    }
}
