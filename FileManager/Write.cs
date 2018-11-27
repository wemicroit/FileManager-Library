using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WeMicroIt.Utils.FileConverter.Interfaces;

namespace WeMicroIt.Utils.FileConverter
{
    public partial class FileManager : IFileManager
    {
        private bool StartWrite()
        {
            return StartWrite(false);
        }

        private bool StartWrite(bool append)
        {
            FinishRead();
            if (writer == null)
            {
                if (!WriterInfo.CheckDirectory())
                {
                    throw new DirectoryNotFoundException();
                }
                writer = WriterInfo.CheckFile(true) ? new StreamWriter(ReaderInfo.FullPath, append) : null;
            }
            return writer != null ? true : throw new FileNotFoundException();
        }

        private bool FinishWrite()
        {
            if (!MultiAction && writer != null)
            {
                writer.Close();
                writer.Dispose();
                writer = null;
            }
            return writer == null;
        }

        private bool Write(string contents, bool append)
        {
            try
            {
                StartWrite(append);
                writer.Write(contents);
                return true;
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                FinishWrite();
            }
        }

        public bool WriteBlock(string contents)
        {
            return Write(contents, false);
        }

        public bool WriteLine(string contents)
        {
            return Write(contents, true);
        }

        public bool WriteLine(string contents, bool append)
        {
            try
            {
                StartWrite(append);
                writer.WriteLine(contents);
                return true;
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                FinishWrite();
            }
        }

        public bool WriteLines(List<string> contents)
        {
            return WriteLines(contents, true);
        }

        public bool WriteLines(List<string> contents, bool append)
        {
            try
            {
                int written = 0;
                MultiAction = true;
                foreach (var item in contents)
                {
                    if (WriteLine(item, append))
                    {
                        written++;
                    }
                }
                return written == contents.Count? true : throw new DataMisalignedException();
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException();
            }
            catch (DataMisalignedException)
            {
                throw new DataMisalignedException();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                MultiAction = false;
                FinishWrite();
            }
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
                if (data == null)
                {
                    throw new ArgumentNullException();
                }
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
                return WriteBlock(JSONConverter.SerializeObjects(data));
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

        public bool WriteXML<T>(T data)
        {
            if (WriterInfo.IsXML)
            {
                return WriteBlock(XMLConverter.SerializeObjects(data));
            }
            throw new NotSupportedException();
        }
    }
}
