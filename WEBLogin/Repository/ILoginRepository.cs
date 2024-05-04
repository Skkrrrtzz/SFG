using WEBLogin.Models;

namespace WEBLogin.Repository
{
    public interface ILoginRepository
    {

        public Task<IEnumerable<UserLoginModel>> GetLogin(string strpass);


    }
}
