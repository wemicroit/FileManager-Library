using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WeMicroIt.Utils.FileConverter.Models
{
    /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/model/*'/> 
    public class FileDetails
    {
        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/constructor[@name="Default"]/*'/> 
        public FileDetails()
        {
            DirPath = null;
            FileName = "temp";
            FileExt = "xml";

            xmls = new List<string>() { "xml", "html" };
        }

        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/properties[@name="DirPath"]/*'/> 
        public string DirPath { get; set; }
        
        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/properties[@name="SubDirPath"]/*'/> 
        public string SubDirPath { get; set; }
        
        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/properties[@name="FileName"]/*'/> 
        public string FileName { get; set; }
        
        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/properties[@name="FileExt"]/*'/> 
        public string FileExt { get; set; }

        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/properties[@name="FullFileName"]/*'/> 
        public string FullFileName
        {
            get
            {
                return string.Join(".", FileName, FileExt);
            }
        }

        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/properties[@name="FullDirectory"]/*'/> 
        public string FullDirectory
        {
            get
            {
                if (!string.IsNullOrEmpty(SubDirPath))
                {
                    return Path.Combine(DirPath, SubDirPath);
                }
                return DirPath;
            }
        }

        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/properties[@name="FullPath"]/*'/> 
        public string FullPath {
            get {
                if (string.IsNullOrEmpty(DirPath))
                {
                    return FullFileName;
                }
                return Path.Combine(FullDirectory, FullFileName);
            }
            set {
                SubDirPath = null;
                if (string.IsNullOrEmpty(value))
                {
                    DirPath = null;
                    FileExt = null;
                    FileName = null;
                    return;
                }
                int index = value.LastIndexOf('\\');
                int ext = value.LastIndexOf('.');
                if (index > ext)
                {
                    DirPath = value;
                    return;
                }
                DirPath = value.Substring(0, index);
                FileName = value.Substring(index + 1, ext - index - 1); ;
                FileExt = value.Substring(ext + 1);
            }
        }

        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/properties[@name="IsCSV"]/*'/> 
        public bool IsCSV
        {
            get
            {
                return checkExtension("csv");
            }
        }

        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/properties[@name="IsJSON"]/*'/> 
        public bool IsJSON
        {
            get
            {
                return checkExtension("json");
            }
        }

        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/properties[@name="IsXML"]/*'/> 
        public bool IsXML
        {
            get
            {
                return checkExtension(xmls);
            }
        }

        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/properties[@name="IsDirectory"]/*'/> 
        public bool IsDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(DirPath))
                {
                    throw new ArgumentNullException();
                }
                return Directory.Exists(FullDirectory) ? true : throw new DirectoryNotFoundException();
            }
        }

        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/properties[@name="IsFile"]/*'/> 
        public bool IsFile
        {
            get
            {
                if (IsDirectory)
                {
                    if (string.IsNullOrEmpty(FileName) || string.IsNullOrEmpty(FileExt))
                    {
                        throw new ArgumentNullException();
                    }
                    return File.Exists(FullPath) ? true : throw new FileNotFoundException();
                }
                throw new DirectoryNotFoundException();
            }
        }

        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/properties[@name="Files"]/*'/> 
        public List<string> Files
        {
            get
            {
                return GetFiles(null);
            }
        }

        private List<string> xmls { get; set; }

        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/method[@name="BuildFileName"]/*'/> 
        public bool BuildFileName(string name)
        {
            var temp = name.Split('.').ToList();
            FileName = temp.LastOrDefault();
            temp.RemoveAt(temp.Count - 1);
            SubDirPath = Path.Combine(temp.ToArray());
            return DirectoryExists(true);
        }

        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/method[@name="DirectoryExists"]/*'/> 
        public bool DirectoryExists(bool create)
        {
            if (create)
            {
                try
                {
                    return IsDirectory;
                }
                catch (DirectoryNotFoundException)
                {
                    Directory.CreateDirectory(FullDirectory);
                }
            }
            return IsDirectory;
        }

        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/method[@name="FileExists"]/*'/> 
        public bool FileExists(bool create)
        {
            if (!create && !IsDirectory)
            {
                return false;
            }
            if (create)
            {
                try
                {
                    return IsFile;
                }
                catch (FileNotFoundException)
                {
                    File.Create(FullPath);
                }
            }
            else
            {
                try
                {
                    if (IsFile)
                    {
                        File.Delete(FullPath);
                    }
                }
                catch (FileNotFoundException)
                {
                   
                }
            }
            return IsFile;
        }

        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/method[@name="GetFiles"][@version="All"]/*'/>
        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/method[@name="GetFiles"][@version="Filtered"]/*'/> 
        public List<string> GetFiles(string filter)
        {
            if (true)
            {

            }
            /*var options = new EnumerationOptions()
            {
                //MatchCasing = MatchCasing.CaseInsensitive,
                RecurseSubdirectories = true,
                IgnoreInaccessible = true,
            };*/
            return GetFileNames(filter, SearchOption.AllDirectories);
        }

        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/method[@name="GetFiles"][@version="All"]/*'/>
        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/method[@name="GetFiles"][@version="Options"]/*'/> 
        public List<string> GetFilePaths(string filter, SearchOption options)
        {
            try
            {
                if (!IsDirectory)
                {
                    return null;
                }
                return Directory.GetFiles(FullDirectory, filter, SearchOption.AllDirectories).ToList();
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/method[@name="GetFiles"][@version="All"]/*'/>
        /// <include file='./docs/Models.xml' path='doc/models[@name="FileDetails"]/method[@name="GetFiles"][@version="Names"]/*'/> 
        public List<string> GetFileNames(string filter, SearchOption options)
        {
            return GetFilePaths(filter, options).Select(x => x.Replace(FullDirectory, "")).ToList();
        }

        private bool checkExtension(List<string> supported)
        {
            return supported.Contains(FileExt.ToLower()) ? true : throw new FileNotFoundException();
        }

        private bool checkExtension(string ext)
        {
            return checkExtension(new List<string>() { ext });
        }
    }
}
