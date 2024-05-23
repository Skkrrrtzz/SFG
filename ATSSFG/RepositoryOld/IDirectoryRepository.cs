using ATSSFG.Models;

namespace ATSSFG.Repository
{
    public interface IDirectoryRepository
    {
        public Task<IEnumerable<DirectoryModel>> GetDirectory();

        public Task<byte[]> GetEmployeeImage(int in_empno);
    }
}