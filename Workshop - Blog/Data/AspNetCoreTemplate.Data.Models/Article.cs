namespace Blog.Data.Models
{
    using Blog.Data.Common.Validation;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(ArticleValidationConstants.TitleMinLength)]
        [MaxLength(ArticleValidationConstants.TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(ArticleValidationConstants.ContentMinLength)]
        [MaxLength(ArticleValidationConstants.ContentMaxLength)]
        public string Content { get; set; } = null!;

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public string AuthorId { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(AuthorId))]
        public virtual ApplicationUser Author { get; set; }

        [Required]
        public int GenreId { get; set; }

        [Required]
        [ForeignKey(nameof(GenreId))]
        public virtual Genre Genre { get; set; }
    }
}
