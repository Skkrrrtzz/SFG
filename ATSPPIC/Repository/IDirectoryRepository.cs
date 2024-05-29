using ATSPPIC.Models;

namespace ATSPPIC.Repository
{
    public interface IDirectoryRepository
    {
        public Task<IEnumerable<DirectoryModel>> GetDirectory();

        public Task<byte[]> GetEmployeeImage(int in_empno);
    }
}