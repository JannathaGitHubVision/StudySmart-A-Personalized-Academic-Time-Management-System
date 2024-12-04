using Newtonsoft.Json;
using SampleDLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProgPoe
{
    public class DBController
    {
        SqlConnection con;
        SqlCommand cmd;
        public DBController()
        {
            con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\UsersInfo2.mdf;Integrated Security=True");
        }

        public void AddUsersValues(StoringUsersValues users)
        {
            con.Open();

            string name = users.Name;
            string surname = users.Surname;
            string username = users.Username;
            string password = users.Password;

            cmd = new SqlCommand($"INSERT INTO users(name, surname, username, password)" +
                        $"VALUES ('{name}','{surname}', '{username}', '{password}')", con);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public List<StoringUsersValues> GetUsersValues()
        {
            con.Open();
            cmd = new SqlCommand("SELECT * FROM users", con);
            SqlDataReader reader = cmd.ExecuteReader();

            List<StoringUsersValues> usersList = new List<StoringUsersValues>();
            while (reader.Read())
            {
                StoringUsersValues user = new StoringUsersValues();
                user.Name = reader["name"].ToString();
                user.Surname = reader["surname"].ToString();
                user.Username = reader["username"].ToString();
                user.Password = reader["password"].ToString();

                usersList.Add(user);
            }

            con.Close();
            return usersList;
        }

        /// <summary>
        /// it is just get the user's ID in username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GetUserId(string username)
        {
            con.Open();
            cmd = new SqlCommand("SELECT * FROM Users WHERE Username = @Username", con);
            cmd.Parameters.AddWithValue("@Username", username);
            SqlDataReader reader = cmd.ExecuteReader();

            int userId = -1; // Default value indicating user not found
            if (reader.Read())
            {
                userId = Convert.ToInt32(reader["id"]); // Assuming the ID column in your database is named "id"
            }

            con.Close();
            return userId;
        }

        /// <summary>
        /// Adding the values of a module in a 'Capture
        /// </summary>
        /// <param name="modules"></param>
        public void AddModuleValues(ModulesInfo modules)
        {
            con.Open();

            int usersID = modules.UsersID;
            string moduleName = modules.ModuleName1;
            string moduleCode = modules.ModuleCode1;
            int moduleClassHrs = modules.ModuleClassHrs1;
            int moduleCredits = modules.ModuleCredits1;
            int semWeeks = modules.Weeks1;
            string semDate = modules.Date1;
            int selfStudyHour = modules.selfStudyHr;

            cmd = new SqlCommand("INSERT INTO CaptureModules(UserID, ModuleName, ModuleCode, ModuleClassHrs, ModuleCredits, SemWeeks, SemDate, SelfStudyHour) VALUES (@usersID, @moduleName, @moduleCode, @moduleClassHrs, @moduleCredits, @semWeeks, @semDate, @selfStudyHour)", con);

            cmd.Parameters.AddWithValue("@usersID", usersID);
            cmd.Parameters.AddWithValue("@moduleName", moduleName);
            cmd.Parameters.AddWithValue("@moduleCode", moduleCode);
            cmd.Parameters.AddWithValue("@moduleClassHrs", moduleClassHrs);
            cmd.Parameters.AddWithValue("@moduleCredits", moduleCredits);
            cmd.Parameters.AddWithValue("@semWeeks", semWeeks);
            cmd.Parameters.AddWithValue("@semDate", semDate);
            cmd.Parameters.AddWithValue("@selfStudyHour", selfStudyHour);

            cmd.ExecuteNonQuery();


            con.Close();
        }

        /// <summary>
        /// Getting the Modules values that I store in a sql database
        /// </summary>
        /// <returns></returns>
        public List<ModulesInfo> GetModuleValues()
        {
            List<ModulesInfo> ModulesList = new List<ModulesInfo>();
            con.Open();
            cmd = new SqlCommand("SELECT * FROM CaptureModules", con);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ModulesInfo module = new ModulesInfo();

                module.UsersID = Convert.ToInt32(reader["UserID"]);
                module.ModuleName1 = reader["ModuleName"].ToString();
                module.ModuleCode1 = reader["ModuleCode"].ToString();
                module.ModuleClassHrs1 = Convert.ToInt32(reader["ModuleClassHrs"]);
                module.ModuleCredits1 = Convert.ToInt32(reader["ModuleCredits"]);
                module.Weeks1 = Convert.ToInt32(reader["SemWeeks"]);
                module.Date1 = reader["SemDate"].ToString();
                module.selfStudyHr = Convert.ToInt32(reader["SelfStudyHour"]);

                ModulesList.Add(module);
            }

            con.Close();
            return ModulesList;
        }


        public void UpdateSelfStudyHours(int usersID, int selfStudyHours, string moduleName, int weekNumber)
        {
            con.Open();

            string sql = @"
        IF EXISTS (SELECT * FROM ModifiedHoursTable WHERE UserID = @userID AND ModuleName = @moduleName AND WeekNumber = @weekNumber)
            UPDATE ModifiedHoursTable SET ModifiedHours = @modifiedHours WHERE UserID = @userID AND ModuleName = @moduleName AND WeekNumber = @weekNumber
        ELSE
            INSERT INTO ModifiedHoursTable(UserID, ModuleName, WeekNumber, ModifiedHours) VALUES (@userID, @moduleName, @weekNumber, @modifiedHours)
    ";

            cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@userID", usersID);
            cmd.Parameters.AddWithValue("@moduleName", moduleName);
            cmd.Parameters.AddWithValue("@weekNumber", weekNumber);
            cmd.Parameters.AddWithValue("@modifiedHours", selfStudyHours);

            cmd.ExecuteNonQuery();
            con.Close();
        }


        public List<ModifiedHoursInfo> GetModifiedHours(int usersID)
        {
            Dictionary<string, ModifiedHoursInfo> modifiedHoursDict = new Dictionary<string, ModifiedHoursInfo>();

            con.Open();

            string sql = @"SELECT ModuleName, WeekNumber, ModifiedHours FROM ModifiedHoursTable WHERE UserID = @userID";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@userID", usersID);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string moduleName = reader.GetString(0);
                    int weekNumber = reader.GetInt32(1);
                    int modifiedHours = reader.GetInt32(2);

                    if (!modifiedHoursDict.TryGetValue(moduleName, out ModifiedHoursInfo modifiedHour))
                    {
                        // If the module doesn't exist in the dictionary, create a new ModifiedHoursInfo
                        modifiedHour = new ModifiedHoursInfo();
                        modifiedHour.UserID = usersID;
                        modifiedHour.ModuleName = moduleName;
                        modifiedHour.ModifiedHours = modifiedHours;
                        modifiedHour.WeekModifiedHours = new Dictionary<string, int>();

                        // Add it to the dictionary
                        modifiedHoursDict.Add(moduleName, modifiedHour);
                    }

                    // Add the modified hours for the week to the WeekModifiedHours dictionary
                    // Add the modified hours for the week to the WeekModifiedHours dictionary
                    modifiedHour.WeekModifiedHours.Add($"Week{weekNumber}", modifiedHours);

                }
            }

            con.Close();

            // Convert the dictionary values to a list and return it
            return new List<ModifiedHoursInfo>(modifiedHoursDict.Values);
        }

        public List<ModifiedHoursInfo> GetModifiedHoursData(int usersID)
        {
            List<ModifiedHoursInfo> modifiedHoursInfos = new List<ModifiedHoursInfo>();
            con.Open();

            string sql = @"SELECT ModuleName, WeekNumber, ModifiedHours FROM ModifiedHoursTable WHERE UserID = @userID";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@userID", usersID);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ModifiedHoursInfo info = new ModifiedHoursInfo();

                info.UserID = usersID;
                info.ModuleName = reader["ModuleName"].ToString();
                info.WeekNumber = Convert.ToInt32(reader["WeekNumber"]);
                info.ModifiedHours = Convert.ToInt32(reader["ModifiedHours"]);
                // Add the modified hours to the WeekModifiedHours dictionary
                info.WeekModifiedHours.Add($"Week{info.WeekNumber}", info.ModifiedHours);
                modifiedHoursInfos.Add(info);
            }

            con.Close();
            return modifiedHoursInfos;
        }


    }
}
