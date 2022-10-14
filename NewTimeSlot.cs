using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using GenSlots;

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

        public NewTimeSlot(string time1, string time2, string activity, List<bool> Daysinp)
        {
            BeginningTime = time1;
            EndingTime = time2;
            ActivityName = activity;

            Days = Daysinp;
        }

        public void SetDays(List<bool> InputDays)
        {
            Days = InputDays;
        }

        public (int, int) GetRowBegin()
        {
            //Row on WPF grid is different to actuality
            //==> Rows start at 2 inclusive

            try
            {
                int beginningtime = Convert.ToInt32(BeginningTime);
                int beginningtimehr = Convert.ToInt32(beginningtime / 100); //time is 0000 digits so divide by 100 and round for integer
                int beginningtimemin = (beginningtime - (beginningtimehr * 100)) / 60; //finds difference as a fraction of an hour
                int rowbegin = beginningtimehr - 5;
                return (rowbegin, beginningtimemin);
            }
            catch
            {
                return (0, 0);
            }
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
            try
            {
                int endingtime = Convert.ToInt32(EndingTime);
                int endingtimehr = Convert.ToInt32(endingtime / 100); //time is 0000 digits so divide by 100 and round for integer
                int endingtimemin = (endingtime - (endingtimehr * 100)) / 60; //finds difference as a fraction of an hour
                int rowend = endingtimehr - 5;
                return (rowend, endingtimemin);
            }

            catch
            {
                return (0, 0);
            }
        }

        public void WriteToCsv()
        { //just stores each new timeslot on a new line (easy as that)

            string filename = "Save.csv";
            string FullPath = Path.GetFullPath(filename);

            FullPath = FullPath.Replace(@"bin\Debug\netcoreapp3.1\", "");

            //Debug.Write(FullPath);

            StreamWriter file = new StreamWriter(FullPath, true);
            string[] ActivityLines = new string[10];
            ActivityLines[0] = ActivityName + ','; //adds object info to string[] starting with activity name
            ActivityLines[1] = BeginningTime + ',';
            ActivityLines[2] = EndingTime + ',';

            int count = 3;
            foreach (bool val in Days)
            {
                if (count == 9)
                {
                    ActivityLines[count] = Convert.ToString(val) + '\n';
                }
                else
                {
                    ActivityLines[count] = Convert.ToString(val) + ',';
                }
                count++;
            }

            foreach (string val in ActivityLines) //applies list in a way where it becomes a single line in the csv
            {
                file.Write(val);
            }
            file.Close();
        }

        public string BeginningTime { get; }
        public string EndingTime { get; }
        public string ActivityName { get; }
        public List<bool> Days { get; set; }

    }
}