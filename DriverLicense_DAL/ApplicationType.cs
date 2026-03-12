using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicense_DAL
{
    public class ApplicationType
    {

        public static bool GetApplicationTypeInfoByID(int ID,
            ref string ApplicationTypeTitle, ref float ApplicationFees)
        {

            bool isFound = false;

            string qeury = "SELECT * FROM Country WHERE ApplicationTypeID = @ApplicationTypeID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand(qeury, connection))
                    {

                        command.Parameters.AddWithValue("@ID", ID);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                isFound = true;

                                ApplicationTypeTitle = reader["ApplicationTypeTitle"].ToString();

                                ApplicationFees = Convert.ToSingle(reader["ApplicationFees"]);
                            }
                            else
                            {
                                isFound = false;
                            }




                        }
                    }


                }

            }
            catch (Exception ex) { isFound = false; }

            return isFound;

        }


        public static DataTable GetAllApplicationTypes()
        {

            DataTable dt = new DataTable();
            string query = "SELECT * FROM ApplicationTypes order by ApplicationTypeTitle";

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
            catch (Exception ex) { }

            return dt;


        }


        public static int AddNewApplicationType(string Title, decimal Fees)
        {
            int AppID = -1;

            string query = @"Insert Into ApplicationTypes (ApplicationTypeTitle,ApplicationFees)
                            Values (@Title,@Fees)   

                            SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        command.Parameters.AddWithValue("@Title", Title);
                        command.Parameters.AddWithValue("@Fees", Fees);

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            AppID = insertedID;
                        }



                    }
                }



            }catch (Exception ex) { }
            

               return AppID;
            }



        public static bool UpdateApplicationType(int ApplicationTypeID, string Title, decimal Fees)
        {
            int rowsAffected = 0;


            string query = @"Update  ApplicationTypes  
                            set ApplicationTypeTitle = @Title,
                                ApplicationFees = @Fees
                                where ApplicationTypeID = @ApplicationTypeID";


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                {

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                        command.Parameters.AddWithValue("@Title", Title);
                        command.Parameters.AddWithValue("@Fees", Fees);




                        connection.Open();

                        rowsAffected = command.ExecuteNonQuery();


                    }



                }
            }catch(Exception ex)
            {
                return false;
            }

            return (rowsAffected > 0);
        }




    }  }
