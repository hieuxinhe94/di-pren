using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;
using DIClassLib.DbHelpers;
using System.Configuration;


namespace DIClassLib.Misc
{
    /// <summary>
    /// serializes objects to txt files
    /// files are stored in 'PersistedObjects' dir
    /// objects are deserialized from txt files if session objects has died after using the extern pay page
    /// files in dir are protected by authorization settings in web.config
    /// </summary>
    public class Serializer
    {
        int _numDaysKeepFiles = 20;  //replaced by value from appsettings in constructor
        string _pathToFiles = ConfigurationManager.AppSettings["pathSerializedSubs"];
        string _fileEnding = ".txt";


        public Serializer()
        {
            int.TryParse(ConfigurationManager.AppSettings["daysKeepSerializedSubs"], out _numDaysKeepFiles);
            DeleteOldFiles();
        }

        public void SaveObjectToFile(string fileId, object obj)
        {
            string fileName = _pathToFiles + fileId + _fileEnding;
            FileStream strm = null;

            try
            {
                strm = new FileStream(fileName, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(strm, obj);
            }
            catch (Exception ex)
            {
                new Logger("SaveObjectToFile() failed", ex.ToString());
            }
            finally
            {
                if (strm != null)
                    strm.Close();
            }
        }

        public object GetObjectFromFile(string fileId)
        {
            string fileName = _pathToFiles + fileId + _fileEnding;

            if (!File.Exists(fileName))
            {
                new Logger("GetObjectFromFile() could not find file: " + fileName, "");
                return null;
            }

            FileStream strm = null;
            try
            {
                strm = new FileStream(fileName, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                return (bf.Deserialize(strm));
            }
            catch (Exception ex)
            {
                new Logger("GetObjectFromFile() failed", ex.ToString());
                return null;
            }
            finally
            {
                if (strm != null)
                    strm.Close();
            }
        }


        //just to avoid lots of files that are not needed
        private void DeleteOldFiles()
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(_pathToFiles);

                foreach (FileInfo f in di.GetFiles())
                {
                    if (f.CreationTime.AddDays(_numDaysKeepFiles) < DateTime.Now)
                        f.Delete();
                }
                //foreach (DirectoryInfo d in di.GetDirectories()) { }
            }
            catch (Exception ex)
            {
                new Logger("DeleteOldFiles() failed", ex.ToString());
            }
        }
    }
}
