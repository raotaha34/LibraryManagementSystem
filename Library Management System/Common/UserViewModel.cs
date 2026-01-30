namespace Library_Management_System.Common
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; } // Admin / Member
        public string? TwoFactorSecret { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}