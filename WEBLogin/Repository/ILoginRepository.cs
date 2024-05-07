using WEBLogin.Models;

namespace WEBLogin.Repository
{
    public interface ILoginRepository
    {
        public Task<IEnumerable<UserLoginModel>> GetLogin(string strpass);
        public Task<byte[]> GetEmployeeImage(int in_empno);
        public Task<IEnumerable<UserMenuModel>> GetMenu(string strpass);
        public Task<IEnumerable<UserPendingModel>> GetPending(string struser);
        public Task<IEnumerable<UserRoleModel>> GetRole();
    }
}