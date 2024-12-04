using PoeDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SemesterPOE_Project
{
    /// <summary>
    /// Interaction logic for DisplayAcademics.xaml
    /// </summary>
    public partial class DisplayAcademics : Window
    {
        private List<SemesterDatesWeeks> semList;
        Dictionary<string, int> moduleSelfStudyDict = new Dictionary<string, int>();
        private string date;
        private int totalWeeks;
        public DisplayAcademics()
        {
            InitializeComponent();
        }

        public DisplayAcademics(List<ModulesInfo> modulesList, List<SemesterDatesWeeks> semesterList)
        {
            InitializeComponent();
            semList = semesterList;
            RefactorValuesofListView(modulesList, semesterList);
        }

        private void RefactorValuesofListView(List<ModulesInfo> modulesList, List<SemesterDatesWeeks> semesterList)
        {

            foreach (SemesterDatesWeeks semester in semesterList)
            {
                totalWeeks += semester.Weeks1; // Accumulate the total weeks

            }

            //the starting of a semester date
            foreach (SemesterDatesWeeks semester in semesterList)
            {
                date = semester.Date1;
            }
            //Getting the values for a semester
            DateTime startDate = DateTime.Parse(date);
            SelectedDate(startDate);


            ///calculation self study hours of each module
            foreach (ModulesInfo module in modulesList)
            {
                int moduleSelfStudy = (module.ModuleCredits1 * 10 / totalWeeks) - module.ModuleClassHrs1;
                moduleSelfStudyDict.Add(module.ModuleName1, moduleSelfStudy);
            }

            // Prevent the DataGrid from automatically generating columns
            GridViewData.AutoGenerateColumns = false;

            // Create a new column for module names and add it to the DataGrid
            DataGridTextColumn nameColumn = new DataGridTextColumn();
            nameColumn.Header = "Module Name";
            nameColumn.Binding = new Binding("Name");
            GridViewData.Columns.Add(nameColumn);

            for (int i = 1; i <= totalWeeks; i++)
            {

                //Creating a weeks columns
                DataGridTextColumn AddColumn = new DataGridTextColumn();
                string header = $"Week {i}";
                AddColumn.Header = header;
                AddColumn.Binding = new Binding($"Weeks[Week{i}]");
                GridViewData.Columns.Add(AddColumn);


            }

            // Create a new column for Total Hours and add it to the DataGrid
            DataGridTextColumn totalHoursColumn = new DataGridTextColumn();
            totalHoursColumn.Header = "Total Hours";
            totalHoursColumn.Binding = new Binding("TotalHours");
            GridViewData.Columns.Add(totalHoursColumn);

            List<displayData> storeData = new List<displayData>();

            foreach (ModulesInfo module in modulesList)
            {
                // Get the moduleSelfStudy value from the dictionary
                int selfstudy = moduleSelfStudyDict[module.ModuleName1];


                // Use a for loop to add self-study hours for each week to the Weeks dictionary
                Dictionary<string, int> Weeks = Enumerable.Range(1, totalWeeks).ToDictionary(weekNumber => $"Week{weekNumber}", weekNumber => selfstudy);

                // Calculate total self-study hours for all weeks
                int totalHours = Weeks.Values.Sum();

                // Create a new displayData item for each module
                displayData item = new displayData
                {
                    Name = module.ModuleName1,
                    Weeks = Weeks,
                    TotalHours = totalHours
                };

                // Add the item to the list
                storeData.Add(item);

                // Send the module name and self-study hours to CaptureValues
                CaptureValues(module, selfstudy);
            }


            GridViewData.ItemsSource = storeData;

        }



        private int CalculateTotalWeeks(List<SemesterDatesWeeks> semesterList)
        {
            // Implement your logic to calculate total weeks here

            // For example, you can calculate it based on semesterList or other data
            int totalWeeks = semesterList.Sum(semester => semester.Weeks1);


            return totalWeeks;
        }


        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            bool validationPasssed = true;
            int RequriedRemHrs = 0;
            int RemHrs;
            string module;
            string SelectDate = "";
            ExceptionHandlingPOE exception = new ExceptionHandlingPOE();

            ///Module Name
            if (ModuleComboBox.SelectedItem != null)
            {
                module = ModuleComboBox.SelectedItem.ToString();
            }
            else
            {
                MessageBox.Show("Please select a module.");
                return;
            }

            ///Module Remaining Hours
            if (ModuleRemainingHrs.SelectedItem != null)
            {
                RemHrs = exception.NumValid1(ModuleRemainingHrs.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("Please select remaining hours.");
                return;
            }

            ///Requried Hours
            try
            {
                RequriedRemHrs = exception.NumValid(CaptureValuesText.Text.Trim(), "Capture Hours");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                CaptureValuesText.Text = "";
                validationPasssed = false;
            }

            ///Select Date for Study
            try
            {
                SelectDate = exception.DateHandling(ModuleDatePicker.Text);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                validationPasssed = false;
            }


            if (validationPasssed)
            {
                // Calculate total weeks here 
                int totalWeeks = CalculateTotalWeeks(semList);

                // It Put back the value to self study hours
                int final = RemHrs - RequriedRemHrs;

                DateTime startDate = DateTime.Parse(date);

                DateTime selectedDate = DateTime.Parse(SelectDate);

                int selectedWeek = (selectedDate - startDate).Days / 7 + 1;

                for (int i = 1; i <= totalWeeks; i++)
                {
                    // Check if the current week matches the selected week
                    if (i == selectedWeek)
                    {
                        MessageBox.Show($"Week {i} matches the selected week!");

                        // Update the self-study hours for the selected week of the specific module
                        List<displayData> rows = (List<displayData>)GridViewData.ItemsSource;
                        foreach (displayData row in rows)
                        {
                            if (row.Name == module) // module is selected from the combo box
                            {
                                string selectedWeekKey = $"Week{selectedWeek}";
                                if (row.Weeks.ContainsKey(selectedWeekKey))// module is selected from the combo box
                                {
                                    // Update the self-study hours for the selected week
                                    row.Weeks[selectedWeekKey] = final;

                                    // Update the self-study hours for the selected week
                                    row.TotalHours = row.Weeks.Values.Sum();
                                }
                                break; // Exit the loop once the update is done
                            }
                        }
                    }
                }

                // Refresh the GridView to reflect the changes
                GridViewData.Items.Refresh();

            }
        }

        /// <summary>
        /// Restriction for Date
        /// </summary>
        /// <param name="startDate"></param>
        private void SelectedDate(DateTime startDate)
        {
            // Assuming startDate and totalWeeks are already defined
            DateTime endDate = startDate.AddDays(totalWeeks * 7);
            ModuleDatePicker.DisplayDateStart = startDate;
            ModuleDatePicker.DisplayDateEnd = endDate;
        }

        /// <summary>
        /// Capture values and store it in Combo box
        /// </summary>
        /// <param name="module"></param>
        /// <param name="selfstudy"></param>
        private void CaptureValues(ModulesInfo module, int selfstudy)
        {
            // Check if the module name already exists in the combo box
            if (!ModuleComboBox.Items.Cast<string>().Any(item => item == module.ModuleName1))
            {
                ModuleComboBox.Items.Add(module.ModuleName1);
            }

            // Check if the self-study hours already exist in the combo box
            if (!ModuleRemainingHrs.Items.Cast<int>().Any(item => item == selfstudy))
            {
                ModuleRemainingHrs.Items.Add(selfstudy);
            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpeningMenu main = new OpeningMenu();
            main.Show();
            Close();
        }
    }
}
