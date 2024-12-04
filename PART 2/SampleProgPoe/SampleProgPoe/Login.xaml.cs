using SampleDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace SampleProgPoe
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        public static int loginID;
        /// <summary>
        /// This is for creating a hash password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


        /// <summary>
        /// Login Button Handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        DBController dBController = new DBController();
        private void LoginButton(object sender, RoutedEventArgs e)
        {
            //Iam storing the username and password
            string username = userName.Text.Trim();
            string password = passWord.Password.Trim();

            string hashedPassword = HashPassword(password);

            ///Getting the values from users Table to match the username and password
            List<StoringUsersValues> usersFromDB = dBController.GetUsersValues();

            bool userFound = false;
            foreach (var user in usersFromDB)
            { 
                //Iam matching the username and password
                if (user.Username.Equals(username) && user.Password.Equals(hashedPassword))
                {
                    MessageBox.Show($"Welcome {user.Name.ToUpper()} ,{user.Surname.ToUpper()}  to our Time Management App ", "Welcome Message", MessageBoxButton.OK, MessageBoxImage.Information);

                    ///Iam getting the login ID that is based on username
                    loginID = dBController.GetUserId(username);

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close();

                    userFound = true;

                    break; // exit the loop once the user is found
                }
            }

            if (!userFound)
            {
                // If the user is not valid, show an error message
                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                passWord.Password = "";
                userName.Text = "";
            }

        }

        /// <summary>
        /// Register Button Handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            register.Show();
            Close();
        }

        
    }
}
