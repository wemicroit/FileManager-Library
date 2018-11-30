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
        private FileDetails readerInfo {get; set;}
        private FileDetails writerInfo { get; set; }
        private FileDetails transformerInfo { get; set; }
        private FileDetails templateInfo { get; set; }

        private JSONConversion jSONConverter { get; set; }
        private XMLConversion xMLConverter { get; set; }

        public FileManager()
        {
            //CSVConverter = new CSVConverter.CSVConverter();
            jSONConverter = new JSONConversion();
            xMLConverter = new XMLConversion();

            readerInfo = new FileDetails();
            writerInfo = new FileDetails();
            transformerInfo = new FileDetails();
            templateInfo = new FileDetails();
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