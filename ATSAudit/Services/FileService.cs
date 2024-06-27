using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using ATSAudit.DTOs;

namespace ATSAudit.Services
{
    public class FileService
    {
        private readonly string directoryPath = @"\\DASHBOARDPC\ATSPortals\ATSAuditFiles";

        public string GetFullPath(ActionItemDTO actionItem)
        {
            return Path.Combine(directoryPath, $@"{actionItem.Form}\{actionItem.Subform}\{actionItem.Id}");
        }
 
        public async Task<IEnumerable<string>> GetFilePathsOrEmpty(ActionItemDTO actionItem)
        {
            var files = Enumerable.Empty<string>();
            var ignore = new[] {"*.db"};

            //Check if there are any files in the directory
            if (Directory.Exists(GetFullPath(actionItem)))
            {
                files = await Task.Run(() => Directory.EnumerateFiles(GetFullPath(actionItem))
                                                    .Where(file => !ignore.Contains(Path.GetFileName(file))));
            }

            return files;
        }

        public async Task<IEnumerable<string?>> GetFileNamesOrEmpty(ActionItemDTO actionItem)
        {
            var files = Enumerable.Empty<string?>();
            var ignore = new[] {".db"};

            //Check if there are any files in the directory
            if (Directory.Exists(GetFullPath(actionItem)))
            {
                files = await Task.Run(() => Directory.EnumerateFiles(GetFullPath(actionItem))
                                                    .Where(file => !ignore.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase))
                                                    .Select(Path.GetFileName));
            }

            return files;
        }

        public async Task<bool> CheckDirectoryIfEmpty(ActionItemDTO actionItem)
        {
            //Check if there are any files in the directory
            if (Directory.Exists(GetFullPath(actionItem)))
            {
                var files = await Task.Run(() => Directory.EnumerateFiles(GetFullPath(actionItem)));
                return files.Any();
            }

            return false;

        }

        public async Task<bool> DeleteDirectory(ActionItemDTO actionItem)
        {
            string fullPath = GetFullPath(actionItem);

            //Delete files
            if (Directory.Exists(GetFullPath(actionItem)))
            {
                await Task.Run(() => Directory.Delete(fullPath, true));
                return !Directory.Exists(fullPath);
            }

            return false;
        }

    }
}