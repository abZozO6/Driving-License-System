using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicense_DAL
{
    public class clsTestAppointmentData
    {

        public static bool GetTestAppointmentInfoByID(int TestAppointmentID, ref int TestTypeID, ref int LocalDrivingLicenseApplicationID,
         ref DateTime AppointmentDate, ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked,ref int RetakeTestApplicationID)
        {
            bool isFound = false;

            string query = @"SELECT TestTypeID, LocalDrivingLicenseApplicationID,
                            AppointmentDate, PaidFees, CreatedByUserID,
                            IsLocked, RetakeTestApplicationID
                     FROM TestAppointments
                     WHERE TestAppointmentID = @TestAppointmentID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = TestAppointmentID;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            TestTypeID = (int)reader["TestTypeID"];
                            LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                            AppointmentDate = (DateTime)reader["AppointmentDate"];
                            PaidFees = Convert.ToSingle(reader["PaidFees"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            IsLocked = (bool)reader["IsLocked"];

                            if (reader["RetakeTestApplicationID"] != DBNull.Value)
                                RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                            else
                                RetakeTestApplicationID = -1;
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


        public static bool GetLastTestAppointment( int LocalDrivingLicenseApplicationID, int TestTypeID, ref int TestAppointmentID, ref DateTime AppointmentDate,
        ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool isFound = false;

            string query = @"SELECT TOP 1 TestAppointmentID, AppointmentDate,
                            PaidFees, CreatedByUserID,
                            IsLocked, RetakeTestApplicationID
                     FROM TestAppointments
                     WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                       AND TestTypeID = @TestTypeID
                     ORDER BY TestAppointmentID DESC";

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

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            TestAppointmentID = (int)reader["TestAppointmentID"];
                            AppointmentDate = (DateTime)reader["AppointmentDate"];
                            PaidFees = Convert.ToSingle(reader["PaidFees"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            IsLocked = (bool)reader["IsLocked"];

                            if (reader["RetakeTestApplicationID"] != DBNull.Value)
                                RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                            else
                                RetakeTestApplicationID = -1;
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

        public static DataTable GetAllTestAppointments()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT TestAppointmentID, TestTypeID, 
                            LocalDrivingLicenseApplicationID,
                            AppointmentDate, PaidFees,
                            CreatedByUserID, IsLocked,
                            RetakeTestApplicationID
                     FROM TestAppointments
                     ORDER BY AppointmentDate DESC";

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

        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            DataTable dt = new DataTable();

            string query = @"SELECT TestAppointmentID, AppointmentDate,PaidFees, IsLocked
                        FROM TestAppointments
                        WHERE  
                        (TestTypeID = @TestTypeID) 
                        AND (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)
                        order by AppointmentDate  desc;";

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


        public static int AddNewTestAppointment( int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, float PaidFees,
        int CreatedByUserID, bool IsLocked, int? RetakeTestApplicationID)
        {
            int newID = -1;

            string query = @"INSERT INTO TestAppointments
                     (TestTypeID, LocalDrivingLicenseApplicationID,
                      AppointmentDate, PaidFees,
                      CreatedByUserID, IsLocked, RetakeTestApplicationID)
                     OUTPUT INSERTED.TestAppointmentID
                     VALUES (@TestTypeID, @LocalDrivingLicenseApplicationID,
                             @AppointmentDate, @PaidFees,
                             @CreatedByUserID, @IsLocked, @RetakeTestApplicationID)";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = TestTypeID;
                    command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int).Value = LocalDrivingLicenseApplicationID;
                    command.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = AppointmentDate;
                    command.Parameters.Add("@PaidFees", SqlDbType.Float).Value = PaidFees;
                    command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = CreatedByUserID;
                    command.Parameters.Add("@IsLocked", SqlDbType.Bit).Value = IsLocked;

                    if (RetakeTestApplicationID.HasValue)
                        command.Parameters.Add("@RetakeTestApplicationID", SqlDbType.Int).Value = RetakeTestApplicationID.Value;
                    else
                        command.Parameters.Add("@RetakeTestApplicationID", SqlDbType.Int).Value = DBNull.Value;

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


        public static bool UpdateTestAppointment(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID,DateTime AppointmentDate, float PaidFees,
        int CreatedByUserID, bool IsLocked, int? RetakeTestApplicationID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE TestAppointments
                     SET TestTypeID = @TestTypeID,
                         LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID,
                         AppointmentDate = @AppointmentDate,
                         PaidFees = @PaidFees,
                         CreatedByUserID = @CreatedByUserID,
                         IsLocked = @IsLocked,
                         RetakeTestApplicationID = @RetakeTestApplicationID
                     WHERE TestAppointmentID = @TestAppointmentID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = TestAppointmentID;
                    command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = TestTypeID;
                    command.Parameters.Add("@LocalDrivingLicenseApplicationID", SqlDbType.Int).Value = LocalDrivingLicenseApplicationID;
                    command.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = AppointmentDate;
                    command.Parameters.Add("@PaidFees", SqlDbType.Float).Value = PaidFees;
                    command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = CreatedByUserID;
                    command.Parameters.Add("@IsLocked", SqlDbType.Bit).Value = IsLocked;

                    if (RetakeTestApplicationID.HasValue)
                        command.Parameters.Add("@RetakeTestApplicationID", SqlDbType.Int).Value = RetakeTestApplicationID.Value;
                    else
                        command.Parameters.Add("@RetakeTestApplicationID", SqlDbType.Int).Value = DBNull.Value;

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


        public static int GetTestID(int TestAppointmentID)
        {
            int TestID = -1;

            string query = @"SELECT TestID
                     FROM Tests
                     WHERE TestAppointmentID = @TestAppointmentID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@TestAppointmentID", SqlDbType.Int)
                                      .Value = TestAppointmentID;

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        TestID = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return TestID;
        }






    }
}
