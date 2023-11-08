using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PayrollWebApp.Models
{
    public class EmployeeEntity
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Employee ID")]
        public int? EmployeeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime DOB { get; set; }
        [Required]
        public string Address { get; set; }
        public string? Role { get; set; } = "User";
        [DataType(DataType.Date)]
        [Display(Name = "Join Date")]
        public DateTime? DOJ { get; set; }

        [Display(Name = "Designation")]
        public int? DesignationId { get; set; }

        [ForeignKey("DesignationId")]
        public virtual DesignationEntity? Designations { get; set; }


    }
}
