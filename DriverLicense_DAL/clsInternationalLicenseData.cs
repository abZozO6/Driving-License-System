using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicense_DAL
{
    public class clsInternationalLicenseData
    {

        public static bool GetInternationalLicenseInfoByID(int InternationalLicenseID,ref int ApplicationID, ref int DriverID, ref int IssuedUsingLocalLicenseID,
         ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive, ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = @"SELECT ApplicationID, DriverID, IssuedUsingLocalLicenseID,
                            IssueDate, ExpirationDate, IsActive, CreatedByUserID
                     FROM InternationalLicenses
                     WHERE InternationalLicenseID = @InternationalLicenseID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@InternationalLicenseID", SqlDbType.Int).Value = InternationalLicenseID;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            ApplicationID = (int)reader["ApplicationID"];
                            DriverID = (int)reader["DriverID"];
                            IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                            IssueDate = (DateTime)reader["IssueDate"];
                            ExpirationDate = (DateTime)reader["ExpirationDate"];
                            IsActive = (bool)reader["IsActive"];
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return isFound;
        }


        public static DataTable GetAllInternationalLicenses()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT InternationalLicenseID, ApplicationID,DriverID, IssuedUsingLocalLicenseID , IssueDate, 
                           ExpirationDate, IsActive from InternationalLicenses 
                           order by IsActive, ExpirationDate desc";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dt;
        }


        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            DataTable dt = new DataTable();

            string query = @"SELECT InternationalLicenseID, ApplicationID, IssuedUsingLocalLicenseID , IssueDate, 
                             ExpirationDate, IsActive from InternationalLicenses where DriverID=@DriverID
                             order by ExpirationDate desc";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@DriverID", SqlDbType.Int).Value = DriverID;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        public static int AddNewInternationalLicense(int ApplicationID,  int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate,
        bool IsActive, int CreatedByUserID)
        {
            int newID = -1;

            string query = @"INSERT INTO InternationalLicenses
                     (ApplicationID, DriverID, IssuedUsingLocalLicenseID,
                      IssueDate, ExpirationDate, IsActive, CreatedByUserID)
                     OUTPUT INSERTED.InternationalLicenseID
                     VALUES (@ApplicationID, @DriverID, @IssuedUsingLocalLicenseID,
                             @IssueDate, @ExpirationDate, @IsActive, @CreatedByUserID)";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = ApplicationID;
                    command.Parameters.Add("@DriverID", SqlDbType.Int).Value = DriverID;
                    command.Parameters.Add("@IssuedUsingLocalLicenseID", SqlDbType.Int).Value = IssuedUsingLocalLicenseID;
                    command.Parameters.Add("@IssueDate", SqlDbType.DateTime).Value = IssueDate;
                    command.Parameters.Add("@ExpirationDate", SqlDbType.DateTime).Value = ExpirationDate;
                    command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = IsActive;
                    command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = CreatedByUserID;

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                        newID = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return newID;
        }


        public static bool UpdateInternationalLicense(int InternationalLicenseID, int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID,
        DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE InternationalLicenses
                     SET ApplicationID = @ApplicationID,
                         DriverID = @DriverID,
                         IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID,
                         IssueDate = @IssueDate,
                         ExpirationDate = @ExpirationDate,
                         IsActive = @IsActive,
                         CreatedByUserID = @CreatedByUserID
                     WHERE InternationalLicenseID = @InternationalLicenseID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@InternationalLicenseID", SqlDbType.Int).Value = InternationalLicenseID;
                    command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = ApplicationID;
                    command.Parameters.Add("@DriverID", SqlDbType.Int).Value = DriverID;
                    command.Parameters.Add("@IssuedUsingLocalLicenseID", SqlDbType.Int).Value = IssuedUsingLocalLicenseID;
                    command.Parameters.Add("@IssueDate", SqlDbType.DateTime).Value = IssueDate;
                    command.Parameters.Add("@ExpirationDate", SqlDbType.DateTime).Value = ExpirationDate;
                    command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = IsActive;
                    command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = CreatedByUserID;

                    connection.Open();

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return (rowsAffected > 0);
        }



        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            int LicenseID = -1;

            string query = @"SELECT TOP 1 InternationalLicenseID
                     FROM InternationalLicenses
                     WHERE DriverID = @DriverID
                     and GetDate() between IssueDate and ExpirationDate 
                     order by ExpirationDate Desc;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@DriverID", SqlDbType.Int).Value = DriverID;

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                        LicenseID = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return LicenseID;
        }





    }
}
