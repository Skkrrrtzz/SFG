using ATSPROD.Models;

namespace ATSPROD.Repository
{
    public interface IDirectoryRepository
    {
        public Task<IEnumerable<DirectoryModel>> GetDirectory();

        public Task<byte[]> GetEmployeeImage(int in_empno);
    }
}