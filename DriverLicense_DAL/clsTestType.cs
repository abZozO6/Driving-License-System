using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicense_DAL
{
    public class clsTestType
    {
        public static bool GetTestTypeInfoByID(int TestTypeID,
         ref string TestTypeTitle, ref string TestDescription, ref float TestFees)
        {
            bool isFound = false;

            string query = @"SELECT TestTypeTitle, TestDescription, TestFees
                     FROM TestTypes
                     WHERE TestTypeID = @TestTypeID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = TestTypeID;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            TestTypeTitle = reader["TestTypeTitle"].ToString();
                            TestDescription = reader["TestDescription"].ToString();
                            TestFees = Convert.ToSingle(reader["TestFees"]);
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


        public static DataTable GetAllTestTypes()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT TestTypeID, TestTypeTitle, TestDescription, TestFees
                     FROM TestTypes
                     ORDER BY TestTypeTitle";

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


        public static int AddNewTestType(string Title, string Description, float Fees)
        {
            int newID = -1;

            string query = @"INSERT INTO TestTypes 
                (TestTypeTitle, TestDescription, TestFees)
                OUTPUT INSERTED.TestTypeID 
                VALUES (@TestTypeTitle, @TestTypeDescription, @TestTypeFees);"; // use  OUTPUT INSERTED.TestTypeID  to save id without any error

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Title", SqlDbType.NVarChar, 50).Value = Title;
                    command.Parameters.Add("@Description", SqlDbType.NVarChar, 200).Value = Description;
                    command.Parameters.Add("@Fees", SqlDbType.Float).Value = Fees;

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

        public static bool UpdateTestType(int TestTypeID, string Title, string Description, float Fees)
        {
            int rowsAffected = 0;

            string query = @"UPDATE TestTypes
                     SET TestTypeTitle = @Title,
                         TestDescription = @Description,
                         TestFees = @Fees
                     WHERE TestTypeID = @TestTypeID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@TestTypeID", SqlDbType.Int).Value = TestTypeID;
                    command.Parameters.Add("@Title", SqlDbType.NVarChar, 50).Value = Title;
                    command.Parameters.Add("@Description", SqlDbType.NVarChar, 200).Value = Description;
                    command.Parameters.Add("@Fees", SqlDbType.Float).Value = Fees;

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
