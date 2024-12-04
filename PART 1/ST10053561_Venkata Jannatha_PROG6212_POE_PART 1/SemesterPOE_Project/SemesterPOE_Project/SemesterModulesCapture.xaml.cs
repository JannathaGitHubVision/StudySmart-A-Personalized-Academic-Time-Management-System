using PoeDLL;
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
using System.Windows.Shapes;

namespace SemesterPOE_Project
{
    /// <summary>
    /// Interaction logic for SemesterModulesCapture.xaml
    /// </summary>
    public partial class SemesterModulesCapture : Window
    {
        public SemesterModulesCapture()
        {
            InitializeComponent();
        }

        private List<ModulesInfo> modulesList;
        public SemesterModulesCapture(List<ModulesInfo> modulesL)
        {
            InitializeComponent();
            modulesList = modulesL;
        }

        /// <summary>
        /// Here Iam storing the info of module here
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StoringInfo(object sender, RoutedEventArgs e)
        {
            ExceptionHandlingPOE exception = new ExceptionHandlingPOE();

            string ModuleCode = "";
            string ModuleName = "";
            int ModuleCredits = 0;
            int ModuleClassHrs = 0;
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

            ///The validation is assigned to every value where if all the validationPassed is true then activate.
            if (validationPasssed)
            {
                ///Storing onto a constructor
                ModulesInfo modules = new ModulesInfo(ModuleName, ModuleCode, ModuleCredits, ModuleClassHrs);

                // Check if modulesList is null
                if (modulesList == null)
                {
                    // If it's null, create a new list
                    modulesList = new List<ModulesInfo>();
                }

                // Add the module to the list
                modulesList.Add(modules);

                MessageBox.Show("Successfully captured the values", "Success Message", MessageBoxButton.OK, MessageBoxImage.Information);

                SemesterDates semesterDates = new SemesterDates(modulesList);
                semesterDates.Show();
                Close();

                ///It clear the value after message box
                ModuleCodeText.Text = "";
                ModuleNameText.Text = "";
                ModuleCreditsNum.Text = "";
                ModuleClasshrsNum.Text = "";
            }

        }

    }
}
