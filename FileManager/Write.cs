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
        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Write"][@version="All"]/*'/> 
        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Write"][@version="Block"]/*'/> 
        public bool WriteBlock(string contents)
        {
            return write(contents, false, FileIOType.Block);
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Write"][@version="All"]/*'/> 
        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Write"][@version="Line"]/*'/> 
        public bool WriteLine(string contents)
        {
            return WriteLine(contents, false);
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Write"][@version="All"]/*'/> 
        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Append"][@version="Line"]/*'/> 
        public bool WriteLine(string contents, bool append)
        {
            return write(string.Join(contents, "\r\n"), append, FileIOType.Line);
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Write"][@version="All"]/*'/> 
        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Write"][@version="Lines"]/*'/> 
        public bool WriteLines(List<string> contents)
        {
            return WriteLines(contents, false);
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Write"][@version="All"]/*'/> 
        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Append"][@version="Lines"]/*'/> 
        public bool WriteLines(List<string> contents, bool append)
        {
            return write(string.Join("\r\n", contents), append, FileIOType.Lines);
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Write"][@version="All"]/*'/> 
        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Write"][@version="CSV"]/*'/> 
        public bool WriteCSV<T>(List<T> data)
        {
            return WriteCSV<T>(data, false);
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Write"][@version="All"]/*'/> 
        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Append"][@version="CSV"]/*'/> 
        public bool WriteCSV<T>(List<T> data, bool append)
        {
            if (writerInfo.IsCSV)
            {
                return WriteLines(null, append);
                //return WriteLines(CSVConverter.SerializeBlock<T>(data), Append);
                throw new NotImplementedException();
            }
            throw new NotSupportedException();
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Write"][@version="All"]/*'/> 
        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Write"][@version="JSON"]/*'/> 
        public bool WriteJSON<T>(List<T> data)
        {
            return WriteJSON<T>(data, false);
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Write"][@version="All"]/*'/> 
        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Append"][@version="JSON"]/*'/> 
        public bool WriteJSON<T>(List<T> data, bool append)
        {
            if (writerInfo.IsJSON)
            {
                var fullData = append? ReadJSON<T>() : new List<T>();
                if (fullData == null)
                {
                    fullData = new List<T>();
                }
                fullData.AddRange(data);
                return write(jSONConverter.SerializeObjects(fullData), false, FileIOType.Block);
            }
            throw new NotSupportedException();
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Write"][@version="All"]/*'/> 
        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Write"][@version="XMLDoc"]/*'/> 
        public bool WriteXML(XElement data)
        {
            return WriteXML(data, false);
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Write"][@version="All"]/*'/> 
        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Append"][@version="XMLDoc"]/*'/> 
        public bool WriteXML(XElement data, bool append)
        {
            if (writerInfo.IsXML)
            {
                XDocument fullData = append ? ReadXML() : new XDocument();
                fullData.AddAfterSelf(data);
                return write(fullData.ToString(), false, FileIOType.Block);
            }
            throw new NotSupportedException();
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Write"][@version="All"]/*'/> 
        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Write"][@version="XMLObj"]/*'/> 
        public bool WriteXML<T>(List<T> data)
        {
            return WriteXML<T>(data, false);
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Write"][@version="All"]/*'/> 
        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Append"][@version="XMLObj"]/*'/> 
        public bool WriteXML<T>(List<T> data, bool append)
        {
            if (writerInfo.IsXML)
            {
                var fullData = append? new List<T>() : new List<T>();
                if (fullData == null)
                {
                    fullData = new List<T>();
                }
                fullData.AddRange(data);
                return write(xMLConverter.SerializeObjects(fullData), false, FileIOType.Block);
            }
            throw new NotSupportedException();
        }

        private bool write(object content, bool append, FileIOType ioType)
        {
            if (content == null)
            {
                throw new ArgumentNullException();
            }
            if (!writerInfo.IsDirectory)
            {
                throw new DirectoryNotFoundException();
            }
            using (StreamWriter writer = new StreamWriter(Writer, append))
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
    }
}
