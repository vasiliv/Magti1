namespace Magti1.Models
{
    public class BoughtNumber
    {
        public int Id { get; set; }
        public int PhoneNumber { get; set; }
        // Foreign key property
        public int ApplicationUserId { get; set; }

        // Navigation property to access the related customer
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
