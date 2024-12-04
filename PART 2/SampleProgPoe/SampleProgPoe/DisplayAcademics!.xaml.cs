using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;
using SampleDLL;
using System.Threading.Tasks;
namespace SampleProgPoe
{
    /// <summary>
    /// Interaction logic for DisplayAcademics_.xaml
    /// </summary>
    public partial class DisplayAcademics_ : Window
    {
        //Declare variables
        List<ModulesInfo> Modules;
        string SelectDate;
        int totalWeeks = 0;
        string date;
        double moduleSelfStudy;
        int RequriedRemHrs = 0;
        string selectedModule;

        /// <summary>
        /// This is where Iam getting the loginID
        /// </summary>
        public static int loginID = Login.loginID;
        public DisplayAcademics_()
        {
            InitializeComponent();
        }
        public DisplayAcademics_(List<ModulesInfo> modules)
        {
            InitializeComponent();
            Modules = modules;
            this.Loaded += Activation;
        }

        private void Activation(object sender, RoutedEventArgs e)
        {
            AddValues();
        }

        //Implementing the multi-threading
        public void InvokeMethodsInNewThreads()
        {
            Task[] tasks = new Task[2];

            tasks[0] = Task.Run(() => AddValues());
            tasks[1] = Task.Run(() => LoadValues(loginID));

            Task.WaitAll(tasks); // if you want to wait for all tasks to complete
        }

        /// <summary>
        /// Sending values back to menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackToMenu(object sender, RoutedEventArgs e)
        {
            //make sure user don't lose the data by switching tabs
            if (IsDataModified())
            {
                MessageBoxResult result = MessageBox.Show("You have unsaved changes. Do you want to continue without saving?", "Unsaved Changes", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            //Iam making sure, to provide the values to the user
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }


        /// <summary>
        /// Iam adding the values in a combo box, and I adding to that
        /// </summary>
        public void AddValues()
        {
            for (int i = 0; i < Modules.Count; i++)
            {
                ModulesInfo module = Modules[i];

                //Adding the values of Module names
                ModuleComboBox.Items.Add(module.ModuleName1);

            }

        }


        /// <summary>
        /// Make sure data is not empty or left
        /// </summary>
        /// <returns></returns>
        private bool IsDataModified()
        {
            // Check if any of your input fields have unsaved changes.
            // I can compare the current values with the original values here.
            // Return true if changes are detected, false otherwise.

            bool dataModified = false;
            if (CaptureValuesText.Text != "")
            {
                dataModified = true;
            }

            // Add similar checks for other input fields.
            return dataModified;
        }


        /// <summary>
        /// Iam restricting the date here, based on provide values 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="totalWeeks"></param>
        /// <param name="module"></param>
        private void DateResctriction(out string date, out int totalWeeks, ModulesInfo module)
        {
            totalWeeks = module.Weeks1;
            //This is for restriction the weeks
            date = module.Date1;
            DateTime startDate = DateTime.Parse(date); // 'date' is the date I chose

            // Add the number of weeks to the start date
            DateTime endDate = startDate.AddDays(totalWeeks * 7);


            // Set the DisplayDateStart and DisplayDateEnd properties
            ModuleDatePicker.DisplayDateStart = startDate;
            ModuleDatePicker.DisplayDateEnd = endDate;

        }


        /// <summary>
        /// These are the values that I capture using button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitButton(object sender, RoutedEventArgs e)
        {
            bool validationPasssed = true;
            ExceptionHanldingValues exception = new ExceptionHanldingValues();
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

            if (validationPasssed)
            {
                ///If modules list don't contain the values then go to different method
                if (Modules.Count > 0)
                {
                    UpdatingValuesDataGrid(RequriedRemHrs);
                    CaptureValuesText.Text = "";
                }
                else
                {
                    // Call UpdatingModifyValues and check if it returns true
                    bool isUpdateSuccessful = UpdatingModifyValues();

                    // Only show the success message if the update was successful
                    if (isUpdateSuccessful)
                    {
                        MessageBox.Show("Successfully Updated the  modify values In a table", "Success Message", MessageBoxButton.OK, MessageBoxImage.Information);
                        CaptureValuesText.Text = "";
                    }
                }


            }

        }

        /// <summary>
        /// Iam actually updating the modify values
        /// </summary>
        private bool UpdatingModifyValues()
        {
            ///These are the objects I gather
            DBController dBController = new DBController();
            List<ModulesInfo> modulesInfos = dBController.GetModuleValues();
            List<ModifiedHoursInfo> modifiedHours = dBController.GetModifiedHours(loginID);
            ModulesInfo selectedModuleInfo = modulesInfos.FirstOrDefault(m => m.ModuleName1 == selectedModule);
            ModifiedHoursInfo selectModifiedinfo = modifiedHours.FirstOrDefault(m => m.ModuleName == selectedModule);


            //Calculations for the selected week
            DateTime startDate = DateTime.Parse(selectedModuleInfo.Date1);
            DateTime selectedDate = DateTime.Parse(SelectDate);
            int selectedWeek = (selectedDate - startDate).Days / 7 + 1;

            string weekKey = $"Week{selectedWeek}";


            if (RequriedRemHrs > 0)
            {
                // Calculate the final value by subtracting the required hours from the selected week's modified hours
                int final = selectModifiedinfo.WeekModifiedHours[weekKey] - RequriedRemHrs;

                // Check if the required hours are less than or equal to the selected week's modified hours
                if (RequriedRemHrs <= selectModifiedinfo.WeekModifiedHours[weekKey])
                {
                    // If it is, update the final value in the WeekModifiedHours dictionary
                    selectModifiedinfo.WeekModifiedHours[weekKey] = final;

                    // Update the self-study hours in the database
                    dBController.UpdateSelfStudyHours(loginID, final, selectedModule, selectedWeek);

                    // Refresh the data grid view
                    modifiedHours = dBController.GetModifiedHours(loginID);
                    GridViewData.ItemsSource = new List<ModifiedHoursInfo> { selectModifiedinfo };

                    // Return true to indicate that the update was successful
                    return true;
                }
                else
                {
                    // If the required hours are more than the selected week's modified hours, show an error message
                    MessageBox.Show("Please provide a value less than or equal to the selected week's modified hours.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    // Return false to indicate that the update was not successful
                    return false;
                }
            }
            else
            {
                // If RequriedRem is 0 or the week doesn't exist in the dictionary, add a default value
                selectModifiedinfo.WeekModifiedHours[weekKey] = 0;
                return false;
            }


        }


        // Define a data structure to store self-study hours for each module and week
        Dictionary<string, Dictionary<string, int>> moduleSelfStudyHours = new Dictionary<string, Dictionary<string, int>>();
        private void UpdatingValuesDataGrid(int RequriedRemHrs)
        {
            // Get the selected module name from the ComboBox
            string selectedModuleName = (string)ModuleComboBox.SelectedItem;

            for (int i = 0; i < Modules.Count; i++)
            {
                if (Modules[i].ModuleName1 == selectedModuleName)
                {
                    //Modules info values
                    ModulesInfo module = Modules[i];

                    //Weeks store
                    int week = module.Weeks1;

                    //storing date
                    date = module.Date1;

                    //Getting selected week based on chosen date
                    DateTime startDate = DateTime.Parse(date);
                    DateTime selectedDate = DateTime.Parse(SelectDate);
                    int selectedWeek = (selectedDate - startDate).Days / 7 + 1;


                    for (int v = 1; v <= week; v++)
                    {
                        // Calculate self study hours for the selected module
                        moduleSelfStudy = (module.ModuleCredits1 * 10 / module.Weeks1) - module.ModuleClassHrs1;

                        // Check if the current week matches the selected week
                        if (v == selectedWeek)
                        {
                            // If the required hours are less than or equal to the module self study hours
                            if (RequriedRemHrs > moduleSelfStudy)
                            {
                                // If the required hours are more than the module self study hours, show an error message
                                MessageBox.Show("Please provide a value less than or equal to the module self study hours.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            else
                            {
                                // Subtract the required hours from the module self study hours
                                moduleSelfStudy -= RequriedRemHrs;
                            }
                        }


                        // Update the self-study hours for each week of the specific module
                        List<DisplayData> rows = (List<DisplayData>)GridViewData.ItemsSource;

                        foreach (DisplayData row in rows)
                        {
                            if (row.ModuleName == selectedModuleName)
                            {
                                string weekKey = $"Week{v}";
                                if (row.WeekHours.ContainsKey(weekKey))
                                {
                                    // Update the self-study hours for the current week
                                    row.WeekHours[weekKey] = moduleSelfStudy;

                                    // Update the total hours
                                    row.TotalHours = row.WeekHours.Values.Sum();

                                    // Save updated self-study hours in dictionary
                                    if (!moduleSelfStudyHours.ContainsKey(selectedModuleName))
                                    {
                                        moduleSelfStudyHours[selectedModuleName] = new Dictionary<string, int>();
                                    }
                                    moduleSelfStudyHours[selectedModuleName][weekKey] = (int)moduleSelfStudy;

                                    // Update the self-study hours in the database
                                    DBController dBController = new DBController();
                                    dBController.UpdateSelfStudyHours(loginID, (int)moduleSelfStudy, selectedModuleName, v);
                                }
                                break;
                            }
                        }
                    }

                }
                // Refresh the GridView to reflect the changes
                GridViewData.Items.Refresh();
                MessageBox.Show("Successfully Updated the  modify values In a table", "Success Message", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }


        ModifiedHoursInfo ModifiedHoursInfo;
        public void LoadValues(int loggedID)
        {
            if (loggedID == 0)
            {
                loggedID = loginID;

            }

            DBController dBController = new DBController();
            List<ModulesInfo> modulesInfos = dBController.GetModuleValues();
            List<ModifiedHoursInfo> modifiedHours = dBController.GetModifiedHours(loggedID);


            ModuleComboBox.Items.Clear();
            GridViewData.Columns.Clear();

            // Get unique modules from the List
            HashSet<string> uniqueModules = new HashSet<string>();

            foreach (ModifiedHoursInfo modified in modifiedHours)
            {
                if (modified.UserID == loggedID)
                {
                    uniqueModules.Add(modified.ModuleName);
                }
            }

            foreach (string module in uniqueModules)
            {
                ModuleComboBox.Items.Add(module);
            }

            UpdatingModulesData(modulesInfos, modifiedHours);
        }

        private void UpdatingModulesData(List<ModulesInfo> modulesInfos, List<ModifiedHoursInfo> modifiedHours)
        {
            // Handle SelectionChanged event
            ModuleComboBox.SelectionChanged += (s, e) =>
            {
                ModuleDatePicker.IsEnabled = true;
                CaptureValuesText.IsEnabled = true;
                selectedModule = (string)ModuleComboBox.SelectedItem;

                ModifiedHoursInfo ModifedhoursInfo = null;
                foreach (ModifiedHoursInfo modified in modifiedHours)
                {
                    if (modified.ModuleName.Equals(selectedModule))
                    {
                        // Perform your action here
                        ModifedhoursInfo = modified;
                        break; // This will exit the loop once the module is found
                    }
                }

                // Get the selected module info
                ModulesInfo selectedModuleInfo = modulesInfos.FirstOrDefault(m => m.ModuleName1 == selectedModule);

                ModifiedHoursInfo selectModifiedinfo = modifiedHours.FirstOrDefault(m => m.ModuleName == selectedModule);

                Dictionary<string, int> UpdatedWeeks = new Dictionary<string, int>();

                if (selectModifiedinfo != null)
                {
                    AddingUpdatedColumns(selectedModuleInfo, selectModifiedinfo, UpdatedWeeks);

                    string value = $"Module Name: {selectedModuleInfo.ModuleName1}\n" +
                        $"Remaining Hours: {selectedModuleInfo.selfStudyHr}\n" +
                        $"Number of Weeks: {selectedModuleInfo.Weeks1}";

                    // Display in the TextBox
                    displayTextbox.Text = value;

                    ModifiedHoursInfo = new ModifiedHoursInfo
                    {
                        UserID = ModifedhoursInfo.UserID,
                        ModuleName = selectedModule,
                        WeekModifiedHours = UpdatedWeeks,

                    };

                    // Add the ModifiedHoursInfo object to the list
                    modifiedHours.Add(ModifiedHoursInfo);

                    value = UpdatingDateModel(selectedModuleInfo, value);

                }
                // Set the GridViewData.ItemsSource after setting up the columns
                GridViewData.ItemsSource = new List<ModifiedHoursInfo> { ModifiedHoursInfo };
            };
        }

        /// <summary>
        /// Updating Date in from retrieve values
        /// </summary>
        /// <param name="selectedModuleInfo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string UpdatingDateModel(ModulesInfo selectedModuleInfo, string value)
        {
            DateTime startDate = DateTime.Parse(selectedModuleInfo.Date1);
            DateTime endDate = startDate.AddDays(selectedModuleInfo.Weeks1 * 7);

            // Set the DisplayDateStart and DisplayDateEnd properties
            ModuleDatePicker.DisplayDateStart = startDate;
            ModuleDatePicker.DisplayDateEnd = endDate;


            // Handle SelectedDateChanged event for ModuleDatePicker
            ModuleDatePicker.SelectedDateChanged += (sender, eventArgs) =>
            {
                DateTime selectedDate = ModuleDatePicker.SelectedDate ?? DateTime.Now;
                int selectedWeek = (selectedDate - startDate).Days / 7 + 1;

                value = $"Module Name: {selectedModuleInfo.ModuleName1}\n" +
                    $"Selected Week: {selectedWeek}\n" +
                    $"Remaining Hours: {selectedModuleInfo.selfStudyHr}\n" +
                    $"Number of Weeks: {selectedModuleInfo.Weeks1}";

                displayTextbox.Text = value;
            };
            return value;
        }

        /// <summary>
        /// Just adding the updated columns
        /// </summary>
        /// <param name="selectedModuleInfo"></param>
        /// <param name="selectModifiedinfo"></param>
        /// <param name="UpdatedWeeks"></param>
        private void AddingUpdatedColumns(ModulesInfo selectedModuleInfo, ModifiedHoursInfo selectModifiedinfo, Dictionary<string, int> UpdatedWeeks)
        {
            GridViewData.AutoGenerateColumns = false;
            // Clear existing columns (moved this line outside of the if condition)
            GridViewData.Columns.Clear();

            // Add UserID and ModuleName columns first
            DataGridTextColumn userIdColumn = new DataGridTextColumn();
            userIdColumn.Header = "UserID";
            userIdColumn.Binding = new Binding("UserID");
            GridViewData.Columns.Add(userIdColumn);

            DataGridTextColumn moduleNameColumn = new DataGridTextColumn();
            moduleNameColumn.Header = "ModuleName";
            moduleNameColumn.Binding = new Binding("ModuleName");
            GridViewData.Columns.Add(moduleNameColumn);


            // Add a column for each week of this module
            for (int i = 1; i <= selectedModuleInfo.Weeks1; i++)
            {
                string weekKey = $"Week{i}";

                // Check if the week exists in the WeekModifiedHours dictionary
                if (selectModifiedinfo.WeekModifiedHours.ContainsKey(weekKey))
                {
                    // If it exists, add it to the UpdatedWeeks dictionary
                    UpdatedWeeks.Add(weekKey, selectModifiedinfo.WeekModifiedHours[weekKey]);
                }
                else
                {
                    // If it doesn't exist, I want to add a default value:
                    UpdatedWeeks.Add(weekKey, 0);
                }

                // Create a new column for each week and add it to the DataGrid
                DataGridTextColumn weekColumn = new DataGridTextColumn();
                weekColumn.Header = weekKey;
                weekColumn.Binding = new Binding($"WeekModifiedHours[{weekKey}]");
                GridViewData.Columns.Add(weekColumn);
            }
        }

        /// <summary>
        /// Adding the 
        /// </summary>
        private DisplayData currentDisplay;
        private Dictionary<string, DisplayData> moduleDisplayData = new Dictionary<string, DisplayData>();
        private void ModuleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModuleDatePicker.IsEnabled = true;
            CaptureValuesText.IsEnabled = true;

            string selectedModuleName = (string)ModuleComboBox.SelectedItem;
            ModulesInfo selectedModule = null;
            for (int i = 0; i < Modules.Count; i++)
            {
                if (Modules[i].ModuleName1 == selectedModuleName)
                {
                    // Perform your action here
                    selectedModule = Modules[i];
                    break; // This will exit the loop once the module is found
                }
            }


            if (selectedModule != null)
            {
                double moduleSelfStudy = (selectedModule.ModuleCredits1 * 10 / selectedModule.Weeks1) - selectedModule.ModuleClassHrs1;

                GridViewData.AutoGenerateColumns = false;

                DateResctriction(out date, out totalWeeks, selectedModule);

                string value = $"Module Name: {selectedModuleName}\n" +
                    $"Remaining Hours: {moduleSelfStudy}\n" +
                    $"Number of Weeks: {selectedModule.Weeks1}";

                displayTextbox.Text = value;

                Dictionary<string, double> Weeks = new Dictionary<string, double>();

                GridViewData.Columns.Clear();

                // Create a new column for Module Name and add it to the DataGrid
                DataGridTextColumn moduleNameColumn = new DataGridTextColumn();
                moduleNameColumn.Header = "Module Name";
                moduleNameColumn.Binding = new Binding("ModuleName");
                GridViewData.Columns.Add(moduleNameColumn);

                // Use selectedModule.Weeks1 instead of totalWeeks
                for (int i = 1; i <= selectedModule.Weeks1; i++)
                {
                    Weeks.Add($"Week{i}", moduleSelfStudy);

                    // Create a new column for each week and add it to the DataGrid
                    DataGridTextColumn weekColumn = new DataGridTextColumn();
                    weekColumn.Header = $"Week{i}";
                    weekColumn.Binding = new Binding($"WeekHours[Week{i}]");
                    GridViewData.Columns.Add(weekColumn);
                }

                // Create a new column for Total Hours and add it to the DataGrid
                DataGridTextColumn totalHoursColumn = new DataGridTextColumn();
                totalHoursColumn.Header = "Total Hours";
                totalHoursColumn.Binding = new Binding("TotalHours");
                GridViewData.Columns.Add(totalHoursColumn);

                // Calculate total self-study hours for all weeks
                double totalHours = moduleSelfStudy * selectedModule.Weeks1;

                // Check if there's existing data for this module
                if (!moduleDisplayData.TryGetValue(selectedModuleName, out currentDisplay))
                {
                    // If not, create a new DisplayData instance for the selected module
                    currentDisplay = new DisplayData
                    {
                        ModuleName = selectedModuleName,
                        TotalHours = totalHours,
                        WeekHours = Weeks
                    };

                    // Update the dictionary with the latest DisplayData
                    moduleDisplayData[selectedModuleName] = currentDisplay;

                }

                // Rebind GridView data
                GridViewData.ItemsSource = new List<DisplayData> { currentDisplay };
            }

        }

        /// <summary>
        /// Iam sending the values to the capture class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackToCapture(object sender, RoutedEventArgs e)
        {
            //make sure user don't lose the data by switching tabs
            if (IsDataModified())
            {
                MessageBoxResult result = MessageBox.Show("You have unsaved changes. Do you want to continue without saving?", "Unsaved Changes", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            CaptureModules captureModules = new CaptureModules(Modules);
            captureModules.Show();
            Close();
        }

        /// <summary>
        /// Displaying the Selected Week based on user choses
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModuleDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ModuleComboBox.SelectedItem != null)
            {
                //it takes the value based on selected module
                string selectedModuleName = (string)ModuleComboBox.SelectedItem;
                ModulesInfo selectedModule = null;

                for (int i = 0; i < Modules.Count; i++)
                {
                    if (Modules[i].ModuleName1 == selectedModuleName)
                    {
                        selectedModule = Modules[i];
                        break;
                    }
                }

                if (selectedModule != null)
                {
                    //it changes automatically based on selected date
                    DateTime selectedDate = ModuleDatePicker.SelectedDate ?? DateTime.Now; // Get the selected date or the current date if not selected

                    // Calculate the selected week based on the selected date and the module's start date
                    DateTime startDate = DateTime.Parse(selectedModule.Date1);
                    int selectedWeek = (selectedDate - startDate).Days / 7 + 1;


                    string value = $"Module Name: {selectedModuleName}\n" +
                                  $"Selected Week: {selectedWeek}\n" +
                                  $"Remaining Hours: {(selectedModule.ModuleCredits1 * 10 / selectedModule.Weeks1) - selectedModule.ModuleClassHrs1}\n" +
                                  $"Number of Weeks: {selectedModule.Weeks1}";

                    // Display in the TextBox
                    displayTextbox.Text = value;
                }
            }
        }





    }
}
