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
    public interface IFileService
    {
        /// <summary>
        /// Get files in directory
        /// </summary>
        /// <param name="directory">The directory</param>
        /// <param name="extension">Extension of files searched</param>
        /// <returns>Files in directory</returns>
        public List<string> GetFilesInDirectory(string directory, string extension = "*");

        /// <summary>
        /// Get the file path of file inside directory
        /// </summary>
        /// <param name="directory">The directory</param>
        /// <param name="fileName">The name of the file</param>
        /// <param name="extension">Extension of files searched</param>
        /// <returns>File path of file inside directory</returns>
        public string GetFilePathInDirectory(string directory, string fileName, string extension = "*");
    }
}
