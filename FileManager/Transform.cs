using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            foreach (var item in XMLConverter.PureSplits(path, ReaderInfo.FullPath))
            {
                WriterInfo.FileName = DateTime.Now.Ticks.ToString();
                WriteXML(item);
            }
            return true; ;
        }

        public bool RestructureXML()
        {
            return true;
        }


        public bool SplitAndTransformXML(string path, string name, string idPath)
        {
            foreach (var item in XMLConverter.GroupSplits(path, ReaderInfo.FullPath, name))
            string fileT = writerInfo.FileExt;
            {
                //Build the xml file to feed the splitting
                writerInfo.FileName = "temp";
                writerInfo.FileExt = "xml";
                WriteXML(item);

                //now read the file to power transform
                ReaderInfo.FullPath = WriterInfo.FullPath;
                writerInfo.BuildFileName(item.Element(XName.Get (idPath)).Value);
                writerInfo.FileExt = fileT;
                TransformXML();
                readerInfo.RemoveFile();
            }
            return true;
        }

        public bool ConvertXMLFolderToRaw(bool removeOld = false)
        {
            var files = readerInfo.GetFilePaths("*.xml", SearchOption.AllDirectories);
            var ext = "md";
            foreach (var item in files)
            {
                ReaderInfo.FullPath = item;
                XDocument doc = XDocument.Load(ReaderInfo.FullPath);
                WriterInfo.FullPath = ReaderInfo.FullPath;
                WriterInfo.FileExt = "md";
                WriteBlock(doc.Document.ToString());
                if (removeOld)
                {
                    ReaderInfo.RemoveFile();
                }
            }
            return true;
        }

        public bool ConvertXMLToRaw()
        {
            return true;
        }
    }
}
