using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PayrollWebApp.Models
{
    public class PayrollEntity
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Range(100, 999999999)]
        [Display(Name = "Payroll ID")]
        public int? PayrollId { get; set; }
        public double? Bonus { get; set; } = 0;
        public string? Month { get; set; }

        [Display(Name = "Employee Name")]
        public int? EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual EmployeeEntity Employees { get; set; }
        [Display(Name = "Designation")]
        public int? DesignationId { get; set; }
        [ForeignKey("DesignationId")]
        public virtual DesignationEntity Designations { get; set; }
    }
}
