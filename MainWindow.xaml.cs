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

            MessageBox.Show($"{NewActivity.ActivityName}: {NewActivity.BeginningTime} - {NewActivity.EndingTime}");
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
