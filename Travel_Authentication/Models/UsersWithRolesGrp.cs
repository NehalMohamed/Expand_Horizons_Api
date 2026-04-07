namespace Travel_Authentication.Models
{
    public class UsersWithRolesGrp
    {
        public string? Roles { get; set; }
        public int count { get; set; }
        public List<ApplicationUser> users { get; set; }

    }
}
