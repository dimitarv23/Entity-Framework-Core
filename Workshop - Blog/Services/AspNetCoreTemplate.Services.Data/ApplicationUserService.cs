namespace Blog.Services.Data
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    using Blog.Data.Common.Repositories;
    using Blog.Data.Models;
    using Blog.Web.ViewModels.ApplicationUser;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IRepository<ApplicationUser> userRepository;

        public ApplicationUserService(IRepository<ApplicationUser> _userRepository)
        {
            this.userRepository = _userRepository;
        }

        public async Task CreateUserAsync(RegisterUserInputModel inputModel)
        {
            string passwordSalt = this.GenerateSalt();
            string hashedPassword = this.ComputeSha256Hash(inputModel.Password, passwordSalt);

            var user = new ApplicationUser()
            {
                Username = inputModel.Username,
                Email = inputModel.Email,
                Password = hashedPassword,
                PasswordSalt = passwordSalt,
            };

            await this.userRepository.AddAsync(user);
            await this.userRepository.SaveChangesAsync();
        }

        public async Task<string> GetIdByUsernameAsync(string username)
        {
            var user = await this.userRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username);

            return user?.Id;
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await this.userRepository
                .AllAsNoTracking()
                .AnyAsync(u => u.Username == username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await this.userRepository
                .AllAsNoTracking()
                .AnyAsync(u => u.Email == email);
        }

        public async Task<bool> ValidateLoginInfoAsync(LoginInputModel inputModel)
        {
            ApplicationUser user = await this.userRepository
                .AllAsNoTracking()
                .FirstAsync(u => u.Username == inputModel.Username);

            string hashedPassword = this.ComputeSha256Hash(inputModel.Password, user.PasswordSalt);

            return hashedPassword == user.Password;
        }

        private string ComputeSha256Hash(string rawData, string salt)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                var sb = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));
                }

                sb.Append(salt);
                return sb.ToString();
            }
        }

        private string GenerateSalt()
        {
            var bytes = new byte[128 / 8];

            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }
    }
}
