using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
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
using System.Windows.Shapes;
using SampleDLL;

namespace SampleProgPoe
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {

            InitializeComponent();
        }

        /// <summary>
        /// These are the values that I capture for the Register 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitButton(object sender, RoutedEventArgs e)
        {

            ExceptionHandling exceptionHandling = new ExceptionHandling();
            string name;
            string surname;
            string username;
            string password;
            bool validationPasssed = true;


            ///Name
            try
            {
                name = exceptionHandling.StringValid(nameText.Text.Trim() , "Name");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                nameText.Text = "";
                validationPasssed = false;
            }

            ///Surname
            try
            {
                surname = exceptionHandling.StringValid(surnameText.Text.Trim(),"Surname");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                surnameText.Text = "";
                validationPasssed = false;
            }

            ///Username
            try
            {
                username = exceptionHandling.StringValid(usernameText.Text.Trim(),"Username");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                usernameText.Text = "";
                validationPasssed = false;
            }

            ///Password
            try
            {
                password = exceptionHandling.PasswordHanlding(passwordText.Text.Trim(),"Password");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                passwordText.Text = "";
                validationPasssed = false;
            }

            if (validationPasssed)
            {
                StoringUsersValues users = new StoringUsersValues
                {
                    Name = nameText.Text.Trim(),
                    Surname = surnameText.Text.Trim(),
                    Username = usernameText.Text.Trim(),
                    Password = HashPassword(passwordText.Text.Trim()),
                };

                //Iam sending the values to a database
                DBController dBController = new DBController();
                dBController.AddUsersValues(users);

                //Sending the values to the main class
                MainWindow mainWindow = new MainWindow();
                mainWindow.UpdateUsers(users);

                MessageBox.Show("Successfully captured the values", "Success Message", MessageBoxButton.OK, MessageBoxImage.Information);

                Login login = new Login();
                login.Show();
                Close();
            }


        }

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
    }
}
