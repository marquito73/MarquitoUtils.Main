using MarquitoUtils.Main.Class.Entities.File;
using MarquitoUtils.Main.Class.Enums;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace MarquitoUtils.Main.Class.Tools
{
    /// <summary>
    /// File helper
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// Excluded files
        /// </summary>
        protected static List<string> EXCLUDED_FILES = new List<string> { ".Files.Web.Config." };

        /// <summary>
        /// Get list of static files
        /// </summary>
        /// <returns>List of static files</returns>
        public static List<CustomFile> GetStaticFiles(List<string> authorizedExtensions, string authorizedFilePath)
        {
            return GetStaticFiles(authorizedExtensions, authorizedFilePath, Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Get list of static files
        /// </summary>
        /// <returns>List of static files</returns>
        public static List<CustomFile> GetStaticFiles(List<string> authorizedExtensions, string authorizedFilePath, Assembly assembly)
        {
            List<CustomFile> staticFiles = new List<CustomFile>();

            foreach (string name in assembly.GetManifestResourceNames())
            {
                if (name.Contains(authorizedFilePath) && CheckIfFileNotInExcludedFiles(name))
                {
                    string nameSpace = GetWithoutFilename(name);
                    string filenameWithExt = GetFilename(name);
                    //staticFilesNames.Add(nameSpace, fileName);

                    // Filename
                    string filename = filenameWithExt.Split(".")[0];
                    // Extension
                    string extension = filenameWithExt.Split(".")[1];

                    object content = GetResourceContent(assembly, nameSpace, filename, extension, authorizedExtensions);
                    // Object represent filename with his data
                    CustomFile staticFile;

                    if (content.GetType().Equals(typeof(string)))
                    {
                        staticFile = new CustomFile(filename, extension, nameSpace,
                            Utils.GetAsString(content));
                    }
                    else
                    {
                        staticFile = new CustomFile(filename, extension, nameSpace,
                            Utils.GetAsBytes(content));
                    }


                    //File staticFile = new File(filename, extension, nameSpace, content);
                    staticFile.RemoveStringFromNameSpace(authorizedFilePath.Substring(1));

                    staticFiles.Add(staticFile);
                }
            }

            return staticFiles;
        }

        /// <summary>
        /// Get sql files from MarquitoUtils.Main
        /// </summary>
        /// <returns>Sql files from MarquitoUtils.Main</returns>
        public static List<CustomFile> GetSqlFiles()
        {
            return GetSqlFiles(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Get sql files from specific project
        /// </summary>
        /// <param name="assembly">Specific project</param>
        /// <returns>Sql files from specific project</returns>
        public static List<CustomFile> GetSqlFiles(Assembly assembly)
        {
            return GetStaticFiles(Enumerable.Repeat("sql", 1).ToList(), ".Files.Sql.", assembly)
                .OrderBy(file => file.FileName).ToList();
        }

        /// <summary>
        /// Get sql file from MarquitoUtils.Main by name
        /// </summary>
        /// <param name="sqlFileName">The sql file name we search</param>
        /// <returns>Sql file from MarquitoUtils.Main by name</returns>
        public static CustomFile GetSqlFile(string sqlFileName)
        {
            return GetSqlFile(sqlFileName, Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Get sql file from specific project by name
        /// </summary>
        /// <param name="sqlFileName">The sql file name we search</param>
        /// <param name="assembly">Specific project</param>
        /// <returns>Sql file from specific project by name</returns>
        public static CustomFile GetSqlFile(string sqlFileName, Assembly assembly)
        {
            return GetSqlFiles(assembly).Where(file => file.FileName.Equals(sqlFileName)).First();
        }

        /// <summary>
        /// Get resource file content from an assembly
        /// </summary>
        /// <param name="assembly">The assembly contain the file</param>
        /// <param name="nameSpace">The namespace of the file</param>
        /// <param name="filename">The filename</param>
        /// <param name="extension">The filename</param>
        /// <returns>The content of the file</returns>
        protected static object GetResourceContent(Assembly assembly, string nameSpace,
            string filename, string extension, List<string> authorizedExtensions)
        {
            string resourceName = nameSpace + "." + filename + "." + extension;
            object resource = null;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {

                if (authorizedExtensions.Contains(extension))
                {
                    // We can read file as text
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        resource = reader.ReadToEnd();
                    }
                }
                else
                {
                    resource = Utils.ReadAllBytes(stream);
                }
            }
            return resource;
        }

        private static string GetFilename(string namespaceAndName)
        {
            string[] names = namespaceAndName.Split('.');

            int count = names.Length;

            return names[count - 2] + "." + names[count - 1];
        }

        private static string GetWithoutFilename(string namespaceAndName)
        {
            string filename = GetFilename(namespaceAndName);

            return namespaceAndName.Replace("." + filename, "");
        }

        private static bool CheckIfFileNotInExcludedFiles(string fileName)
        {
            bool fileNotInExcludedFiles = true;

            foreach (string excludedFileName in EXCLUDED_FILES)
            {
                if (fileName.Contains(excludedFileName))
                {
                    fileNotInExcludedFiles = false;
                }
            }

            return fileNotInExcludedFiles;
        }
    }
}
