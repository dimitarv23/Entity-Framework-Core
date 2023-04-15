namespace Blog.Web.Controllers
{
    using System.Threading.Tasks;

    using Blog.Services.Data;
    using Blog.Web.ViewModels.Article;
    using Microsoft.AspNetCore.Mvc;

    public class ArticleController : Controller
    {
        private readonly IGenreService genreService;

        public ArticleController(IGenreService _genreService)
        {
            this.genreService = _genreService;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var genres = await this.genreService.GetAllAsync();

            ArticleAddViewModel vm = new ArticleAddViewModel()
            {
                Genres = genres,
            };

            return this.View(vm);
        }
    }
}
