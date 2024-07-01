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
            return Path.Combine(directoryPath, $@"{actionItem.Form}\{actionItem.CPARId}\{actionItem.Subform}\{actionItem.Id}");
        }

        public string GetFullPath(DeleteActionItemDTO actionItem)
        {
            return Path.Combine(directoryPath, $@"{actionItem.Form}\{actionItem.CPARId}\{actionItem.Subform}\{actionItem.Id}");
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

        public bool CheckDirExists(ActionItemDTO actionItem)
        {
            return Directory.Exists(GetFullPath(actionItem));
        }
        public bool CheckDirExists(DeleteActionItemDTO actionItem)
        {
            return Directory.Exists(GetFullPath(actionItem));
        }

        public bool CheckDirHasFiles(ActionItemDTO actionItem)
        {
            var ignore = new[] {".db"};

            //Check if there are any files in the directory
            if (Directory.Exists(GetFullPath(actionItem)))
            {
                var files = Directory.EnumerateFiles(GetFullPath(actionItem))
                                     .Where(file => !ignore.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase))
                                     .Select(Path.GetFileName);
                return files.Any();
            }
            Console.WriteLine("False");

            return false;

        }

        public async Task<bool> CheckDirHasFilesAsync(ActionItemDTO actionItem)
        {
            var ignore = new[] {".db"};

            //Check if there are any files in the directory
            if (Directory.Exists(GetFullPath(actionItem)))
            {
                var files = await Task.Run(() => Directory.EnumerateFiles(GetFullPath(actionItem))
                                                          .Where(file => !ignore.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase))
                                                          .Select(Path.GetFileName));
                return files.Any();
            }

            return false;
        }

        //TODO
        public async Task<FileEnum> CreateEvidence(List<IFormFile> evidence, ActionItemDTO actionItem)
        {
            if (evidence != null && evidence.Count > 0)
            {
                string fullPath = GetFullPath(actionItem);

                foreach (var file in evidence)
                {
                    string filePath = Path.Combine(fullPath, file.FileName);

                    //Check if directory exists, otherwise create directory
                    if (!Directory.Exists(fullPath))
                    {
                        Directory.CreateDirectory(fullPath);
                    }

                    if (File.Exists(filePath))
                    {
                        return FileEnum.Duplicate;
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                return FileEnum.Created;
            } 
            return FileEnum.NoFile;
        }

        public async Task<FileEnum> CreateEvidences(List<IFormFile> evidence, ActionItemDTO actionItem)
        {
            if (evidence != null && evidence.Count > 0)
            {
                string fullPath = GetFullPath(actionItem);

                foreach (var file in evidence)
                {
                    string filePath = Path.Combine(fullPath, file.FileName);

                    //Check if directory exists, otherwise create directory
                    if (!Directory.Exists(fullPath))
                    {
                        Directory.CreateDirectory(fullPath);
                    }

                    if (File.Exists(filePath))
                    {
                        return FileEnum.Duplicate;
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                return FileEnum.Created;
            } 
            return FileEnum.NoFile;
        }

        public async Task<bool> DeleteDirectory(ActionItemDTO actionItem)
        {
            string actionItemPath = GetFullPath(actionItem);

            //Delete directory
            if (Directory.Exists(actionItemPath))
            {
                await Task.Run(() => Directory.Delete(actionItemPath, true));
                return !Directory.Exists(actionItemPath);
            }

            return false;
        }

        public async Task<bool> DeleteEvidence(ActionItemDTO actionItem, string filename)
        {
            string actionItemPath = GetFullPath(actionItem);
            string filePath = Path.Combine(actionItemPath, filename);

            //Delete files
            if (Directory.Exists(actionItemPath))
            {
                await Task.Run(() => File.Delete(filePath));
                return !File.Exists(filePath);
            }

            return false;
        }

        public async Task<bool> DeleteEvidence(DeleteActionItemDTO actionItem)
        {
            string actionItemPath = GetFullPath(actionItem);
            // string filePath = Path.Combine(actionItemPath, filename);
            string filePath = Path.Combine(actionItemPath, actionItem.Filename);

            Console.WriteLine(filePath);

            //Delete files
            if (Directory.Exists(actionItemPath))
            {
                await Task.Run(() => File.Delete(filePath));
                return File.Exists(filePath);
            }

            return false;
        }

    }

    public record FileStatus
    {
        
    }

    public enum FileEnum 
    {
        NoFile,
        Duplicate,
        Created,
        Deleted
    }
}