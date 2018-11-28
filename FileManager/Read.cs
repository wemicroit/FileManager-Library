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
using WeMicroIt.Utils.FileConverter.Resource;

namespace WeMicroIt.Utils.FileConverter
{
    public partial class FileManager : IFileManager
    {
        private bool read(FileIOType ioType, out object content)
        {
            content = null;
            using (StreamReader reader = new StreamReader(ReaderInfo.FullPath))
            {
                switch (ioType)
                {
                    case FileIOType.Block:
                        content = reader.ReadToEnd();
                        return true;
                    case FileIOType.Lines:
                        var listing = new List<string>();
                        while(!reader.EndOfStream)
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

        public string ReadFile()
        {
            object data;
            read(FileIOType.Block, out data);
            return (string)data;
        }

        public string ReadLine()
        {
            object data;
            read(FileIOType.Line, out data);
            return (string)data;
        }

        public List<string> ReadLines()
        {
            object data;
            read(FileIOType.Lines, out data);
            return (List<string>)data;
        }

        public List<T> ReadCSV<T>()
        {
            return ReadCSV<T>(true);
        }

        public List<T> ReadCSV<T>(bool headers)
        {
            if (ReaderInfo.IsCSV)
            {
                throw new NotImplementedException();
            }
            throw new NotSupportedException();
        }

        public List<T> ReadJSON<T>()
        {
            if (ReaderInfo.IsJSON)
            {
                string data = ReadFile();
                throw new NotImplementedException();
            }
            throw new NotSupportedException();
        }

        public T ReadXML<T>()
        {
            if (ReaderInfo.IsXML)
            {
                return XMLConverter.DeSerializeObjects<T>(ReaderInfo.FullPath);
            }
            throw new NotSupportedException();
        }
    }
}
