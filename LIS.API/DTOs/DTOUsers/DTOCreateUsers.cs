namespace APiUsers.DTOs
{
    public class DTOCreateUsers
    {
        public int UserID { get; set;}
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Salt { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }

    }
}
