﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using WeMicroIt.Utils.CSVConverter;
using WeMicroIt.Utils.FileConverter.Interfaces;
using WeMicroIt.Utils.FileConverter.Models;

namespace WeMicroIt.Utils.FileConverter
{
    public partial class FileManager : IFileManager
    {
        private static bool MultiAction { get; set; }

        private static StreamReader Reader { get; set; }
        private static StreamWriter Writer { get; set; }
        public static FileDetails ReaderInfo {get; set;}
        public static FileDetails WriterInfo { get; set; }
        public static FileDetails TransformerInfo { get; set; }
        public static FileDetails TemplateInfo { get; set; }

        public static JSONConverter.JSONConverter JSONConverter { get; set; }
        public static XMLConverter.XMLConverter XMLConverter { get; set; }

        public FileManager()
        {
            //CSVConverter = new CSVConverter.CSVConverter();
            JSONConverter = new JSONConverter.JSONConverter();
            XMLConverter = new XMLConverter.XMLConverter();

            ReaderInfo = new FileDetails();
            WriterInfo = new FileDetails();
            TransformerInfo = new FileDetails();
            TemplateInfo = new FileDetails();

            ReaderInfo.FullPath = Directory.GetCurrentDirectory();
            TransformerInfo.FullPath = Directory.GetCurrentDirectory();
        }

        public bool setFiles(string read, string write, string transform, string template)
        {
            if (!string.IsNullOrEmpty(read))
            {
                ReaderInfo.FullPath = read;
            }
            if (!string.IsNullOrEmpty(write))
            {
                WriterInfo.FullPath = write;
            }
            if (!string.IsNullOrEmpty(transform))
            {
                TransformerInfo.FullPath = transform;
            }
            if (!string.IsNullOrEmpty(template))
            {
                TemplateInfo.FullPath = template;
            }
            return true;
        }

        public bool FilesSet(bool read, bool write, bool transform, bool template)
        {
            if (read)
            {
                ReaderInfo.CheckFile();
            }
            if (write)
            {
                WriterInfo.CheckDirectory();
            }
            if (transform)
            {
                TransformerInfo.CheckDirectory();
            }
            if (template)
            {
                TemplateInfo.CheckFile();
            }
            return true;
        }
    }
}