using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WeMicroIt.Utils.FileConverter.Models
{
    public class FileDetails
    {
        public string DirPath { get; set; }
        public string FileName { get; set; } = "temp";
        public string FileExt { get; set; } = "xml";

        public FileDetails()
        {
            DirPath = null;
            FileName = null;
            FileExt = null;
        }

        public string FullPath {
            get {
                return Path.Combine(DirPath, string.Join(".", FileName, FileExt));
            }
            set {
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
                FileName = value.Substring(index + 1, ext - index - 1);
                FileExt = value.Substring(ext+ 1);
            }
        }

        public bool IsCSV
        {
            get
            {
                return checkExtension("csv");
            }
        }

        public bool IsJSON
        {
            get
            {
                return checkExtension("json");
            }
        }

        public bool IsXML
        {
            get
            {
                return checkExtension("xml");
            }
        }

        private bool checkExtension(string ext)
        {
            return FileExt.ToLower() == ext? true: throw new FileNotFoundException();
        }

        public bool CheckDirectory()
        {
            return CheckDirectory(false);
        }

        public bool CheckDirectory(bool create)
        {
            if (string.IsNullOrEmpty(DirPath))
            {
                throw new ArgumentNullException();
            }
            if (create)
            {
                Directory.CreateDirectory(this.DirPath);
            }
            return Directory.Exists(DirPath) ? true: throw new DirectoryNotFoundException();
        }

        public bool CheckFile()
        {
            return CheckFile(false);
        }

        public bool CheckFile(bool create)
        {
            if (string.IsNullOrEmpty(this.FileName) || string.IsNullOrEmpty(this.FileExt))
            {
                throw new ArgumentNullException();
            }
            CheckDirectory(create);
            if (create)
            {
                File.Create(this.FullPath);
            }
            return File.Exists(this.FullPath)? true : throw new FileNotFoundException();
        }

        public List<string> GetFiles()
        {
            return GetFiles(null);
        }

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
            return GetFiles(filter, null);
        }

        public List<string> GetFiles(string filter, string options)
        {
            try
            {
                CheckDirectory();
                return Directory.GetFiles(this.DirPath, "").Select(x => x.Replace(this.DirPath, "")).ToList();
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

    }
}
