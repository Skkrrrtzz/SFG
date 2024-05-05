using WEBTemplate.Models;
using static WEBTemplate.Models.LaptopPassModel;

namespace WEBTemplate.Repository
{
    public interface ILaptopPassRepository
    {
        public Task<byte[]> GetEmployeeImage(int in_empno);
        public Task<IEnumerable<LaptopPassRoleModel>> GetRole();


    }
}
