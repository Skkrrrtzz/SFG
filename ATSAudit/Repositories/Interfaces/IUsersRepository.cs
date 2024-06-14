using ATSAudit.Models;

namespace ATSAudit.Repositories
{
    public interface IUsersRepository
    {
        public Task<UserModel> GetUser(string user);
    }
}