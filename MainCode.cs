using System;
using System.Collections;
using System.Collections.Generic;

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

        public int GetRowBegin()
        {
            //Row on WPF grid is different to actuality
            //==> Rows start at 2 inclusive

            int bg = Convert.ToInt32(BeginningTime);
            bg = bg - 5;
            bg = bg / 100;
            return bg;
        }

        public List<int> GetColumnBegin()
        {
            //columns start on 4 inclusive
            int count = 0;
            List<int> dayscol = new List<int>();
            foreach (bool value in Days)
            {
                if (value) {
                    dayscol.Add(count + 4);
                }
                count++;
            }
            return dayscol;
        }

        public string BeginningTime { get; }
        public string EndingTime { get; }
        public string ActivityName { get; }
        public List<bool> Days { get; set; }
    }
}