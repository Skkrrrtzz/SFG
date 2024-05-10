using static APPTemplate.Models.LaptopPassModel;

namespace APPTemplate.Repository
{
    public interface ILaptopPassRepository
    {

        public Task<IEnumerable<LaptopPassRoleModel>> GetRole();
    }
}