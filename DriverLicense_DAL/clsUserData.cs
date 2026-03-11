using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicense_DAL
{
    public class clsUserData
    {

        public static clsUser GetUserInfoByUserID(int UserID)
        {
            clsUser user = null;

            string query = "SELECT * FROM Users WHERE UserID = @UserID;";

            using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new clsUser();

                                user.PersonID = (int)reader["PersonID"];
                                user.UserName = (string)reader["UserName"];
                                user.Password = (string)reader["Password"];
                                user.IsActive = (bool)reader["IsActive"];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        user = null;
                    }
                }
            }

            return user;
        }

        public static clsUser GetUserInfoByPersonID(int PersonID)
        {
            clsUser user = null;

            string query = "SELECT * FROM Users WHERE PersonID = @PersonID;";

            using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new clsUser();

                                user.UserID = (int)reader["UserID"];
                                user.UserName = (string)reader["UserName"];
                                user.Password = (string)reader["Password"];
                                user.IsActive = (bool)reader["IsActive"];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        user = null;
                    }
                }
            }

            return user;
        }

        public static bool GetUserInfoByUsernameAndPassword(string UserName, string Password, ref int UserID,
            ref int PersonID, ref bool IsActive)
        {
            bool isFound = false;

            string query = "SELECT * FROM Users WHERE UserName = @UserName And Password =@Password;";

            using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Password", Password);


                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                isFound = true;

                                UserID = (int)reader["UserID"];
                                PersonID = (int)reader["PersonID"];
                                IsActive = (bool)reader["IsActive"];

                            }

                            else { isFound = false; }

                        }
                    }
                    catch (Exception ex) { isFound = false; }


                }

            }

            return isFound;
        }

        public static int AddNewUser(int PersonID, string UserName,
        string Password, bool IsActive)
        {
            int UserID = -1;

            string query = @"INSERT INTO Users (PersonID,UserName,Password,IsActive)
                     VALUES (@PersonID,@UserName,@Password,@IsActive);
                     SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@UserName", UserName);
                        command.Parameters.AddWithValue("@Password", Password);
                        command.Parameters.AddWithValue("@IsActive", IsActive);

                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            UserID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return UserID;
        }

        public static bool UpdateUser(int UserID, int PersonID, string UserName,
        string Password, bool IsActive)
        {
            int rowsAffected = 0;

            string query = @"UPDATE Users  
                     SET PersonID = @PersonID,
                         UserName = @UserName,
                         Password = @Password,
                         IsActive = @IsActive
                     WHERE UserID = @UserID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@UserName", UserName);
                        command.Parameters.AddWithValue("@Password", Password);
                        command.Parameters.AddWithValue("@IsActive", IsActive);
                        command.Parameters.AddWithValue("@UserID", UserID);

                        connection.Open();

                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return (rowsAffected > 0);
        }

        public static bool DeleteUser(int UserID)
        {
            int rowsAffected = 0;

            string query = @"DELETE FROM Users 
                     WHERE UserID = @UserID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@UserID", UserID);

                        connection.Open();

                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return (rowsAffected > 0);
        }

        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT Users.UserID, Users.PersonID,
                    FullName = People.FirstName + ' ' + People.SecondName + ' ' +
                    ISNULL(People.ThirdName,'') + ' ' + People.LastName,
                    Users.UserName, Users.IsActive
                    FROM Users
                    INNER JOIN People ON Users.PersonID = People.PersonID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return dt;
        }

        public static bool IsUserExist(int UserID)
        {
            bool isFound = false;

            string query = "SELECT Found=1 FROM Users WHERE UserID = @UserID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@UserID", UserID);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            isFound = reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isFound = false;
            }

            return isFound;
        }

        public static bool IsUserExist(string UserName)
        {
            bool isFound = false;

            string query = "SELECT Found=1 FROM Users WHERE UserName = @UserName";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@UserName", UserName);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            isFound = reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isFound = false;
            }

            return isFound;
        }

        public static bool IsUserExistForPersonID(int PersonID)
        {
            bool isFound = false;

            string query = "SELECT Found=1 FROM Users WHERE PersonID = @PersonID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@PersonID", PersonID);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            isFound = reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isFound = false;
            }

            return isFound;
        }

        public static bool DoesPersonHaveUser(int PersonID)
        {
            bool isFound = false;

            string query = "SELECT Found=1 FROM Users WHERE PersonID = @PersonID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@PersonID", PersonID);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            isFound = reader.HasRows;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isFound = false;
            }

            return isFound;
        }

        public static bool ChangePassword(int UserID, string NewPassword)
        {
            int rowsAffected = 0;

            string query = @"UPDATE Users  
                     SET Password = @Password
                     WHERE UserID = @UserID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@Password", NewPassword);

                        connection.Open();

                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return (rowsAffected > 0);
        }

    }
}

