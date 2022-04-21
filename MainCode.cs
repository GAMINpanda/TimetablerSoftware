using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

//File with callable functions relating to the timetabling software

namespace MainCode
{
    public class NewTimeSlot
    {
        public NewTimeSlot()
        {
            BeginningTime = "0000";
            EndingTime = "0000";
            ActivityName = "void";

            Days = new List<bool> { false, false, false, false, false, false, false };
        }

        public NewTimeSlot(string time1, string time2, string activity)
        {
            BeginningTime = time1;
            EndingTime = time2;
            ActivityName = activity;

            Days = new List<bool> { false, false, false, false, false, false, false };
        }

        public void SetDays(List<bool> InputDays)
        {
            Days = InputDays;
        }

        public (int, int) GetRowBegin()
        {
            //Row on WPF grid is different to actuality
            //==> Rows start at 2 inclusive

            int beginningtime = Convert.ToInt32(BeginningTime);
            int beginningtimehr = Convert.ToInt32(beginningtime / 100); //time is 0000 digits so divide by 100 and round for integer
            int beginningtimemin = (beginningtime - (beginningtimehr * 100)) / 60; //finds difference as a fraction of an hour
            int rowbegin = beginningtimehr - 5;
            return (rowbegin, beginningtimemin);
        }

        public List<int> GetColumnBegin()
        {
            //Can be on multiple days so need to iterate through days
            int count = 0;
            List<int> dayscol = new List<int>();
            foreach (bool value in Days)
            {
                if (value) {
                    dayscol.Add(count + 4); //columns start on 4 inclusive
                }
                count++; //successive day +1
            }
            return dayscol;
        }

        public (int, int) GetRowEnd()
        {
            //Row on WPF grid is different to actuality
            //==> Rows start at 2 inclusive

            int endingtime = Convert.ToInt32(EndingTime);
            int endingtimehr = Convert.ToInt32(endingtime / 100); //time is 0000 digits so divide by 100 and round for integer
            int endingtimemin = (endingtime - (endingtimehr * 100)) / 60; //finds difference as a fraction of an hour
            int rowend = endingtimehr - 5;
            return (rowend, endingtimemin);
        }

        public string BeginningTime { get; }
        public string EndingTime { get; }
        public string ActivityName { get; }
        public List<bool> Days { get; set; }

        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = true) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
    }
}