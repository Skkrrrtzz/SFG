using APPTemplate.Models;

namespace APPTemplate.Repository
{
    public interface ILoginRepository
    {
        public Task<IEnumerable<UserLoginModel>> GetLogin(string strpass);

        public Task<byte[]> GetEmployeeImage(int in_empno);

        public Task<IEnumerable<UserRoleModel>> GetRole();
    }
}