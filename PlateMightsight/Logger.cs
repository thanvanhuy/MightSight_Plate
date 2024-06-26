using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PlateMightsight
{
    public class Logger
    {
        public enum LogType
        {
            ERROR,
            MESSAGE
        }

        private string logFile;

        private string logContent;

        public string LogFolder { get; set; }

        public Logger()
        {
            LogFolder = AppDomain.CurrentDomain.BaseDirectory + "\\LogFolder\\";
            if (!Directory.Exists(LogFolder))
            {
                Directory.CreateDirectory(LogFolder);
            }

            logFile = string.Empty;
            logContent = string.Empty;
        }

        public Logger(string logFolder)
        {
            LogFolder = logFolder;
            if (!Directory.Exists(LogFolder))
            {
                Directory.CreateDirectory(LogFolder);
            }

            logFile = string.Empty;
            logContent = string.Empty;
        }

        public Logger(string logFolder, string logFile)
        {
            LogFolder = logFolder;
            if (!Directory.Exists(LogFolder))
            {
                Directory.CreateDirectory(LogFolder);
            }

            this.logFile = logFile;
            logContent = string.Empty;
        }

        public void Write(string functionName, string content, LogType type)
        {
            try
            {
                logFile = DateTime.Now.ToString("yyyyMMdd") + ".log";
                string path = LogFolder + "\\" + logFile;
                using (FileStream fileStream = new FileStream(path, FileMode.Append, FileAccess.Write))
                {
                    logContent = DateTime.Now.ToString("dd-MM-yyyy") + "  " + DateTime.Now.ToString("HH:mm:ss.fff");
                    logContent += (functionName.Equals(string.Empty) ? "" : (" " + functionName + "() - "));
                    logContent += ((type == LogType.ERROR) ? "Error: " : "Message: ");
                    logContent = logContent + content + Environment.NewLine;
                    byte[] bytes = Encoding.ASCII.GetBytes(logContent);
                    fileStream.Write(bytes, 0, bytes.Length);
                    fileStream.Close();
                }

                logContent = string.Empty;
            }
            catch
            {
            }
        }

        public void Add(string functionName, string content, LogType type)
        {
            try
            {
                logContent = DateTime.Now.ToString("dd-MM-yyyy") + "  " + DateTime.Now.ToString("HH:mm:ss.fff");
                logContent += (functionName.Equals(string.Empty) ? "" : (" " + functionName + "() - "));
                logContent += ((type == LogType.ERROR) ? "Error: " : "Message: ");
                logContent = logContent + content + Environment.NewLine;
            }
            catch
            {
            }
        }
        public void WriteXmlLog(Dictionary<string, string> parameters)
        {
            try
            {
                logFile = DateTime.Now.ToString("yyyyMMdd") + ".log";
                string text = LogFolder + "\\" + logFile;
                if (!File.Exists(text))
                {
                    FileStream fileStream = new FileStream(text, FileMode.Append, FileAccess.Write);
                    string text2 = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine;
                    text2 = text2 + "<Application>" + Environment.NewLine;
                    text2 = text2 + "</Application>" + Environment.NewLine;
                    byte[] bytes = Encoding.ASCII.GetBytes(text2);
                    fileStream.Write(bytes, 0, bytes.Length);
                    fileStream.Close();
                }

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(text);
                XmlNode xmlNode = xmlDocument.ChildNodes[1];
                XmlNode xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Element, "session", null);
                XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("time");
                xmlAttribute.Value = DateTime.Now.ToString("dd-MM-yyyy") + "  " + DateTime.Now.ToString("HH:mm:ss.fff");
                xmlNode2.Attributes.Append(xmlAttribute);
                foreach (KeyValuePair<string, string> parameter in parameters)
                {
                    XmlAttribute xmlAttribute2 = xmlDocument.CreateAttribute(parameter.Key.Replace(" ", "_"));
                    xmlAttribute2.Value = parameter.Value;
                    xmlNode2.Attributes.Append(xmlAttribute2);
                }

                xmlNode.AppendChild(xmlNode2);
                xmlDocument.Save(text);
            }
            catch
            {
            }
        }
    }
}
