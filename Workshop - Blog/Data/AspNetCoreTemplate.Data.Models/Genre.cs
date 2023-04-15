namespace Blog.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Blog.Data.Common.Validation;

    public class Genre
    {
        public Genre()
        {
            this.Articles = new List<Article>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GenreValidationConstants.GenreNameMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
