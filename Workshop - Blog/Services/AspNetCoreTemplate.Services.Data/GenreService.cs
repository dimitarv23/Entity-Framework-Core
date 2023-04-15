namespace Blog.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Blog.Data.Common.Repositories;
    using Blog.Data.Models;
    using Blog.Services.Mapping;
    using Blog.Web.ViewModels.Genres;
    using Microsoft.EntityFrameworkCore;

    public class GenreService : IGenreService
    {
        private readonly IRepository<Genre> genreRepository;

        public GenreService(IRepository<Genre> _genreRepository)
        {
            this.genreRepository = _genreRepository;
        }

        public async Task<ICollection<ListGenreArticleAddViewModel>> GetAllAsync()
        {
            return await this.genreRepository
                .AllAsNoTracking()
                .To<ListGenreArticleAddViewModel>()
                .ToArrayAsync();
        }
    }
}
