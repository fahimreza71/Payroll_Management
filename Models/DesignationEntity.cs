using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayrollWebApp.Models
{
    public class DesignationEntity
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Designation ID")]
        public int? DesignationId { get; set; }

        [Required]
        [Display(Name = "Designation")]
        public string? DesignationType { get; set; }

        [Required]
        [Display(Name = "Base Salary")]
        public double? BaseSalary { get; set; }
    }
}
