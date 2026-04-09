using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicense_DAL
{
    public class clsDetainedLicenseData
    {
        public static bool GetDetainedLicenseInfoByID(int DetainID, ref int LicenseID, ref DateTime DetainDate,ref float FineFees, 
        ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate, ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool isFound = false;

            string query = @"SELECT LicenseID, DetainDate, FineFees, CreatedByUserID,
                            IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID
                     FROM DetainedLicenses
                     WHERE DetainID = @DetainID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@DetainID", SqlDbType.Int).Value = DetainID;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            LicenseID = (int)reader["LicenseID"];
                            DetainDate = (DateTime)reader["DetainDate"];
                            FineFees = Convert.ToSingle(reader["FineFees"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            IsReleased = (bool)reader["IsReleased"];

                            ReleaseDate = reader["ReleaseDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["ReleaseDate"];
                            ReleasedByUserID = reader["ReleasedByUserID"] == DBNull.Value ? -1 : (int)reader["ReleasedByUserID"];
                            ReleaseApplicationID = reader["ReleaseApplicationID"] == DBNull.Value ? -1 : (int)reader["ReleaseApplicationID"];
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


        public static bool GetDetainedLicenseInfoByLicenseID(int LicenseID, ref int DetainID, ref DateTime DetainDate,
        ref float FineFees, ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate,  ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool isFound = false;

            string query = @"SELECT TOP 1 DetainID, DetainDate, FineFees, CreatedByUserID,
                            IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID
                     FROM DetainedLicenses
                     WHERE LicenseID = @LicenseID
                     ORDER BY DetainID DESC";

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

                            DetainID = (int)reader["DetainID"];
                            DetainDate = (DateTime)reader["DetainDate"];
                            FineFees = Convert.ToSingle(reader["FineFees"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            IsReleased = (bool)reader["IsReleased"];

                            ReleaseDate = reader["ReleaseDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["ReleaseDate"];
                            ReleasedByUserID = reader["ReleasedByUserID"] == DBNull.Value ? -1 : (int)reader["ReleasedByUserID"];
                            ReleaseApplicationID = reader["ReleaseApplicationID"] == DBNull.Value ? -1 : (int)reader["ReleaseApplicationID"];
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


        public static DataTable GetAllDetainedLicenses()
        {
            DataTable dt = new DataTable();

            string query = "select * from detainedLicenses_View order by IsReleased ,DetainID;";


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


        public static int AddNewDetainedLicense(int LicenseID, DateTime DetainDate, float FineFees, int CreatedByUserID)
        {
            int newID = -1;

            string query = @"INSERT INTO DetainedLicenses
                     (LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased)
                     OUTPUT INSERTED.DetainID
                     VALUES (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, 0)";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = LicenseID;
                    command.Parameters.Add("@DetainDate", SqlDbType.DateTime).Value = DetainDate;
                    command.Parameters.Add("@FineFees", SqlDbType.Float).Value = FineFees;
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


        public static bool UpdateDetainedLicense(int DetainID,int LicenseID, DateTime DetainDate,float FineFees, int CreatedByUserID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE DetainedLicenses
                     SET LicenseID = @LicenseID,
                         DetainDate = @DetainDate,
                         FineFees = @FineFees,
                         CreatedByUserID = @CreatedByUserID
                     WHERE DetainID = @DetainID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@DetainID", SqlDbType.Int).Value = DetainID;
                    command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = LicenseID;
                    command.Parameters.Add("@DetainDate", SqlDbType.DateTime).Value = DetainDate;
                    command.Parameters.Add("@FineFees", SqlDbType.Float).Value = FineFees;
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



        public static bool ReleaseDetainedLicense(int DetainID, int ReleasedByUserID, int ReleaseApplicationID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE DetainedLicenses
                     SET IsReleased = 1,
                         ReleaseDate = @ReleaseDate,
                         ReleasedByUserID = @ReleasedByUserID,
                         ReleaseApplicationID = @ReleaseApplicationID
                        WHERE DetainID = @DetainID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@DetainID", SqlDbType.Int).Value = DetainID;
                    command.Parameters.Add("@ReleasedByUserID", SqlDbType.Int).Value = ReleasedByUserID;
                    command.Parameters.Add("@ReleaseApplicationID", SqlDbType.Int).Value = ReleaseApplicationID;

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



        public static bool IsLicenseDetained(int LicenseID)
        {

            string query = @"select IsDetained=1 
                            from detainedLicenses 
                            where 
                            LicenseID=@LicenseID 
                            and IsReleased=0;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@LicenseID", SqlDbType.Int).Value = LicenseID;

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
