using System.ComponentModel.DataAnnotations;

namespace ContactsManagement.Domain.Models
{
    public class ContactModel
    {
        public int ContactId { get; set; }
        [Required]
        [StringLength(50)]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Only alphabets are allowed in First Name field")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Only alphabets are allowed in Last Name field")]
        public string LastName { get; set; }
        [Required]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Phone number is invalid")]
        public decimal? PhoneNumber { get; set; }
        [Required]
        [StringLength(10)]
        public string Status { get; set; }
    }
}
