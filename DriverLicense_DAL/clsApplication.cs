using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicense_DAL
{
    public class clsApplication
    {


         public static bool GetApplicationInfoByID(int ApplicationID,
         ref int ApplicantPersonID, ref DateTime ApplicationDate, ref int ApplicationTypeID,
         ref byte ApplicationStatus, ref DateTime LastStatusDate,
         ref float PaidFees, ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = "SELECT * FROM Applications WHERE ApplicationID = @ApplicationID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;

                                ApplicantPersonID = (int)reader["ApplicantPersonID"];
                                ApplicationDate = (DateTime)reader["ApplicationDate"];
                                ApplicationTypeID = (int)reader["ApplicationTypeID"];
                                ApplicationStatus = (byte)reader["ApplicationStatus"];
                                LastStatusDate = (DateTime)reader["LastStatusDate"];
                                PaidFees = Convert.ToSingle(reader["PaidFees"]);
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                isFound = false;
            }

            return isFound;
        }
        public static DataTable GetAllApplications()
        {


            DataTable dt = new DataTable();


            String query = "select * from ApplicationsList_View order by ApplicationDate desc;";
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
            catch (Exception ex) { throw;  }

            return dt;


        }

        public static int AddNewApplication(int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
        byte ApplicationStatus, DateTime LastStatusDate,
        float PaidFees, int CreatedByUserID)
        {
            int ID = -1;

            string query = @"INSERT INTO Applications ( 
                        ApplicantPersonID, ApplicationDate, ApplicationTypeID,
                        ApplicationStatus, LastStatusDate,
                        PaidFees, CreatedByUserID)
                     VALUES (@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID,
                             @ApplicationStatus, @LastStatusDate,
                             @PaidFees, @CreatedByUserID);
                     SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ApplicantPersonID", SqlDbType.Int).Value = ApplicantPersonID;
                    command.Parameters.Add("@ApplicationDate", SqlDbType.DateTime).Value = ApplicationDate;
                    command.Parameters.Add("@ApplicationTypeID", SqlDbType.Int).Value = ApplicationTypeID;
                    command.Parameters.Add("@ApplicationStatus", SqlDbType.TinyInt).Value = ApplicationStatus;
                    command.Parameters.Add("@LastStatusDate", SqlDbType.DateTime).Value = LastStatusDate;
                    command.Parameters.Add("@PaidFees", SqlDbType.Float).Value = PaidFees;
                    command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = CreatedByUserID;

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        ID = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return ID;
        }

        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
            byte ApplicationStatus, DateTime LastStatusDate,
            float PaidFees, int CreatedByUserID)
        {
            int rowsAffected = 0;


            string query = @"Update  Applications  
                            set ApplicantPersonID = @ApplicantPersonID,
                                ApplicationDate = @ApplicationDate,
                                ApplicationTypeID = @ApplicationTypeID,
                                ApplicationStatus = @ApplicationStatus, 
                                LastStatusDate = @LastStatusDate,
                                PaidFees = @PaidFees,
                                CreatedByUserID=@CreatedByUserID
                            where ApplicationID=@ApplicationID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = ApplicationID;
                        command.Parameters.Add("@ApplicantPersonID", SqlDbType.Int).Value = ApplicantPersonID;
                        command.Parameters.Add("@ApplicationDate", SqlDbType.DateTime).Value = ApplicationDate;
                        command.Parameters.Add("@ApplicationTypeID", SqlDbType.Int).Value = ApplicationTypeID;
                        command.Parameters.Add("@ApplicationStatus", SqlDbType.TinyInt).Value = ApplicationStatus;
                        command.Parameters.Add("@LastStatusDate", SqlDbType.DateTime).Value = LastStatusDate;
                        command.Parameters.Add("@PaidFees", SqlDbType.Float).Value = PaidFees;
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

        public static bool DeleteApplication(int ApplicationID)
        {
            int rowsAffected = 0;

            string query = "DELETE FROM Applications WHERE ApplicationID = @ApplicationID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = ApplicationID;

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

        public static bool IsApplicationExist(int ApplicationID)
        {
            bool isFound = false;

            string query = "SELECT 1 FROM Applications WHERE ApplicationID = @ApplicationID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = ApplicationID;

                    connection.Open();

                    object result = command.ExecuteScalar();

                    isFound = (result != null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return isFound;
        }

        public static int GetActiveApplicationID(int PersonID, int ApplicationTypeID)
        {
            int applicationID = -1;

            string query = @"SELECT TOP 1 ApplicationID 
                     FROM Applications
                     WHERE ApplicantPersonID = @ApplicantPersonID
                       AND ApplicationTypeID = @ApplicationTypeID
                       AND ApplicationStatus = 1";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ApplicantPersonID", SqlDbType.Int).Value = PersonID;
                    command.Parameters.Add("@ApplicationTypeID", SqlDbType.Int).Value = ApplicationTypeID;

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        applicationID = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return applicationID;
        }

        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {
            return GetActiveApplicationID(PersonID, ApplicationTypeID) != -1;

        }


        public static int GetActiveApplicationIDForLicenseClass(int PersonID, int ApplicationTypeID, int LicenseClassID)
        {
            int applicationID = -1;

            string query = @"SELECT TOP 1 Applications.ApplicationID  
                            FROM Applications 
                            INNER JOIN LocalDrivingLicenseApplications 
                            ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                            WHERE ApplicantPersonID = @ApplicantPersonID 
                            AND ApplicationTypeID = @ApplicationTypeID 
                            AND LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                            AND ApplicationStatus = 1
                            ORDER BY Applications.ApplicationDate DESC";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;
                    command.Parameters.Add("@ApplicationTypeID", SqlDbType.Int).Value = ApplicationTypeID;
                    command.Parameters.Add("@LicenseClassID", SqlDbType.Int).Value = LicenseClassID;

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        applicationID = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return applicationID;
        }


        public static bool UpdateStatus(int ApplicationID, short NewStatus)
        {
            int rowsAffected = 0;

            string query = @"UPDATE Applications
                     SET ApplicationStatus = @NewStatus,
                         LastStatusDate = @LastStatusDate
                     WHERE ApplicationID = @ApplicationID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = ApplicationID;
                    command.Parameters.Add("@NewStatus", SqlDbType.SmallInt).Value = NewStatus;
                    command.Parameters.Add("@LastStatusDate", SqlDbType.DateTime).Value = DateTime.Now;

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



    }
}

