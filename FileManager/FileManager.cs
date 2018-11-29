using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using WeMicroIt.Utils.CSVConverter;
using WeMicroIt.Utils.FileConverter.Interfaces;
using WeMicroIt.Utils.FileConverter.Models;
using WeMicroIt.Utils.JSONConverter;
using WeMicroIt.Utils.XMLConverter;

namespace WeMicroIt.Utils.FileConverter
{
    public partial class FileManager : IFileManager
    {
        private static bool MultiAction { get; set; }

        public static FileDetails ReaderInfo {get; set;}
        public static FileDetails WriterInfo { get; set; }
        public static FileDetails TransformerInfo { get; set; }
        public static FileDetails TemplateInfo { get; set; }

        public static JSONConversion JSONConverter { get; set; }
        public static XMLConversion XMLConverter { get; set; }

        public FileManager()
        {
            //CSVConverter = new CSVConverter.CSVConverter();
            JSONConverter = new JSONConversion();
            XMLConverter = new XMLConversion();

            ReaderInfo = new FileDetails();
            WriterInfo = new FileDetails();
            TransformerInfo = new FileDetails();
            TemplateInfo = new FileDetails();

            ReaderInfo.FullPath = Directory.GetCurrentDirectory();
            TransformerInfo.FullPath = Directory.GetCurrentDirectory();
        }

        public bool setFiles(string read, string write, string template)
        {
            if (!string.IsNullOrEmpty(read))
            {
                ReaderInfo.FullPath = read;
            }
            if (!string.IsNullOrEmpty(write))
            {
                WriterInfo.FullPath = write;
            }
            if (!string.IsNullOrEmpty(template))
            {
                TemplateInfo.FullPath = template;
            }
            return true;
        }

        public bool FilesSet(bool read, bool write, bool template, bool create)
        {
            if (read)
            {
                ReaderInfo.CheckDirectory(create);
            }
            if (write)
            {
                WriterInfo.CheckDirectory(create);
            }
            if (template)
            {
                TemplateInfo.CheckFile();
            }
            return true;
        }
    }
}