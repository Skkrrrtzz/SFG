using ATSSFG.Models;

namespace ATSSFG.Repository
{
    public interface IUserRepository
    {
        public Task<IEnumerable<UsersModel>> GetUsers();

        public Task<bool> DeleteUser(int id);

        public Task<bool> EditUserAsync(UsersModel edit);

        public Task<string> AddUserAsync(UsersModel add);
    }
}