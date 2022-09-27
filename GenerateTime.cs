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
        static void GetActvitiesToGenerate()
        {

        }

        public static void FreeTime() //Reads save file to decide what times are free in the week - Creates new file with details (call once on save)
        {
            string[] Day;
            string filename = "Save.csv";
            string FullPath = Path.GetFullPath(filename);

            FullPath = FullPath.Replace(@"bin\Debug\netcoreapp3.1\", "");

            StreamReader file = new StreamReader(FullPath, true);

            string[] CSVitems = (file.ReadToEnd()).Split('\n'); //seperate activities

            List<string> CorrectCSVitems = new List<string>(CSVitems);

            CorrectCSVitems.RemoveAt(CSVitems.Length - 1); //cast to list then remove final (empty) space

            /*
            foreach(string val in CSVitems)
            {
                Debug.WriteLine(val);
            }
            */

            file.Close();

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


            filename = "SaveBoundaries.csv";
            FullPath = Path.GetFullPath(filename);

            FullPath = FullPath.Replace(@"bin\Debug\netcoreapp3.1\", "");

            StreamWriter filenew = new StreamWriter(FullPath, false); //not appending

            foreach (string value in DaysOfTheWeek)
            {
                filenew.WriteLine(value);
            }

            filenew.Close();
        }

        static void DecideActivityDistribution()
        {

        }
    }

}