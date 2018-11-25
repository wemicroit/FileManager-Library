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
        public string SetFilePath(string path)
        {
            try
            {
                readerPath = path;
                return path;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<string> GetFiles()
        {
            return GetFiles(null);
        }

        public List<string> GetFiles(string filter)
        {
            if (true)
            {

            }
            /*var options = new EnumerationOptions()
            {
                //MatchCasing = MatchCasing.CaseInsensitive,
                RecurseSubdirectories = true,
                IgnoreInaccessible = true,
            };*/
            return GetFiles(filter, null);
        }

        public List<string> GetFiles(string filter, string options)
        {
            try
            {
                if (!CheckDirectory())
                {
                    throw new DirectoryNotFoundException();
                }
                return Directory.GetFiles(readerPath, "").Select(x => x.Replace(readerPath, "")).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private bool StartRead()
        {
            try
            {
                FinishWrite();
                if (Reader == null)
                {
                    Reader = new StreamReader(readerPath);
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
                if (!CheckFile(readerPath))
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

        public Stream ReadStream()
        {
            try
            {
                if (!CheckFile(readerPath))
                {
                    throw new FileNotFoundException();
                }
                StartRead();
                Reader.ReadToEnd();
                return Reader.BaseStream;
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
                if (!CheckFile(readerPath))
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
                if (!CheckFile(readerPath))
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
                if (!readerPath.EndsWith(".csv"))
                {
                    throw new FileNotFoundException();
                }
                throw new NotImplementedException();
                //return CSVConverter.DeSerializeBlock<T>(ReadFile(), headers);
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
                if (!readerPath.EndsWith(".json"))
                {
                    throw new FileNotFoundException();
                };
                return JSONConverter.DeSerializeObjects<T>(ReadFile());
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
                if (!readerPath.EndsWith(".xml"))
                {
                    throw new FileNotFoundException();
                }
                //return XMLConverter.DeSerializeObjects<T>(ReadFile());
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(ReadFile());

                string jsonText = JsonConvert.SerializeXmlNode(doc).Replace("\"?xml\":{\"@version\":\"1.0\"},", "");
                List<T> list2 = JSONConverter.DeSerializeObjects<T>(jsonText);

                List<T> list = null;
                foreach (var item in list)
                {
                    Console.WriteLine(item);
                }
                FinishRead();
                return list;

            }
            catch (Exception exc)
            {

                throw new NotImplementedException();
            }
            finally
            {
                FinishRead();
            }
        }

    }
}
