using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;
using SampleDLL;
namespace SampleProgPoe
{
    /// <summary>
    /// Interaction logic for CaptureModules.xaml
    /// </summary>
    public partial class CaptureModules : Window
    {
        private List<ModulesInfo> modulesInfos = new List<ModulesInfo>();
        public static int logedID = Login.loginID;
        public CaptureModules()
        {
            InitializeComponent();

        }
        public CaptureModules(List<ModulesInfo> existingModules)
        {
            InitializeComponent();
            modulesInfos = existingModules;
        }

        /// <summary>
        /// Here Iam storing the info of module here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void StoringInfo(object sender, RoutedEventArgs e)
        {
            ExceptionHanldingValues exception = new ExceptionHanldingValues();

            string ModuleCode = "";
            string ModuleName = "";
            int ModuleCredits = 0;
            int ModuleClassHrs = 0;
            int SemesterWeeks = 0;
            string SemesterDate = "";
            bool validationPasssed = true;
            ///Module code
            try
            {
                ModuleCode = exception.ModuleCode(ModuleCodeText.Text.Trim());

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ModuleCodeText.Text = "";
                validationPasssed = false;
            }
            ///Module Name
            try
            {
                ModuleName = exception.StringValid(ModuleNameText.Text.Trim(), "Module Name");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ModuleNameText.Text = "";
                validationPasssed = false;
            }

            ///Module Credits
            try
            {
                ModuleCredits = exception.NumValid(ModuleCreditsNum.Text.Trim(), "Module Credits");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ModuleCreditsNum.Text = "";
                validationPasssed = false;
            }

            ///Module class hrs
            try
            {
                ModuleClassHrs = exception.NumValid(ModuleClasshrsNum.Text.Trim(), "Module Class hours");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ModuleClasshrsNum.Text = "";
                validationPasssed = false;
            }
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


            ///The validation is assigned to every value where if all the validationPassed is true then activate.
            if (validationPasssed)
            {
                MessageBox.Show("Successfully captured the values", "Success Message", MessageBoxButton.OK, MessageBoxImage.Information);

                int moduleSelfStudy = (ModuleCredits * 10 / SemesterWeeks) - ModuleClassHrs;

                ///Storing onto a constructor
                ModulesInfo modules = new ModulesInfo
                {
                    UsersID = logedID,
                    ModuleName1 = ModuleName,
                    ModuleCode1 = ModuleCode,
                    ModuleClassHrs1 = ModuleClassHrs,
                    ModuleCredits1 = ModuleCredits,
                    Weeks1 = SemesterWeeks,
                    Date1 = SemesterDate,
                    selfStudyHr = moduleSelfStudy
                };

                //Storing the values in a list
                modulesInfos.Add(modules);

                DBController dBController = new DBController();
                //Adding values to the SQL Databae
                dBController.AddModuleValues(modules);

                //Sending values to a main class
                MainWindow mainWindow = new MainWindow();
                mainWindow.StoreModules(modulesInfos);


                ModuleCodeText.Text = "";
                ModuleNameText.Text = "";
                ModuleClasshrsNum.Text = "";
                ModuleCreditsNum.Text = "";
                WeeksNumText.Text = "";
                DateText.Text = "";
            }
        }
        /// <summary>
        /// Sending back to menu
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

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
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

            // Example: Check if ModuleCodeText has unsaved changes
            if (ModuleCodeText.Text != "")
            {
                dataModified = true;
            }
            else if (ModuleNameText.Text != "")
            {
                dataModified = true;
            }
            else if (ModuleClasshrsNum.Text != "")
            {
                dataModified = true;
            }
            else if (ModuleCreditsNum.Text != "")
            {
                dataModified = true;
            }
            else if (WeeksNumText.Text != "")
            {
                dataModified = true;
            }
            else if (DateText.Text != "")
            {
                dataModified = true;
            }

            // Add similar checks for other input fields.
            return dataModified;
        }

        private void DisplayAcademi(object sender, RoutedEventArgs e)
        {
            //Going to the display class
            MainWindow mainWindow = new MainWindow();
            mainWindow.openDisplay(modulesInfos);
            Close();
        }


        private void Display(object sender, RoutedEventArgs e)
        {
            DisplayAcademics_ displayValues = new DisplayAcademics_(modulesInfos);
            displayValues.LoadValues(logedID);
            displayValues.Show();
            Close();
        }
    }
}

