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
            //WriterInfo.FileExt = "xml";
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
            string fileT = WriterInfo.FileExt;
            foreach (var item in XMLConverter.GroupSplits(path, ReaderInfo.FullPath, name))
            {
                //Build the xml file to feed the splitting
                WriterInfo.FileName = "temp";
                WriterInfo.FileExt = "xml";
                WriteXML(item);

                //now read the file to power transform
                ReaderInfo.FullPath = WriterInfo.FullPath;
                WriterInfo.BuildFileName(item.Element(XName.Get (idPath)).Value);
                WriterInfo.FileExt = fileT;
                TransformXML();
                ReaderInfo.RemoveFile();
            }
            return true;
        }

        public bool ConvertXMLFolderToRaw(bool removeOld = false)
        {
            var files = ReaderInfo.GetFilePaths("*.xml", SearchOption.AllDirectories);
            WriterInfo.SubDirPath = null;
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
