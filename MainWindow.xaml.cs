﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.IO;
using Path = System.IO.Path;
using MainCode;

namespace TimetablerSoftware
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<NewTimeSlot> Activities = LoadFromCsv();
            foreach (NewTimeSlot Activity in Activities)
            {
                AddActivity(Activity);
            }
        }

        public List<NewTimeSlot> LoadFromCsv() //Method to get items to create an activity off the csv which can then be loaded
        {
            string filename1 = "Save.csv";
            string FullPath1 = Path.GetFullPath(filename1);

            string Name;
            string time1;
            string time2;
            List<bool> Days;

            string[] tempActivity;
            NewTimeSlot ActivityTemp;

            List<NewTimeSlot> Activities = new List<NewTimeSlot>() { };
            FullPath1 = FullPath1.Replace(@"bin\Debug\netcoreapp3.1\", "");

            StreamReader file = new StreamReader(FullPath1);

            for (string line; (line = file.ReadLine()) != null;)
            {
                tempActivity = line.Split(',');

                if (tempActivity.Length == 10)
                {
                    /*
                    foreach(string var in tempActivity)
                    {
                        MessageBox.Show($"{var}");
                    }
                    */

                    Name = tempActivity[0];
                    time1 = tempActivity[1];
                    time2 = tempActivity[2];

                    ActivityTemp = new NewTimeSlot(time1, time2, Name);
                    Days = new List<bool>() { };

                    for (int i = 3; i < 10; i++)
                    {
                        Days.Add(Convert.ToBoolean(tempActivity[i]));
                    }

                    ActivityTemp.SetDays(Days);
                    Activities.Add(ActivityTemp);
                }
            }

            file.Close();

            return Activities;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string time1 = ActivityStartInput.Text;
            string time2 = ActivityEndInput.Text;
            string activity = ActivityNameInput.Text; //needed to form activity

            List<bool> Days = new List<bool> {
                Mon.IsChecked.Value, Tue.IsChecked.Value, Wed.IsChecked.Value,
                Thu.IsChecked.Value, Fri.IsChecked.Value, Sat.IsChecked.Value,
                Sun.IsChecked.Value
            }; //List of what days are selected

            NewTimeSlot NewActivity = new MainCode.NewTimeSlot(time1, time2, activity); //uses NewTimeSlot to create NewActivity object

            NewActivity.SetDays(Days);

            AddActivity(NewActivity);

            NewActivity.WriteToCsv(); //godisfing

            //MessageBox.Show($"{NewActivity.ActivityName}: {NewActivity.BeginningTime} - {NewActivity.EndingTime}");
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e) //not working rn
        {
            string itemRemove = ActivityRemoveInput.Text;
            string filename1 = "Save.csv";
            string filename2 = "Save_Temp.csv";
            string FullPath1 = Path.GetFullPath(filename1);
            string FullPath2 = Path.GetFullPath(filename2);

            FullPath1 = FullPath1.Replace(@"bin\Debug\netcoreapp3.1\", "");
            FullPath2 = FullPath2.Replace(@"bin\Debug\netcoreapp3.1\", "");

            StreamReader file = new StreamReader(FullPath1, true);

            using (StreamWriter filewrite = new StreamWriter(FullPath2))
            {
                for (string line; (line = file.ReadLine()) != null;)
                {
                    if (!(line.Split(',')).Contains(itemRemove))
                    {
                        filewrite.WriteLine(line);
                    }
                }
            }
            //read line by line
            file.Close();

            using (StreamReader fileread = new StreamReader(FullPath2, true))
            {
                string NewText = fileread.ReadToEnd();

                using (StreamWriter filewrite = new StreamWriter(FullPath1))
                {
                    filewrite.Write(NewText);
                }
            }
        }

            public void AddActivity(NewTimeSlot Activity)
        {
            (int Rowbegin, int rowbdecimal) = Activity.GetRowBegin();
            (int Rowend, int rowedecimal) = Activity.GetRowEnd(); //see MainCode.cs for function details

            int rowdif = Rowend - Rowbegin;

            List<int> Columns = Activity.GetColumnBegin();

            foreach (int ColumnNum in Columns) //Puts 'pen to paper' in a sense
            {
                TextBlock Temp = new TextBlock()
                {
                    Text = Activity.ActivityName
                };

                Border NewBorder = new Border()
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brushes.Black,
                    Child = Temp
                };

                Grid.SetColumn(NewBorder, ColumnNum);
                Grid.SetRow(NewBorder, Rowbegin);
                Grid.SetRowSpan(NewBorder, rowdif);

                MainGrid.Children.Add(NewBorder);
            }
        }
    }


}
