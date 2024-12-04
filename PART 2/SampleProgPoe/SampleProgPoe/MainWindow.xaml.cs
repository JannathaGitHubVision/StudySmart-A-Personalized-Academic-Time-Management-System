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
using SampleDLL;

namespace SampleProgPoe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<StoringUsersValues> Users;
        List<ModulesInfo> Modules;
        private static int loggedID = Login.loginID;

        /// <summary>
        /// Iam getting values for this
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Users = new List<StoringUsersValues>();
            Modules = new List<ModulesInfo>();
        }

        //The values of Register information
        public void UpdateUsers(StoringUsersValues newUsers)
        {
            Users.Add(newUsers);
        }

        //The values for Modules
        public void StoreModules(List<ModulesInfo> newModules)
        {
            Modules = newModules;
        }

        /// <summary>
        /// Display class
        /// </summary>
        /// <param name="newModules"></param>
        public void openDisplay(List<ModulesInfo> newModules)
        {
            Modules = newModules;
            DisplayAcademics_ displayAcademics_ = new DisplayAcademics_(Modules);
            displayAcademics_.Show();

        }

        private void NaviagateModule(object sender, RoutedEventArgs e)
        {
            CaptureModules captureModules = new CaptureModules();
            captureModules.Show();
            Close();
        }

        /// <summary>
        /// Iam getting the user's info for seeing their own data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckingUserInfo(object sender, RoutedEventArgs e)
        {
            DisplayAcademics_ displayValues = new DisplayAcademics_(Modules);
            displayValues.LoadValues(loggedID);
            displayValues.Show();
            Close();
        }

    }
}
