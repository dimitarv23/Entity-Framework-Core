namespace Blog.Web.ViewModels.ApplicationUser
{
    using System.ComponentModel.DataAnnotations;

    using Blog.Common.Validation;

    public class RegisterUserInputModel
    {
        [Required]
        [MinLength(RegisterUserValidationConstants.UsernameMinLength)]
        [MaxLength(RegisterUserValidationConstants.UsernameMaxLength)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MinLength(RegisterUserValidationConstants.EmailMinLength)]
        [MaxLength(RegisterUserValidationConstants.EmailMaxLength)]
        public string Email { get; set; }

        [Required]
        [MinLength(RegisterUserValidationConstants.PasswordMinLength)]
        [MaxLength(RegisterUserValidationConstants.PasswordMaxLength)]
        public string Password { get; set; }

        [Required]
        [MinLength(RegisterUserValidationConstants.PasswordMinLength)]
        [MaxLength(RegisterUserValidationConstants.PasswordMaxLength)]
        public string PasswordConfirmation { get; set; }
    }
}
