using System;
using System.IO;

namespace PlateMightsight
{
    public class LogData
    {
        public void WriteLog(string logMessage)
        {
            try
            {
                string stringLogPath = @"Log/" + System.DateTime.Today.ToString("ddMMyy") + "." + "log";
                FileInfo log_FileInfo = new FileInfo(stringLogPath);
                DirectoryInfo log_DirInfo = new DirectoryInfo(log_FileInfo.DirectoryName);
                if (!log_DirInfo.Exists) log_DirInfo.Create();
                using (FileStream file_Stream = new FileStream(stringLogPath, FileMode.Append))
                {
                    using (StreamWriter log = new StreamWriter(file_Stream))
                    {
                        log.WriteLine(logMessage + " " + DateTime.Now.ToString());
                        log.Close();
                    }
                }
            }
            catch (Exception)
            {

            }
           
        }
    }
}
