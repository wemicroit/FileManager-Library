using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using WeMicroIt.Utils.FileConverter.Interfaces;
using WeMicroIt.Utils.FileConverter.Resources.Enums;

namespace WeMicroIt.Utils.FileConverter
{
    public partial class FileManager : IFileManager
    {
        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Read"][@version="File"]/*'/> 
        public string ReadFile()
        {
            read(FileIOType.Block, out object data);
            return (string)data;
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Read"][@version="Line"]/*'/> 
        public string ReadLine()
        {
            read(FileIOType.Line, out object data);
            return (string)data;
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Read"][@version="Lines"]/*'/> 
        public List<string> ReadLines()
        {
            read(FileIOType.Lines, out object data);
            return (List<string>)data;
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Read"][@version="CSV"]/*'/> 
        public List<T> ReadCSV<T>()
        {
            return ReadCSV<T>(true);
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Read"][@version="CSVHeader"]/*'/> 
        public List<T> ReadCSV<T>(bool headers)
        {
            if (readerInfo.IsCSV)
            {
                throw new NotImplementedException();
            }
            throw new NotSupportedException();
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Read"][@version="JSON"]/*'/> 
        public List<T> ReadJSON<T>()
        {
            if (readerInfo.IsJSON)
            {
                string data = ReadFile();
                throw new NotImplementedException();
            }
            throw new NotSupportedException();
        }

        /// <include file='./docs/Classes.xml' path='doc/classes[@name="FileManager"]/methods[@name="Read"][@version="XML"]/*'/> 
        public XDocument ReadXML()
        {
            if (readerInfo.IsXML)
            {
                return xMLConverter.DeSerializeDocument(Reader);
            }
            throw new NotSupportedException();
        }

        private bool read(FileIOType ioType, out object content)
        {
            content = null;
            using (StreamReader reader = new StreamReader(Reader))
            {
                switch (ioType)
                {
                    case FileIOType.Block:
                        content = reader.ReadToEnd();
                        return true;
                    case FileIOType.Lines:
                        var listing = new List<string>();
                        while (!reader.EndOfStream)
                        {
                            listing.Add(reader.ReadLine());
                        }
                        content = listing;
                        return true;
                    case FileIOType.Line:
                        content = reader.ReadLine();
                        return true;
                    case FileIOType.Data:
                        content = reader.Read();
                        return true;
                    case FileIOType.Stream:
                        string temp = reader.ReadToEnd();
                        content = reader.BaseStream;
                        return true;
                }
            }
            return false;
        }
    }
}
