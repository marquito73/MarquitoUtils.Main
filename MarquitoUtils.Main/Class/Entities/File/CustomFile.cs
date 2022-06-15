using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MarquitoUtils.Main.Class.Entities.File
{
    /// <summary>
    /// File, with filename, extension, and content
    /// </summary>
    public class CustomFile
    {
        /// <summary>
        /// Name of file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Extension of file
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Namespace, if file is in project / library
        /// </summary>
        public string NameSpace { get; set; }

        /// <summary>
        /// Path, if it's an existing file
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Content of file
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Content of file as binary
        /// </summary>
        public byte[] BinaryContent { get; set; }

        /// <summary>
        /// A file
        /// </summary>
        /// <param name="fileName">Name of file</param>
        /// <param name="extension">Extension of file</param>
        /// <param name="content">Content of file</param>
        public CustomFile(string fileName, string extension, string content)
        {
            this.FileName = fileName;
            this.Extension = extension;
            this.Content = content;
        }

        /// <summary>
        /// A file
        /// </summary>
        /// <param name="fileName">Name of file</param>
        /// <param name="extension">Extension of file</param>
        /// <param name="nameSpace">Namespace, if file is in project / library</param>
        /// <param name="content">Content of file</param>
        public CustomFile(string fileName, string extension, string nameSpace, string content)
        {
            this.FileName = fileName;
            this.Extension = extension;
            this.NameSpace = nameSpace;
            this.Content = content;
        }

        /// <summary>
        /// A file
        /// </summary>
        /// <param name="fileName">Name of file</param>
        /// <param name="extension">Extension of file</param>
        /// <param name="content">Content of file</param>
        public CustomFile(string fileName, string extension, byte[] binaryContent)
        {
            this.FileName = fileName;
            this.Extension = extension;
            this.BinaryContent = binaryContent;
        }

        /// <summary>
        /// A file
        /// </summary>
        /// <param name="fileName">Name of file</param>
        /// <param name="extension">Extension of file</param>
        /// <param name="nameSpace">Namespace, if file is in project / library</param>
        /// <param name="content">Content of file</param>
        public CustomFile(string fileName, string extension, string nameSpace, byte[] binaryContent)
        {
            this.FileName = fileName;
            this.Extension = extension;
            this.NameSpace = nameSpace;
            this.BinaryContent = binaryContent;
        }

        public string GetNameSpaceAsCombinedPath(string pathToCombine)
        {
            return Path.Combine(pathToCombine, this.GetNameSpaceAsPath());
        }

        private string GetNameSpaceAsPath()
        {
            return this.NameSpace
                .Substring(this.NameSpace.IndexOf(".") + 1)
                .Replace(".", "\\");
        }

        public string GetCompletePathName()
        {
            StringBuilder sbCompletePath = new StringBuilder();

            sbCompletePath.Append(this.GetNameSpaceAsPath()).Append("\\")
                .Append(this.FileName).Append(".").Append(this.Extension);

            return sbCompletePath.ToString();
        }

        public void RemoveStringFromNameSpace(string stringToRemove)
        {
            this.NameSpace = this.NameSpace.Replace(stringToRemove, "");
        }
    }
}
