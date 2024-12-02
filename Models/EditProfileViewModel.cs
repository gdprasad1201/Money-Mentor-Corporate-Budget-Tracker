namespace Expense_Tracker.Models
{
    public class EditProfileViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ProfilePictureUrl { get; set; } // This will hold the user's profile picture URL
    }
}