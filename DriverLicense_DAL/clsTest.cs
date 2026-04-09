using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicense_DAL
{
    public class clsTest
    {

        public static bool GetTestInfoByID(int TestID,ref int TestAppointmentID, ref bool TestResult,ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = @"SELECT TestAppointmentID, TestResult, Notes, CreatedByUserID
                     FROM Tests
                     WHERE TestID = @TestID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@TestID", SqlDbType.Int).Value = TestID;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            TestAppointmentID = (int)reader["TestAppointmentID"];
                            TestResult = (bool)reader["TestResult"];
                            Notes = reader["Notes"]?.ToString();
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


        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass( int PersonID, int LicenseClassID, int TestTypeID,ref int TestID, ref int TestAppointmentID,
        ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = @"SELECT TOP 1 
                    T.TestID,
                    T.TestAppointmentID,
                    T.TestResult,
                    T.Notes,
                    T.CreatedByUserID,
                    A.ApplicantPersonID
                FROM Tests T
                INNER JOIN TestAppointments TA 
                    ON T.TestAppointmentID = TA.TestAppointmentID
                INNER JOIN LocalDrivingLicenseApplications L 
                    ON TA.LocalDrivingLicenseApplicationID = L.LocalDrivingLicenseApplicationID
                INNER JOIN Applications A 
                    ON L.ApplicationID = A.ApplicationID
                WHERE A.ApplicantPersonID = @PersonID
                  AND L.LicenseClassID = @LicenseClassID
                  AND TA.TestTypeID = @TestTypeID
                ORDER BY TA.TestAppointmentID DESC";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;
                    command.Parameters.Add("@LicenseClassID", SqlDbType.Int).Value = LicenseClassID;
                    command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = TestTypeID;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            TestID = (int)reader["TestID"];
                            TestAppointmentID = (int)reader["TestAppointmentID"];
                            TestResult = (bool)reader["TestResult"];
                            Notes = reader["Notes"]?.ToString();
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


        public static DataTable GetAllTests()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID
                     FROM Tests ORDER BY TestID ";

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


        public static int AddNewTest(int TestAppointmentID, bool TestResult,string Notes, int CreatedByUserID)
        {
            int newID = -1;

            string query = @"INSERT INTO Tests
                     (TestAppointmentID, TestResult, Notes, CreatedByUserID)
                     OUTPUT INSERTED.TestID
                     VALUES (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID)";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = TestAppointmentID;
                    command.Parameters.Add("@TestResult", SqlDbType.Bit).Value = TestResult;
                    command.Parameters.Add("@Notes", SqlDbType.NVarChar, 200).Value = (object)Notes ?? DBNull.Value;
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


        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE Tests
                     SET TestAppointmentID = @TestAppointmentID,
                         TestResult = @TestResult,
                         Notes = @Notes,
                         CreatedByUserID = @CreatedByUserID
                     WHERE TestID = @TestID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@TestID", SqlDbType.Int).Value = TestID;
                    command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = TestAppointmentID;
                    command.Parameters.Add("@TestResult", SqlDbType.Bit).Value = TestResult;
                    command.Parameters.Add("@Notes", SqlDbType.NVarChar, 200).Value = (object)Notes ?? DBNull.Value;
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

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            byte count = 0;

            string query = @"SELECT COUNT(T.TestID)
                     FROM Tests T
                     INNER JOIN TestAppointments TA 
                         ON T.TestAppointmentID = TA.TestAppointmentID
                     WHERE TA.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                       AND T.TestResult = 1";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int)
                                      .Value = LocalDrivingLicenseApplicationID;

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                        count = Convert.ToByte(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return count;
        }


        //public static bool HasPassedAllTests(int LocalDrivingLicenseApplicationID)
        //{
        //    int passedTests = 0;

        //    string query = @"SELECT COUNT(DISTINCT TA.TestTypeID)
        //             FROM Tests T
        //             INNER JOIN TestAppointments TA 
        //                 ON T.TestAppointmentID = TA.TestAppointmentID
        //             WHERE TA.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
        //               AND T.TestResult = 1";

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int)
        //                              .Value = LocalDrivingLicenseApplicationID;

        //            connection.Open();

        //            object result = command.ExecuteScalar();

        //            if (result != null)
        //                passedTests = Convert.ToInt32(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return false;
        //    }

        //    int requiredTests = 3;

        //    return passedTests >= requiredTests;
        //}

    }
}
