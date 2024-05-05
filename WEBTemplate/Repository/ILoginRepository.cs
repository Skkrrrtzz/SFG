using WEBTemplate.Models;

namespace WEBTemplate.Repository
{
    public interface ILoginRepository
    {

        public Task<IEnumerable<UserLoginModel>> GetLogin(string strpass);


    }
}
