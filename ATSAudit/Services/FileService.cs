using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;

namespace ATSAudit.Services
{
    public class FileService
    {
        private readonly string directoryPath = @"\\DASHBOARDPC\ATSPortals\ATSAuditFiles";
 
        public async Task<IEnumerable<string>> GetFilesOrEmpty(string form, string subform, string id)
        {
            string fullPath = $@"{directoryPath}\{form}\{subform}\{id}";
            var files = Enumerable.Empty<string>();

            //Check if there are any files in the directory
            if (Directory.Exists(fullPath))
            {
                files = await Task.Run(() => Directory.EnumerateFiles(fullPath));
            }

            return files;
        }

        public async Task<bool> CheckDirectoryIfEmpty(string form, string subform, int id)
        {
            string fullPath = $@"{directoryPath}\{form}\{subform}\{id}";

            //Check if there are any files in the directory
            if (Directory.Exists(fullPath))
            {
                var files = await Task.Run(() => Directory.EnumerateFiles(fullPath));
                return files.Any();
            }

            return false;

        }

        public async Task<bool> DeleteDirectory(string form, string subform, int id)
        {
            string fullPath = $@"{directoryPath}\{form}\{subform}\{id}";

            //Delete files
            if (Directory.Exists(fullPath))
            {
                await Task.Run(() => Directory.Delete(fullPath, true));
                return !Directory.Exists(fullPath);
            }

            return false;
        }

    }
}