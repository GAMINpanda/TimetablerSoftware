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

        public string BeginningTime { get; }
        public string EndingTime { get; }
        public string ActivityName { get; }
        public List<bool> Days { get; set; }
    }
}