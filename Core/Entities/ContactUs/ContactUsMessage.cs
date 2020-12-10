using System.ComponentModel.DataAnnotations;

namespace Core.Entities.ContactUs
{
    public class ContactUsMessage : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "The email is not valid!")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$", ErrorMessage = "The phone number is not valid!")]
        public string PhoneNumber { get; set; }
        [Required]
        [MinLength(30, ErrorMessage = "Minimum message characters 30.")]
        [MaxLength(600, ErrorMessage = "Maximum message characters 600!")]
        public string MessageText { get; set; }
    }
}