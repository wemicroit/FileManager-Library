using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using WeMicroIt.Utils.FileConverter.Interfaces;

namespace WeMicroIt.Utils.FileConverter
{
    public partial class FileManager : IFileManager
    {
        public bool TransformXML()
        {
            return XMLConverter.Transforms(ReaderInfo.FullPath, TemplateInfo.FullPath, WriterInfo.FullPath);
        }

        public bool SplitXML(string path)
        {
            WriterInfo.FileExt = "xml";
            foreach (var item in XMLConverter.Splits(path, ReaderInfo.FullPath))
            {
                WriterInfo.FileName = DateTime.Now.Ticks.ToString();
                WriteXML(item);
            }
            return true; ;
        }


        public bool SplitAndTransformXML(string path)
        {
            string fileT = WriterInfo.FileExt;
            foreach (var item in XMLConverter.Splits(path, ReaderInfo.FullPath))
            {
                WriterInfo.FileName = "temp";
                WriterInfo.FileExt = "xml";
                WriteXML(item);
                ReaderInfo.FullPath = WriterInfo.FullPath;
                WriterInfo.FileName = DateTime.Now.Ticks.ToString();
                WriterInfo.FileExt = fileT;
                TransformXML();
            }
            return true;
        }
    }
}
