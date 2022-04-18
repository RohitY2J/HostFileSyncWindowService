using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReadToHost
{
    public class FileHandler
    {

        private string readPath;
        private string writePath;
        private string splitText;
        private string scheduledReadPath;
        public FileHandler(IOptions<FileConfiguration> fs)
        {
            readPath = fs.Value.FileToRead;
            writePath = fs.Value.FileToWrite;
            splitText = fs.Value.TextToSplit;
            scheduledReadPath = fs.Value.SheduledFileToRead;
        }
        public void syncFiles()
        {
            try
            {
                string readText = File.ReadAllText(readPath);
                DateTime currentTime = new DateTime();
                TimeSpan start = new TimeSpan(20, 0, 0);
                TimeSpan end = new TimeSpan(22, 0, 0);
                if(start <= currentTime.TimeOfDay && end >= currentTime.TimeOfDay)
                {
                    EventLog.WriteEntry("FileReadToHost", "It is break time. 8pm-10pm", EventLogEntryType.Information);
                }
                else
                {
                    EventLog.WriteEntry("FileReadToHost", "It is not break time. Not(8pm-10pm)", EventLogEntryType.Information);
                    readText = readText + File.ReadAllText(scheduledReadPath);
                }

                string[] writeArr = File.ReadAllText(writePath).Split(splitText);
                string writeText = writeArr[0];
                string compareText = writeArr[1];
                if (readText == compareText)
                {
                    EventLog.WriteEntry("FileReadToHost", "Nothing to sync", EventLogEntryType.Information);
                }
                else
                {
                    writeText = writeText + splitText + readText;
                    File.WriteAllText(writePath, writeText);
                    EventLog.WriteEntry("FileReadToHost", "Files synced", EventLogEntryType.Information);
                }
            }
            catch(Exception e)
            {
                EventLog.WriteEntry("FileReadToHost", e.Message , EventLogEntryType.Error);
            }
        }
    }
}
