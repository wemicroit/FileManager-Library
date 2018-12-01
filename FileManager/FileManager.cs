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
    /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/class/*'/> 
    public partial class FileManager : IFileManager
    {
        private FileDetails readerInfo {get; set;}
        private FileDetails writerInfo { get; set; }
        private FileDetails transformerInfo { get; set; }
        private FileDetails templateInfo { get; set; }

        private JSONConversion jSONConverter { get; set; }
        private XMLConversion xMLConverter { get; set; }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/constructor[@name="Default"]/*'/> 
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

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/properties[@name="Reader"]/*'/> 
        public string Reader
        {
            get
            {
                return readerInfo.FullPath;
            }
            set{
                if (!string.IsNullOrEmpty(value))
                {
                    readerInfo.FullPath = value;
                }
                throw new ArgumentNullException();
            }
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/properties[@name="Writer"]/*'/> 
        public string Writer
        {
            get
            {
                return writerInfo.FullPath;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    writerInfo.FullPath = value;
                }
                throw new ArgumentNullException();
            }
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/properties[@name="Templater"]/*'/> 
        public string Templater
        {
            get
            {
                return templateInfo.FullPath;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    templateInfo.FullPath = value;
                }
            }
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/properties[@name="FilesExist"]/*'/> 
        public bool FilesExist
        {
            get
            {
                readerInfo.DirectoryExists(true);
                writerInfo.DirectoryExists(true);
                if (Templater != null)
                {
                    return templateInfo.IsFile;
                }
                return true;
            }
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="SetAllFiles"]/*'/> 
        public bool SetAllFiles(string read, string write, string template)
        {
            Reader = read;
            Writer = write;
            Templater = template;
            return FilesExist;
        }

    }
}