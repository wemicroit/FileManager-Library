using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WeMicroIt.Utils.CSVConverter;
using WeMicroIt.Utils.FileConverter.Interfaces;

namespace WeMicroIt.Utils.FileConverter
{
    public partial class FileManager : IFileManager
    {
        private static string DirectoryPath { get; set; }
        private static string FilePath { get; set; }
        private static bool MultiAction { get; set; }

        private static StreamReader Reader { get; set; }
        private static StreamWriter Writer { get; set; }


        public static CSVConverter.CSVConverter CSVConverter { get; set; }
        public static JSONConverter.JSONConverter JSONConverter { get; set; }
        public static XMLConverter.XMLConverter XMLConverter { get; set; }

        public FileManager()
        {
            DirectoryPath = Directory.GetCurrentDirectory();
            CSVConverter = new CSVConverter.CSVConverter();
            JSONConverter = new JSONConverter.JSONConverter();
            XMLConverter = new XMLConverter.XMLConverter();
        }

        public string SetDirectoryPath(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    DirectoryPath = Directory.GetCurrentDirectory();
                }
                else
                {
                    DirectoryPath = path;
                }
                return DirectoryPath;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string SetFilePath(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    FilePath = DateTimeOffset.Now.Date.ToShortDateString();
                }
                else
                {
                    FilePath = path;
                }
                return FilePath;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetFullPath()
        {
            return Path.Combine(DirectoryPath, FilePath);
        }

        public List<string> GetFiles()
        {
            return GetFiles(null);
        }

        public List<string> GetFiles(string filter)
        {
            var options = new EnumerationOptions()
            {
                MatchCasing = MatchCasing.CaseInsensitive,
                RecurseSubdirectories = true,
                IgnoreInaccessible = true,
            };
            return GetFiles(filter, options);
        }

        public List<string> GetFiles(string filter, EnumerationOptions options)
        {
            try
            {
                if (!CheckDirectory())
                {
                    throw new DirectoryNotFoundException();
                }
                return Directory.GetFiles(DirectoryPath,"").Select(x => x.Replace(DirectoryPath, "")).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool CheckDirectory()
        {
            return CheckDirectory(false);
        }

        public bool CheckDirectory(bool create)
        {
            try
            {
                if (string.IsNullOrEmpty(DirectoryPath))
                {
                    throw new ArgumentNullException();
                }
                if (create)
                {
                    Directory.CreateDirectory(DirectoryPath);
                }
                return (Directory.Exists(DirectoryPath));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckFile()
        {
            return CheckFile(false);
        }

        public bool CheckFile(bool create)
        {
            try
            {
                if (CheckDirectory(create))
                {
                    if (string.IsNullOrEmpty(FilePath))
                    {
                        throw new ArgumentNullException();
                    }
                    if (File.Exists(GetFullPath()))
                    {
                        return true;
                    }
                    else if (create)
                    {
                        File.Create(GetFullPath());
                        return File.Exists(GetFullPath());
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}