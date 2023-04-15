namespace Blog.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Blog.Data.Common.Validation;

    public class ApplicationUser
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Articles = new List<Article>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MinLength(ApplicationUserValidationConstants.UsernameMinLength)]
        [MaxLength(ApplicationUserValidationConstants.UsernameMaxLength)]
        public string Username { get; set; } = null!;

        [Required]
        [MinLength(ApplicationUserValidationConstants.EmailMinLength)]
        [MaxLength(ApplicationUserValidationConstants.EmailMaxLength)]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(ApplicationUserValidationConstants.PasswordMinLength)]
        [MaxLength(ApplicationUserValidationConstants.PasswordMaxLength)]
        public string Password { get; set; } = null!;

        [Required]
        [MaxLength(ApplicationUserValidationConstants.PasswordSaltMaxLength)]
        public string PasswordSalt { get; set; } = null!;

        public virtual ICollection<Article> Articles { get; set; }
    }
}
