using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicense_DAL
{
    public class LicenseClass
    {
        public static bool GetLicenseClassInfoByID(int LicenseClassID,
        ref string ClassName, ref string ClassDescription, ref byte MinimumAllowedAge,
        ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool isFound = false;

            string query = @"SELECT ClassName, ClassDescription, MinimumAllowedAge, 
                            DefaultValidityLength, ClassFees
                     FROM LicenseClasses
                     WHERE LicenseClassID = @LicenseClassID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@LicenseClassID", SqlDbType.Int).Value = LicenseClassID;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            ClassName = reader["ClassName"].ToString();
                            ClassDescription = reader["ClassDescription"].ToString();
                            MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                            DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                            ClassFees = Convert.ToSingle(reader["ClassFees"]);
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

        public static bool GetLicenseClassInfoByClassName(string ClassName, ref int LicenseClassID,
        ref string ClassDescription, ref byte MinimumAllowedAge,
        ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool isFound = false;

            string query = @"SELECT LicenseClassID, ClassDescription, MinimumAllowedAge,
                            DefaultValidityLength, ClassFees
                     FROM LicenseClasses
                     WHERE ClassName = @ClassName";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ClassName", SqlDbType.NVarChar).Value = ClassName;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            LicenseClassID = (int)reader["LicenseClassID"];
                            ClassDescription = reader["ClassDescription"].ToString();
                            MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                            DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                            ClassFees = Convert.ToSingle(reader["ClassFees"]);
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

        public static DataTable GetAllLicenseClasses()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT LicenseClassID, ClassName, ClassDescription,
                            MinimumAllowedAge, DefaultValidityLength, ClassFees
                     FROM LicenseClasses
                     ORDER BY ClassName";

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


        public static int AddNewLicenseClass(string ClassName, string ClassDescription,
         byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {
            int newID = -1;

            string query = @"INSERT INTO LicenseClasses
                     (ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees)
                     OUTPUT INSERTED.LicenseClassID
                     VALUES (@ClassName, @ClassDescription, @MinimumAllowedAge, @DefaultValidityLength, @ClassFees)";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@ClassName", SqlDbType.NVarChar, 50).Value = ClassName;
                    command.Parameters.Add("@ClassDescription", SqlDbType.NVarChar, 200).Value = ClassDescription;
                    command.Parameters.Add("@MinimumAllowedAge", SqlDbType.TinyInt).Value = MinimumAllowedAge;
                    command.Parameters.Add("@DefaultValidityLength", SqlDbType.TinyInt).Value = DefaultValidityLength;
                    command.Parameters.Add("@ClassFees", SqlDbType.Float).Value = ClassFees;

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


        public static bool UpdateLicenseClass(int LicenseClassID, string ClassName, string ClassDescription,
       byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {
            int rowsAffected = 0;

            string query = @"UPDATE LicenseClasses
                     SET ClassName = @ClassName,
                         ClassDescription = @ClassDescription,
                         MinimumAllowedAge = @MinimumAllowedAge,
                         DefaultValidityLength = @DefaultValidityLength,
                         ClassFees = @ClassFees
                     WHERE LicenseClassID = @LicenseClassID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@LicenseClassID", SqlDbType.Int).Value = LicenseClassID;
                    command.Parameters.Add("@ClassName", SqlDbType.NVarChar, 50).Value = ClassName;
                    command.Parameters.Add("@ClassDescription", SqlDbType.NVarChar, 200).Value = ClassDescription;
                    command.Parameters.Add("@MinimumAllowedAge", SqlDbType.TinyInt).Value = MinimumAllowedAge;
                    command.Parameters.Add("@DefaultValidityLength", SqlDbType.TinyInt).Value = DefaultValidityLength;
                    command.Parameters.Add("@ClassFees", SqlDbType.Float).Value = ClassFees;

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
