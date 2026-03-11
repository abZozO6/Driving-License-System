using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicense_DAL
{
    public class clsCountryData
    {
        public enum enGendor { Male = 0, Female = 1 };

        public static bool GetCountryInfoByID(int ID, ref string CountryName)
        {
            bool isFound = false;

            string query = "SELECT * FROM Country WHERE CountryID = @CountryID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CountryID", ID);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                isFound = true;

                                CountryName = reader["CountryName"].ToString();
                            }





                        }
                    }
                }
            }
            catch
            (Exception ex)
            {
                isFound = false;
            }

            return isFound;
        }


        public static bool GetCountryInfoByName(string CountryName, ref int CountryID)
        {
            bool isFound = false;

            string query = "SELECT CountryID FROM Country WHERE CountryName = @CountryName;" ;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CountryName", CountryName);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;

                                CountryID = (int)reader["CountryID"];
                            }
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

        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();

            string query = "SELECT CountryID, CountryName FROM Country";

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

    }
}