using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using WeMicroIt.Utils.FileConverter.Interfaces;
using WeMicroIt.Utils.FileConverter.Resources.Enums;

namespace WeMicroIt.Utils.FileConverter
{
    public partial class FileManager : IFileManager
    {
        private bool write(object content, bool append, FileIOType ioType)
        {
            if (content == null)
            {
                throw new ArgumentNullException();
            }
            if (!WriterInfo.CheckDirectory())
            {
                throw new DirectoryNotFoundException();
            }
            using (StreamWriter writer = new StreamWriter(WriterInfo.FullPath, append))
            {
                switch (ioType)
                {
                    case FileIOType.Block:
                        writer.Write(content);
                        break;
                    case FileIOType.Lines:
                        writer.Write(content);
                        break;
                    case FileIOType.Line:
                        writer.WriteLine(content);
                        break;
                    case FileIOType.Data:
                        writer.Write(content);
                        break;
                    default:
                        break;
                }
                
            }
            return true; ;
        }

        public bool WriteBlock(string contents)
        {
            return write(contents, false, FileIOType.Block);
        }

        public bool WriteLine(string contents)
        {
            return WriteLine(contents, false);
        }

        public bool WriteLine(string contents, bool append)
        {
            return write(string.Join(contents, "\r\n"), append, FileIOType.Line);
        }

        public bool WriteLines(List<string> contents)
        {
            return WriteLines(contents, false);
        }

        public bool WriteLines(List<string> contents, bool append)
        {
            return write(string.Join("\r\n", contents), append, FileIOType.Lines);
        }

        public bool AppendCSV<T>(List<T> data)
        {
            return WriteCSV<T>(data, true);
        }

        public bool WriteCSV<T>(List<T> data)
        {
            return WriteCSV<T>(data, false);
        }

        public bool WriteCSV<T>(List<T> data, bool Append)
        {
            if (WriterInfo.IsCSV)
            {
                return WriteLines(null, Append);
                //return WriteLines(CSVConverter.SerializeBlock<T>(data), Append);
                throw new NotImplementedException();
            }
            throw new NotSupportedException();
        }

        public bool AppendJSON<T>(List<T> data)
        {
            if (WriterInfo.IsJSON)
            {
                var fullData = ReadJSON<T>();
                if (fullData == null)
                {
                    fullData = new List<T>();
                }
                fullData.AddRange(data);
                return WriteJSON<T>(fullData);
            }
            throw new NotSupportedException();
        }

        public bool WriteJSON<T>(List<T> data)
        {
            if (WriterInfo.IsJSON)
            {
                return write(JSONConverter.SerializeObjects(data), false, FileIOType.Block);
            }
            throw new NotSupportedException();
        }

        public bool AppendXML<T>(List<T> data)
        {
            if (WriterInfo.IsXML)
            {
                var fullData = ReadXML<T>();
                if (fullData == null)
                {
                    //fullData = new List<T>();
                }
                //fullData.AddRange(data);
                return WriteXML<T>(fullData);
            }
            throw new NotSupportedException();
        }

        public bool WriteXML(XElement data)
        {
            if (WriterInfo.IsXML)
            {
                return write(data.ToString(),false, FileIOType.Block);
            }
            throw new NotSupportedException();
        }

        public bool WriteXML<T>(T data)
        {
            if (WriterInfo.IsXML)
            {
                return write(XMLConverter.SerializeObjects(data),false, FileIOType.Block);
            }
            throw new NotSupportedException();
        }
    }
}
