using Demo.Data;

namespace Demo
{
    public class StartUp
    {
        static async Task Main(string[] args)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();
        }
    }
}