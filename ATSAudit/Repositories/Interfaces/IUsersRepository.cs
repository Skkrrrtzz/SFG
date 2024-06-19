using ATSAudit.Models;

namespace ATSAudit.Repositories
{
    public interface IUsersRepository
    {
        public UserModel GetUser(string user);
        public Task<UserModel> GetUserAsync(string user);
    }
}