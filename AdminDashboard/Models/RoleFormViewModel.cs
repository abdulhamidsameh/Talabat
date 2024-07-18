using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.Models
{
    public class RoleFormViewModel
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; } = null!;
    }
}
