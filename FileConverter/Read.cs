using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WeMicroIt.Utils.FileConverter.Interfaces;

namespace WeMicroIt.Utils.FileConverter
{
    public partial class FileManager : IFileManager
    {
        private bool StartRead()
        {
            try
            {
                FinishWrite();
                if (Reader == null)
                {
                    Reader = new StreamReader(GetFullPath());
                }
                return Reader != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool FinishRead()
        {
            try
            {
                if (!MultiAction && Reader != null)
                {
                    Reader.Close();
                    Reader.Dispose();
                    Reader = null;
                }
                return Reader == null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string ReadFile()
        {
            try
            {
                if (!CheckFile())
                {
                    throw new FileNotFoundException();
                }
                StartRead();
                return Reader.ReadToEnd();
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
                if (!CheckFile())
                {
                    throw new FileNotFoundException();
                }
                StartRead();
                return Reader.ReadLine();
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
                if (!CheckFile())
                {
                    throw new FileNotFoundException();
                }
                List<string> contents = new List<string>();
                while (!Reader.EndOfStream)
                {
                    contents.Add(ReadLine());
                }
                return contents;
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
            try
            {
                if (!FilePath.EndsWith(".csv"))
                {
                    throw new FileNotFoundException();
                }
                return CSVConverter.DeserialiseBlock<T>(ReadFile(), headers);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<T> ReadJSON<T>()
        {
            try
            {
                if (!FilePath.EndsWith(".json"))
                {
                    throw new FileNotFoundException();
                };
                throw new NotImplementedException();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<T> ReadXML<T>()
        {
            try
            {
                if (!FilePath.EndsWith(".xml"))
                {
                    throw new FileNotFoundException();
                }
                throw new NotImplementedException();
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }

    }
}
