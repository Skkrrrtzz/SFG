using COMLaptopPass.Models;

namespace COMLaptopPass.Repository
{
    public interface ILaptopPassRepository
    {

        public Task<IEnumerable<LaptopPassRoleModel>> GetLogin();

        public Task<IEnumerable<LaptopPassRequestModel>> GetRequest(LaptopPassParameters param);

    }
}
