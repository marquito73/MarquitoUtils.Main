using MarquitoUtils.Main.Class.Service.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarquitoUtils.Main.Class.Service.Files
{
    /// <summary>
    /// File service
    /// </summary>
    public class FileService : DefaultService, IFileService
    {
        public List<string> GetFilesInDirectory(string directory, string extension = "*")
        {
            return Directory.GetFiles(directory, $"*.{extension}", SearchOption.AllDirectories).ToList();
        }

        public string GetFilePathInDirectory(string directory, string fileName, string extension = "*")
        {
            return this.GetFilesInDirectory(directory, extension)
                .Where(filePath => filePath.Contains(fileName)).First();
        }
    }
}
