using System;
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
using GenSlots;

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

            NewTimeGen Generator = new NewTimeGen();

            Generator.FreeTime();
            List<NewTimeSlot> RandomActivities = Generator.ActvitiesToGenerate();

            List<NewTimeSlot> Activities = LoadFromCsv();

            Activities.AddRange(RandomActivities);

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

                if (tempActivity.Length == 10) //might be an empty newline
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

                    ActivityTemp = new NewTimeSlot(time1, time2, Name); //creates new object and assigns accordingly
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
        private void RemoveButton_Click(object sender, RoutedEventArgs e) //When remove button clicked
        {
            string itemRemove = ActivityRemoveInput.Text;
            List<bool> DaysRemove = new List<bool>(){ MonR.IsChecked.Value, TueR.IsChecked.Value, WedR.IsChecked.Value,
                ThuR.IsChecked.Value, FriR.IsChecked.Value, SatR.IsChecked.Value,
                SunR.IsChecked.Value
            };//List of the days to be removed named itemRemove

            string[] temp;
            string tempToWrite;
            bool tempcheck = false;

            List<bool> NewDays = new List<bool>() { false, false, false, false, false, false, false};

            string filename1 = "Save.csv"; //uses a temp file to temporarily store the objects
            string filename2 = "Save_Temp.csv";
            string FullPath1 = Path.GetFullPath(filename1);
            string FullPath2 = Path.GetFullPath(filename2);

            FullPath1 = FullPath1.Replace(@"bin\Debug\netcoreapp3.1\", "");
            FullPath2 = FullPath2.Replace(@"bin\Debug\netcoreapp3.1\", "");

            StreamReader file = new StreamReader(FullPath1, true);

            using (StreamWriter filewrite = new StreamWriter(FullPath2))
            {
                for (string line; (line = file.ReadLine()) != null;) //checks until the end of the file
                {
                    string line1 = line.Replace("\n", string.Empty); //removes \n for later
                    temp = line1.Split(',');

                    if (temp.Length == 10) //eliminate whitespace
                    {
                        if (!temp.Contains(itemRemove))
                        {
                            filewrite.WriteLine(line); //just write original line
                        }

                        else
                        {
                            tempcheck = false;

                            for (int i = 3; i < 10; i++)
                            {
                                //MessageBox.Show($"temp[{i}]: {temp[i]}");
                                //MessageBox.Show($"DaysRemove[{i - 3}]: {DaysRemove[i - 3]}");

                                if (Convert.ToBoolean(temp[i]) == DaysRemove[i - 3] || !Convert.ToBoolean(temp[i])) //if days needs removing and day exists or
                                                                                                                    //day doesn't exist
                                {
                                    NewDays[i - 3] = false;
                                }
                                else
                                {
                                    NewDays[i - 3] = true;
                                }
                            }

                            foreach (bool var in NewDays) //so object isn't removed as long as one day still exists
                            {
                                if (var)
                                {
                                    tempcheck = true;
                                }
                            }

                            //MessageBox.Show($"{tempcheck}");

                            if (tempcheck) //constructs new line to add with changes
                            {
                                tempToWrite = itemRemove + ',' + temp[1] + ',' + temp[2];

                                foreach (bool var in NewDays)
                                {
                                    tempToWrite = tempToWrite + ',' + Convert.ToString(var);
                                }

                                filewrite.WriteLine(tempToWrite);
                            }
                        }
                    }
                }
            }
            //read line by line
            file.Close();

            using (StreamReader fileread = new StreamReader(FullPath2, true)) //just transfers the temp data to Save.csv
            {
                string NewText = fileread.ReadToEnd();

                using (StreamWriter filewrite = new StreamWriter(FullPath1))
                {
                    filewrite.Write(NewText);
                }
            }
        }

        public void AddActivity(NewTimeSlot Activity) //Adds an activity to MainWindow.xaml
        {
            (int Rowbegin, int rowbdecimal) = Activity.GetRowBegin();
            (int Rowend, int rowedecimal) = Activity.GetRowEnd(); //see MainCode.cs for function details

            int rowdif = Rowend - Rowbegin;

            List<int> Columns = Activity.GetColumnBegin();

            foreach (int ColumnNum in Columns) //Adds activities to canvas
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
