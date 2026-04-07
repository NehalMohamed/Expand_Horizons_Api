namespace Travel_Authentication.Models
{
    public class UsersResponse
    {
        public bool? success { get; set; }
        public List<UsersWithRolesGrp>? result { get; set; }
    }
}
