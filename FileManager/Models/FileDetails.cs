﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WeMicroIt.Utils.FileConverter.Models
{
    public class FileDetails
    {
        public string DirPath { get; set; }
        public string SubDirPath { get; set; }
        public string FileName { get; set; } = "temp";
        public string FileExt { get; set; } = "xml";
        public string Section { get; set; }
        public string SubSection { get; set; }
        private static List<string> xmls { get; set; }

        public FileDetails()
        {
            DirPath = null;
            FileName = null;
            FileExt = null;

            xmls = new List<string>() { "xml", "html" };
        }

        public string FullFileName
        {
            get
            {
                return string.Join(".", FileName, FileExt);
            }
        }

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

        public string FullPath {
            get {
                if (string.IsNullOrEmpty(DirPath))
                {
                    return FullFileName;
                }
                return Path.Combine(FullDirectory, FullFileName);
            }
            set {
                if (string.IsNullOrEmpty(value))
                {
                    DirPath = null;
                    FileExt = null;
                    FileName = null;
                    SubDirPath = null;
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
                FileExt = value.Substring(ext+ 1);
            }
        }

        public void BuildFileName(string name)
        {
            var temp = name.Split('.').ToList();
            FileName = temp.LastOrDefault();
            temp.RemoveAt(temp.Count - 1);
            SubDirPath = Path.Combine(temp.ToArray());
            CheckDirectory(true);
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
                return checkExtension(xmls);
            }
        }

        private bool checkExtension(List<string> supported)
        {
            return supported.Contains(FileExt.ToLower())? true: throw new FileNotFoundException();
        }

        private bool checkExtension(string ext)
        {
            return checkExtension(new List<string>() { ext });
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
                Directory.CreateDirectory(FullDirectory);
            }
            return Directory.Exists(FullDirectory) ? true: throw new DirectoryNotFoundException();
        }

        public bool CheckFile()
        {
            return CheckFile(false);
        }

        public bool CheckFile(bool create)
        {
            if (string.IsNullOrEmpty(FileName) || string.IsNullOrEmpty(FileExt))
            {
                throw new ArgumentNullException();
            }
            CheckDirectory(create);
            if (create)
            {
                File.Create(FullPath);
            }
            return File.Exists(FullPath)? true : throw new FileNotFoundException();
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
            return GetFileNames(filter, SearchOption.AllDirectories);
        }

        public List<string> GetFilePaths(string filter, SearchOption options)
        {
            try
            {
                CheckDirectory();
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

        public List<string> GetFileNames(string filter, SearchOption options)
        {
            return GetFilePaths(filter, options).Select(x => x.Replace(FullDirectory, "")).ToList();
        }


        public bool RemoveFile()
        {
            File.Delete(FullPath);
            return File.Exists(FullPath);
        }
    }
}
