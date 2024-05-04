using WEBLogin.Models;
using static WEBLogin.Models.LaptopPassModel;

namespace WEBLogin.Repository
{
    public interface ILaptopPassRepository
    {

        public Task<IEnumerable<LaptopPassRoleModel>> GetRole();


    }
}
