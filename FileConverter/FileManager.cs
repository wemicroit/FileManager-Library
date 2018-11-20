using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WeMicroIt.Utils.CSVConverter;
using WeMicroIt.Utils.FileConverter.Interfaces;

namespace WeMicroIt.Utils.FileConverter
{
    public class FileManager : IFileManager
    {
        private static string DirectoryPath { get; set; }
        private static string FilePath { get; set; }

        public static CSVConverter.CSVConverter CSVConverter { get; set; } = new CSVConverter.CSVConverter();

        public FileManager()
        {
            DirectoryPath = Directory.GetCurrentDirectory();
        }

        public bool SetDirectoryPath(string path)
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
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SetFilePath(string path)
        {
            try
            {
                FilePath = path;
                return true;
            }
            catch (Exception)
            {
                return false;
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
                return Directory.GetFiles(GetFullPath(),"").Select(x => x.Replace(DirectoryPath, "")).ToList();
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

        public string ReadFile()
        {
            try
            {
                if (!CheckFile())
                {
                    throw new FileNotFoundException();
                }
                string contents = "";
                using (StreamReader file = new StreamReader(GetFullPath()))
                {
                    contents = file.ReadToEnd();
                }
                return contents;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string ReadLine()
        {
            try
            {
                if (!CheckFile())
                {
                    throw new FileNotFoundException();
                }
                string contents = "";
                using (StreamReader file = new StreamReader(GetFullPath()))
                {
                    contents = file.ReadLine();
                }
                return contents;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<string> ReadLines()
        {
            try
            {
                if (!CheckFile())
                {
                    throw new FileNotFoundException();
                }
                List<string> contents = new List<string>();
                using (StreamReader file = new StreamReader(GetFullPath()))
                {
                    while (!file.EndOfStream)
                    {
                        contents.Add(file.ReadLine());
                    }
                }
                return contents;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<T> ReadCSV<T>()
        {
            return ReadCSV<T>(true);
        }

        public List<T> ReadCSV<T>(bool headers)
        {
            try
            {
                if (!FilePath.EndsWith(".csv"))
                {
                    throw new FileNotFoundException();
                }
                return CSVConverter.DeserialiseBlock<T>(ReadFile(), headers);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private bool Write(string contents, bool append)
        {
            try
            {
                using (StreamWriter file = new StreamWriter(GetFullPath(), append))
                {
                    file.Write(contents);
                }
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool WriteBlock(string contents)
        {
            return Write(contents, false);
        }

        public bool WriteLine(string contents)
        {
            return Write(contents, true);
        }

        public bool WriteLine(string contents, bool append)
        {
            try
            {
                using (StreamWriter file = new StreamWriter(GetFullPath(), append))
                {
                    file.WriteLine(contents);
                }
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool WriteLines(List<string> contents)
        {
            return WriteLines(contents, true);
        }

        public bool WriteLines(List<string> contents, bool append)
        {
            int written = 0;
            foreach (var item in contents)
            {
                if (WriteLine(item, append))
                {
                    written++;
                }
            }
            return written == contents.Count;
        }

        public bool WriteCSV<T>(List<T> data)
        {
            try
            {
                if (!FilePath.EndsWith(".csv"))
                {
                    throw new FileNotFoundException();
                }
                if (data == null)
                {
                    throw new ArgumentNullException();
                }
                return WriteLines(CSVConverter.SerializeBlock<T>(data));
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
