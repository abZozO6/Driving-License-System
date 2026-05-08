using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicense_DAL
{
    public class clsLicense

    {
        public static bool GetLicenseInfoByID(int LicenseID, ref int ApplicationID, ref int DriverID, ref int LicenseClass,
        ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes, ref float PaidFees, ref bool IsActive, ref byte IssueReason, ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = @"SELECT ApplicationID, DriverID, LicenseClass,
                            IssueDate, ExpirationDate, Notes,
                            PaidFees, IsActive, IssueReason, CreatedByUserID
                     FROM Licenses
                     WHERE LicenseID = @LicenseID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = LicenseID;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            ApplicationID = (int)reader["ApplicationID"];
                            DriverID = (int)reader["DriverID"];
                            LicenseClass = (int)reader["LicenseClass"];
                            IssueDate = (DateTime)reader["IssueDate"];
                            ExpirationDate = (DateTime)reader["ExpirationDate"];
                            Notes = reader["Notes"]?.ToString();
                            PaidFees = Convert.ToSingle(reader["PaidFees"]);
                            IsActive = (bool)reader["IsActive"];
                            IssueReason = (byte)reader["IssueReason"];
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

        public static DataTable GetAllLicenses()
        {
            DataTable dt = new DataTable();

            string query = "SELECT * FROM Licenses";

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


        public static DataTable GetDriverLicenses(int DriverID)
        {
            DataTable dt = new DataTable();

            string query = @"SELECT 
                    L.LicenseID,
                    L.ApplicationID,
                    LC.ClassName,
                    L.IssueDate,
                    L.ExpirationDate,
                    L.IsActive
                FROM Licenses L
                INNER JOIN LicenseClasses LC 
                    ON L.LicenseClass = LC.LicenseClassID
                WHERE L.DriverID = @DriverID
                ORDER BY L.IsActive DESC, L.ExpirationDate DESC";

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


        public static int AddNewLicense(int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate, DateTime ExpirationDate, string Notes,
        float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int newID = -1;

            string query = @"INSERT INTO Licenses
                     (ApplicationID, DriverID, LicenseClass,
                      IssueDate, ExpirationDate, Notes,
                      PaidFees, IsActive, IssueReason, CreatedByUserID)
                     OUTPUT INSERTED.LicenseID
                     VALUES (@ApplicationID, @DriverID, @LicenseClass,
                             @IssueDate, @ExpirationDate, @Notes,
                             @PaidFees, @IsActive, @IssueReason, @CreatedByUserID)";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = ApplicationID;
                    command.Parameters.Add("@DriverID", SqlDbType.Int).Value = DriverID;
                    command.Parameters.Add("@LicenseClass", SqlDbType.Int).Value = LicenseClass;
                    command.Parameters.Add("@IssueDate", SqlDbType.DateTime).Value = IssueDate;
                    command.Parameters.Add("@ExpirationDate", SqlDbType.DateTime).Value = ExpirationDate;
                    command.Parameters.Add("@Notes", SqlDbType.NVarChar, 200).Value = (object)Notes ?? DBNull.Value;
                    command.Parameters.Add("@PaidFees", SqlDbType.Float).Value = PaidFees;
                    command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = IsActive;
                    command.Parameters.Add("@IssueReason", SqlDbType.TinyInt).Value = IssueReason;
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
        public static bool UpdateLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate, DateTime ExpirationDate, string Notes,
        float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE Licenses
                     SET ApplicationID = @ApplicationID,
                         DriverID = @DriverID,
                         LicenseClass = @LicenseClass,
                         IssueDate = @IssueDate,
                         ExpirationDate = @ExpirationDate,
                         Notes = @Notes,
                         PaidFees = @PaidFees,
                         IsActive = @IsActive,
                         IssueReason = @IssueReason,
                         CreatedByUserID = @CreatedByUserID
                     WHERE LicenseID = @LicenseID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = LicenseID;
                    command.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = ApplicationID;
                    command.Parameters.Add("@DriverID", SqlDbType.Int).Value = DriverID;
                    command.Parameters.Add("@LicenseClass", SqlDbType.Int).Value = LicenseClass;
                    command.Parameters.Add("@IssueDate", SqlDbType.DateTime).Value = IssueDate;
                    command.Parameters.Add("@ExpirationDate", SqlDbType.DateTime).Value = ExpirationDate;
                    command.Parameters.Add("@Notes", SqlDbType.NVarChar, 200).Value = (object)Notes ?? DBNull.Value;
                    command.Parameters.Add("@PaidFees", SqlDbType.Float).Value = PaidFees;
                    command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = IsActive;
                    command.Parameters.Add("@IssueReason", SqlDbType.TinyInt).Value = IssueReason;
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

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;

            string query = @"SELECT TOP 1 L.LicenseID
                 FROM Licenses L
                 INNER JOIN Drivers D 
                     ON L.DriverID = D.DriverID
                 WHERE L.LicenseClass = @LicenseClass 
                   AND D.PersonID = @PersonID
                   AND L.IsActive = 1";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;
                    command.Parameters.Add("@LicenseClassID", SqlDbType.Int).Value = LicenseClassID;

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

        public static bool DeactivateLicense(int LicenseID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE Licenses
                     SET IsActive = 0
                     WHERE LicenseID = @LicenseID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = LicenseID;

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
