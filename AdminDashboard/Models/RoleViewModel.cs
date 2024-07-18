using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.Models
{
    public class RoleViewModel
    {
        [Required]
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool IsSelected { get; set; }
    }
}
