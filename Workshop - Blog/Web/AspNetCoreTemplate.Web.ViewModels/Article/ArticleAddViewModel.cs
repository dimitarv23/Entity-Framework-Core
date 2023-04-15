namespace Blog.Web.ViewModels.Article
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Blog.Common.Validation;
    using Blog.Web.ViewModels.Genres;

    public class ArticleAddViewModel
    {
        public ArticleAddViewModel()
        {
            this.Genres = new List<ListGenreArticleAddViewModel>();
        }

        [Required]
        [MinLength(ArticleAddValidationConstants.TitleMinLength)]
        [MaxLength(ArticleAddValidationConstants.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MinLength(ArticleAddValidationConstants.ContentMinLength)]
        [MaxLength(ArticleAddValidationConstants.ContentMaxLength)]
        public string Content { get; set; }

        public int GenreId { get; set; } //Selected genre

        public ICollection<ListGenreArticleAddViewModel> Genres { get; set; }
    }
}
