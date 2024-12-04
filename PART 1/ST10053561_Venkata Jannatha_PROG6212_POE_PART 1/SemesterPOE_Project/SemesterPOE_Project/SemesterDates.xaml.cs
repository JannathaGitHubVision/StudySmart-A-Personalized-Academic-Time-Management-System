using PoeDLL;
using System;
using System.Collections.Generic;
using System.Windows;

namespace SemesterPOE_Project
{
    /// <summary>
    /// Interaction logic for SemesterDates.xaml
    /// </summary>
    public partial class SemesterDates : Window
    {
        private List<ModulesInfo> modulesList = new List<ModulesInfo>();
        private List<SemesterDatesWeeks> semesterList = new List<SemesterDatesWeeks>();

        public SemesterDates(List<ModulesInfo> modulesL)
        {
            InitializeComponent();
            modulesList = modulesL;
        }


        /// <summary>
        /// Here Iam storing the values of Semester Dates and Number of Weeks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SemesterSubmitButton(object sender, RoutedEventArgs e)
        {

            //modulesList = modules;
            ExceptionHandlingPOE exception = new ExceptionHandlingPOE();
            int SemesterWeeks = 0;
            string SemesterDate = "";
            bool validationPasssed = true;

            //Semester Weeks
            try
            {
                SemesterWeeks = exception.NumValid(WeeksNumText.Text.Trim(), "Semester Weeks");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                WeeksNumText.Text = "";
                validationPasssed = false;
            }

            //Semester Dates
            try
            {
                SemesterDate = exception.DateHandling(DateText.Text.Trim());
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                DateText.Text = "";
                validationPasssed = false;
            }

            ///If all validationPassed is true it activates this line
            if (validationPasssed)
            {
                MessageBox.Show("Successfully captured the values", "Sucess Message", MessageBoxButton.OK, MessageBoxImage.Information);

                SemesterDatesWeeks semesterDatesWeeks = new SemesterDatesWeeks(SemesterWeeks, SemesterDate);
                semesterList.Add(semesterDatesWeeks);

                MessageBox.Show($"You Can Click {"Capture More"} , To add more Modules, or Click {"Display Values"} to Display values","Information", MessageBoxButton.OK, MessageBoxImage.Information);
                DateText.Visibility = Visibility.Collapsed;
                WeeksNumText.Visibility = Visibility.Collapsed;
                DateText.Text = "";
                WeeksNumText.Text = "";

            }
        }


        /// <summary>
        /// It naviagtes to the Module class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackToModule(object sender, RoutedEventArgs e)
        {
            // Navigate back to SemesterModules
            SemesterModulesCapture semesterModules = new SemesterModulesCapture(modulesList);
            semesterModules.Show();
            Close();
        }

        /// <summary>
        /// It navigates to the Display Academics class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigateToDisplay_Click(object sender, RoutedEventArgs e)
        {
            DisplayAcademics displayAcademics = new DisplayAcademics(modulesList, semesterList);
            displayAcademics.Show();

            Close();
        }
    }
}
