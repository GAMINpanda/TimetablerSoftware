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
            string activity = ActivityNameInput.Text;

            List<bool> Days = new List<bool> {
                Mon.IsChecked.Value, Tue.IsChecked.Value, Wed.IsChecked.Value,
                Thu.IsChecked.Value, Fri.IsChecked.Value, Sat.IsChecked.Value,
                Sun.IsChecked.Value
            }; //List of what days are selected

            NewTimeSlot NewActivity = new MainCode.NewTimeSlot(time1, time2, activity);

            NewActivity.SetDays(Days);

            MessageBox.Show($"{NewActivity.ActivityName}: {NewActivity.BeginningTime} - {NewActivity.EndingTime}");
        }
    }
}
