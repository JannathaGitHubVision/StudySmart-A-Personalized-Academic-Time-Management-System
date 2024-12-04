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
    /// Interaction logic for OpeningMenu.xaml
    /// </summary>
    public partial class OpeningMenu : Window
    {
        public OpeningMenu()
        {
            InitializeComponent();
        }

        private List<ModulesInfo> modulesList;
        private void NaviagateModule(object sender, RoutedEventArgs e)
        {
            SemesterModulesCapture semesterModules = new SemesterModulesCapture(modulesList);
            semesterModules.Show();
            Close();
        }

        private void CheckingUserINfo(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("We are Still Working on it", "The message", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
