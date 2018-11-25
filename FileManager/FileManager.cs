using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using WeMicroIt.Utils.CSVConverter;
using WeMicroIt.Utils.FileConverter.Interfaces;

namespace WeMicroIt.Utils.FileConverter
{
    public partial class FileManager : IFileManager
    {
        private static bool MultiAction { get; set; }

        private static StreamReader Reader { get; set; }
        private static StreamWriter Writer { get; set; }
        private string readerPath { get; set; }
        private string writerPath { get; set; }

        public static JSONConverter.JSONConverter JSONConverter { get; set; }
        public static XMLConverter.XMLConverter XMLConverter { get; set; }

        public FileManager()
        {
            readerPath = Directory.GetCurrentDirectory();
            //CSVConverter = new CSVConverter.CSVConverter();
            JSONConverter = new JSONConverter.JSONConverter();
            XMLConverter = new XMLConverter.XMLConverter();
        }

        public bool CheckDirectory()
        {
            return CheckDirectory(false);
        }

        public bool CheckDirectory(bool create)
        {
            try
            {
                if (string.IsNullOrEmpty(readerPath))
                {
                    throw new ArgumentNullException();
                }
                if (create)
                {
                    Directory.CreateDirectory(readerPath);
                }
                if (string.IsNullOrEmpty(writerPath))
                {
                    throw new ArgumentNullException();
                }
                if (create)
                {
                    Directory.CreateDirectory(writerPath);
                }

                return (Directory.Exists(readerPath) || Directory.Exists(writerPath));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckFile()
        {
            return CheckFile(readerPath);
        }

        public bool CheckFile(string path)
        {
            return CheckFile(path, false);
        }

        public bool CheckFile(string path, bool create)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    throw new ArgumentNullException();
                }
                if (CheckDirectory(create))
                {
                    if (File.Exists(path))
                    {
                        return true;
                    }
                    else if (create)
                    {
                        File.Create(path);
                        return File.Exists(path);
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