namespace AdminDashboard.Models
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public List<RoleViewModel> Roles { get; set; } = null!;
    }
}
