using ATSSFG.Models;

namespace ATSSFG.Repository
{
    public interface IUserRepository
    {
        public Task<IEnumerable<UsersInfoModel>> GetRole();

        public Task<UsersInfoModel> CheckUser(string name);

        public Task<IEnumerable<UsersInfoModel>> GetUsersAsync();

        public Task<bool> DeleteUserAsync(int id);

        public Task<bool> EditUserAsync(UsersInfoModel edit);

        public Task<string> AddUserAsync(UsersInfoModel add);
    }
}