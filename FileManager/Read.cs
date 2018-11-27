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

namespace WeMicroIt.Utils.FileConverter
{
    public partial class FileManager : IFileManager
    {
        private bool StartRead()
        {
            FinishWrite();
            if (reader == null)
            {
                reader = ReaderInfo.CheckFile() ? new StreamReader(ReaderInfo.FullPath) : null;
            }
            return reader != null ? true : throw new FileNotFoundException();
        }

        private bool FinishRead()
        {
            if (!MultiAction && reader != null)
            {
                reader.Close();
                reader.Dispose();
                reader = null;
            }
            return reader == null;
        }

        public string ReadFile()
        {
            try
            {
                StartRead();
                return reader.ReadToEnd();
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                FinishRead();
            }
        }

        public Stream ReadStream()
        {
            try
            {
                StartRead();
                reader.ReadToEnd();
                return reader.BaseStream;
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                FinishRead();
            }
        }

        public string ReadLine()
        {
            try
            {
                StartRead();
                return reader.ReadLine();
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                FinishRead();
            }
        }

        public List<string> ReadLines()
        {
            try
            {
                List<string> contents = new List<string>();
                while (!reader.EndOfStream)
                {
                    contents.Add(ReadLine());
                }
                return contents;
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                FinishRead();
            }
        }

        public List<T> ReadCSV<T>()
        {
            return ReadCSV<T>(true);
        }

        public List<T> ReadCSV<T>(bool headers)
        {
            if (ReaderInfo.IsCSV)
            {
                try
                {
                    throw new NotImplementedException();
                }
                catch (Exception)
                {
                    return null;
                }
                finally
                {
                    FinishRead();
                }
            }
            throw new NotSupportedException();
        }

        public List<T> ReadJSON<T>()
        {
            if (ReaderInfo.IsJSON)
            {
                try
                {
                    throw new NotImplementedException();
                }
                catch (Exception)
                {
                    return null;
                }
                finally
                {
                    FinishRead();
                }
            }
            throw new NotSupportedException();
        }

        public T ReadXML<T>()
        {
            if (ReaderInfo.IsXML)
            {
                try
                {
                    return XMLConverter.DeSerializeObjects<T>(ReaderInfo.FullPath);
                }
                catch (Exception)
                {
                    return default(T);
                }
                finally
                {
                    FinishRead();
                }
            }
            throw new NotSupportedException();
        }
    }
}
