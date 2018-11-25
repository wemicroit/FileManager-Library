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

        public string SetWriter(string path)
        {
            try
            {
                writerPath = path;
                return writerPath;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private bool StartWrite(bool append)
        {
            try
            {
                FinishRead();
                if (Writer == null)
                {
                    Writer = new StreamWriter(writerPath, append);
                }
                return Writer != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool FinishWrite()
        {
            try
            {
                if (!MultiAction && Writer != null)
                {
                    Writer.Close();
                    Writer.Dispose();
                    Writer = null;
                }
                return Writer == null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool Write(string contents, bool append)
        {
            try
            {
                if (!StartWrite(append))
                {
                    throw new InvalidOperationException();
                }
                Writer.Write(contents);
                return true;
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
                if (!StartWrite(append))
                {
                    throw new InvalidOperationException();
                }
                Writer.WriteLine(contents);
                return true;

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
                return written == contents.Count;
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
            try
            {
                if (!writerPath.EndsWith(".csv"))
                {
                    throw new FileNotFoundException();
                }
                if (data == null)
                {
                    throw new ArgumentNullException();
                }
                throw new NotImplementedException();
                //return WriteLines(CSVConverter.SerializeBlock<T>(data), Append);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AppendJSON<T>(List<T> data)
        {
            try
            {
                var fullData = ReadJSON<T>();
                if (fullData == null)
                {
                    fullData = new List<T>();
                }
                fullData.AddRange(data);
                return WriteJSON<T>(fullData);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool WriteJSON<T>(List<T> data)
        {
            try
            {
                if (!writerPath.EndsWith(".json"))
                {
                    throw new FileNotFoundException();
                }
                return WriteBlock(JSONConverter.SerializeObjects(data));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AppendXML<T>(List<T> data)
        {
            try
            {
                var fullData = ReadXML<T>();
                if (fullData == null)
                {
                    fullData = new List<T>();
                }
                fullData.AddRange(data);
                return WriteXML<T>(fullData);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool WriteXML<T>(List<T> data)
        {
            try
            {
                if (!writerPath.EndsWith(".xml"))
                {
                    throw new FileNotFoundException();
                }
                return WriteBlock(XMLConverter.SerializeObjects(data));
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
