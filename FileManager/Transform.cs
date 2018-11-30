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
            return xMLConverter.Transforms(Reader, Templater, Writer);
        }

        public bool SplitXML(string path)
        {
            foreach (var item in xMLConverter.PureSplits(path, Reader))
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
            string fileT = writerInfo.FileExt;
            foreach (var item in xMLConverter.GroupSplits(path, Reader, name))
            {
                //Build the xml file to feed the splitting
                writerInfo.FileName = "temp";
                writerInfo.FileExt = "xml";
                WriteXML(item);

                //now read the file to power transform
                Reader = Writer;
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
                XDocument doc = XDocument.Load(ReaderInfo.FullPath);
                WriterInfo.FullPath = ReaderInfo.FullPath;
                WriterInfo.FileExt = "md";
                WriteBlock(doc.Document.ToString());
                if (removeOld)
                {
                    ReaderInfo.RemoveFile();
                }
                Reader = item;
            }
            return true;
        }

        public bool ConvertXMLToRaw()
        {
            return true;
        }
    }
}
