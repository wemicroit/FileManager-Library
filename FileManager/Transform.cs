using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using WeMicroIt.Utils.FileConverter.Interfaces;

namespace WeMicroIt.Utils.FileConverter
{
    public partial class FileManager : IFileManager
    {
        public bool TransformXML(bool persist = false)
        {
            string end = TransformerInfo.FullPath;
            if (persist)
            {
                end = WriterInfo.FullPath;
            }
            if(!XMLConverter.TransformObjects(ReaderInfo.FullPath, TemplateInfo.FullPath, end))
            {
                return false;
            }
            ReaderInfo.FullPath = TransformerInfo.FullPath;
            return true;
        }
    }
}
