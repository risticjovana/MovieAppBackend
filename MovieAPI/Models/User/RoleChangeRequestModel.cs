namespace MovieAPI.Models.User
{
    public class RoleChangeRequestModel
    {
        public int UserId { get; set; }
        public string RequestedRole { get; set; } = string.Empty;
    }
}
