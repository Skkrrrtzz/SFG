using ATSAudit.Models;

namespace ATSAudit.Services
{
    public interface IUsersRepository
    {
        public UserModel GetUser(string user);
        public Task<UserModel> GetUserAsync(string user);
    }
}