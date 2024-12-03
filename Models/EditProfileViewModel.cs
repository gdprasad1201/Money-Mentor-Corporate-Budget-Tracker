namespace Expense_Tracker.Models
{
    public class EditProfileViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string FirstName { get; set; }  // Add First Name
        public string LastName { get; set; }   // Add Last Name
        public IFormFile ProfilePicture { get; set; }  // For profile picture upload
    }


}