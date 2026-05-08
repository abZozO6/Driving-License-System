using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicense_DAL
{
    public class clsLocalDrivingLicenseApplicationData
    {

        public static bool GetLocalDrivingLicenseApplicationInfoByID(
             int LocalDrivingLicenseApplicationID, ref int ApplicationID,
             ref int LicenseClassID)
          {
            bool isFound = false;

            string query = @"SELECT *
                     FROM LocalDrivingLicenseApplications 
                     WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int)
                                      .Value = LocalDrivingLicenseApplicationID;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            ApplicationID = (int)reader["ApplicationID"];
                            LicenseClassID = (int)reader["LicenseClassID"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }

            return isFound;
        }

        public static bool GetLocalDrivingLicenseApplicationInfoByApplicationID(
            int ApplicationID, ref int LocalDrivingLicenseApplicationID,
            ref int LicenseClassID)
        {
            bool isFound = false;

            string query = @"SELECT *
                     FROM LocalDrivingLicenseApplications
                     WHERE ApplicationID = @ApplicationID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = ApplicationID;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                            LicenseClassID = (int)reader["LicenseClassID"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                isFound = false;
            }

            return isFound;
        }

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT *
                     FROM LocalDrivingLicenseApplications
                     ORDER BY LocalDrivingLicenseApplicationID DESC";

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

        public static int AddNewLocalDrivingLicenseApplication(
    int ApplicationID, int LicenseClassID)
        {
            int newID = -1;

            string query = @"INSERT INTO LocalDrivingLicenseApplications
                     (ApplicationID, LicenseClassID)
                     VALUES (@ApplicationID, @LicenseClassID);
                     SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = ApplicationID;
                    command.Parameters.Add("@LicenseClassID", SqlDbType.Int).Value = LicenseClassID;

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        newID = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return newID;
        }

        public static bool UpdateLocalDrivingLicenseApplication(
    int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE LocalDrivingLicenseApplications
                     SET ApplicationID = @ApplicationID,
                         LicenseClassID = @LicenseClassID
                     WHERE LocalDrivingLicenseApplicationID = @ID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = LocalDrivingLicenseApplicationID;
                    command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = ApplicationID;
                    command.Parameters.Add("@LicenseClassID", SqlDbType.Int).Value = LicenseClassID;

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

        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            int rowsAffected = 0;

            string query = @"DELETE FROM LocalDrivingLicenseApplications
                     WHERE LocalDrivingLicenseApplicationID = @ID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = LocalDrivingLicenseApplicationID;

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

        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool Result = false;

            string query = @"SELECT TOP 1 TestResult
                     FROM LocalDrivingLicenseApplications 
                     INNER JOIN TestAppointments 
                         ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
                     INNER JOIN Tests 
                         ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                     WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                       AND TestAppointments.TestTypeID = @TestTypeID
                     ORDER BY TestAppointments.TestAppointmentID DESC";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int)
                                      .Value = LocalDrivingLicenseApplicationID;

                    command.Parameters.Add("@TestTypeID", SqlDbType.Int)
                                      .Value = TestTypeID;

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        Result = Convert.ToBoolean(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return Result;
        }


        public static bool DoesAttendTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            string query = @"SELECT TOP 1 1
                            FROM TestAppointments 
                            INNER JOIN Tests 
                            ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE TestAppointments.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                            AND TestAppointments.TestTypeID = @TestTypeID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int)
                                      .Value = LocalDrivingLicenseApplicationID;

                    command.Parameters.Add("@TestTypeID", SqlDbType.Int)
                                      .Value = TestTypeID;

                    connection.Open();

                    object result = command.ExecuteScalar();

                    return (result != null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public static byte TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            byte totalTrials = 0;

            string query = @"SELECT COUNT(T.TestID)
                     FROM TestAppointments TA
                     INNER JOIN Tests T
                         ON TA.TestAppointmentID = T.TestAppointmentID
                     WHERE TA.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                       AND TA.TestTypeID = @TestTypeID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int)
                                      .Value = LocalDrivingLicenseApplicationID;

                    command.Parameters.Add("@TestTypeID", SqlDbType.Int)
                                      .Value = TestTypeID;

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        totalTrials = Convert.ToByte(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return totalTrials;
        }



        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            string query = @"SELECT TOP 1 1 
                 FROM TestAppointments 
                 WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                   AND TestTypeID = @TestTypeID 
                   AND IsLocked = 0";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int)
                                      .Value = LocalDrivingLicenseApplicationID;

                    command.Parameters.Add("@TestTypeID", SqlDbType.Int)
                                      .Value = TestTypeID;

                    connection.Open();

                    object result = command.ExecuteScalar();

                    return (result != null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }



    }
}
