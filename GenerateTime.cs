//A script that generates the rest of the timetable based off of the current and desired inputs

using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using MainCode;

namespace GenSlots
{
    public class NewTimeGen : NewTimeSlot //':' inherits
    {
        static List<string> GetCSVData(string filename)
        {
            string FullPath = Path.GetFullPath(filename);

            FullPath = FullPath.Replace(@"bin\Debug\netcoreapp3.1\", "");

            StreamReader file = new StreamReader(FullPath, true);

            string[] CSVitems = (file.ReadToEnd()).Split('\n'); //seperate activities

            file.Close();

            List<string> CorrectCSVitems = new List<string>(CSVitems);

            return CorrectCSVitems;
        }

        static void GetActvitiesToGenerate()
        {

        }

        public static void FreeTime() //Reads save file to decide what times are free in the week - Creates new file with details (call once on save)
        {
            string[] Day;

            List<string> CorrectCSVitems = GetCSVData("Save.csv");

            CorrectCSVitems.RemoveAt(CorrectCSVitems.Count - 1); //cast to list then remove final (empty) space

            /*
            foreach(string val in CSVitems)
            {
                Debug.WriteLine(val);
            }
            */

            //Now adding values To SaveBoundaries for easy retrieval

            List<string> DaysOfTheWeek = new List<string>() { "", "", "", "", "", "", ""};

            foreach (string activity in CorrectCSVitems)
            {
                Day = activity.Split(',');
                for(int i = 3; i < 10; i++)
                {
                    //Debug.WriteLine(i);
                    if (Convert.ToBoolean(Day[i])) //if activity on day
                    {
                        DaysOfTheWeek[i-3] = DaysOfTheWeek[i-3] + (Day[1] + "," + Day[2] + ","); //add times
                    }
                }
            }


            string filename = "SaveBoundaries.csv";
            string FullPath = Path.GetFullPath(filename);

            FullPath = FullPath.Replace(@"bin\Debug\netcoreapp3.1\", "");

            StreamWriter filenew = new StreamWriter(FullPath, false); //not appending

            foreach (string value in DaysOfTheWeek)
            {
                filenew.WriteLine(value);
            }

            filenew.Close();
        }

        public static List<List<string>> ExtractFromSaveBoundaries() //converts into list with all free times
        {
            List<string> defaultvar = new List<string>() { "0700", "0800", "0900", "1000", "1100", "1200", "1300", "1400", "1500", "1600", "1700", "1800", "1900", "2000", "2100", "2200"};
            List<List<string>> allvalues = new List<List<string>>() { defaultvar, defaultvar, defaultvar, defaultvar, defaultvar, defaultvar, defaultvar };
            List<string> CSVitems = GetCSVData("SaveBoundaries.csv");
            int len;
            List<List<string>> Timing = new List<List<string>>();

            foreach(string value in CSVitems)
            {
                Timing.Add( new List<string> (value.Split(','))); //list of times taken
            }

            for(int i = 0; i < 7; i++)
            {
                len = Timing[i].Count;

                for (int j = 0; j < len; j = j + 2)
                {
                    allvalues[i] = EachDay(Timing[i][j], Timing[i][j+1], allvalues[i]);
                }
            }

            return allvalues;
        }

        public static List<string> EachDay(string time1, string time2, List<string> Day) //removes times from day list in range of time1 --> time2
        {
            int curtime;
            string curtimestring;
            int firsttime = Convert.ToInt32(time1);
            int secondtime = Convert.ToInt32(time2);

            int count = firsttime - secondtime;
            count = count / 100;

            for (int i = 0; i < count; i++)
            {
                curtime = firsttime + (i*100); //gaps of 100

                if (curtime < 1000)
                {
                    curtimestring = "0" + Convert.ToString(curtime);
                }
                else
                {
                    curtimestring = Convert.ToString(curtime);
                }

                Day.Remove(curtimestring);
            }

            return Day;
        }

        static void DecideActivityDistribution()
        {

        }

        private List<List<string>> freetimevals = NewTimeGen.ExtractFromSaveBoundaries();
    }

}