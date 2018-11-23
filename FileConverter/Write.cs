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
            try
            {
                FinishRead();
                if (Writer == null)
                {
                    Writer = new StreamWriter(GetFullPath(), append);
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
                StartWrite(append);
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
                StartWrite(append);
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

        public bool WriteCSV<T>(List<T> data)
        {
            try
            {
                if (!FilePath.EndsWith(".csv"))
                {
                    throw new FileNotFoundException();
                }
                if (data == null)
                {
                    throw new ArgumentNullException();
                }
                return WriteLines(CSVConverter.SerializeBlock<T>(data));
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
                throw new NotImplementedException();
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }

        public bool WriteXML<T>(List<T> data)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }
    }
}
