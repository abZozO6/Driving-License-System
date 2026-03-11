using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace DriverLicense_DAL
{
    public class clsPersonData
    {

        private static object GetNullableValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return DBNull.Value;

            return value;
        }
        public static clsPerson GetPersonInfoByID(int PersonID)
        {
            clsPerson person = null;

            string query = "SELECT * FROM People WHERE PersonID = @PersonID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                person = new clsPerson();

                                person.PersonID = PersonID;
                                person.FirstName = (string)reader["FirstName"];
                                person.SecondName = (string)reader["SecondName"];
                                person.ThirdName = reader["ThirdName"] != DBNull.Value ? (string)reader["ThirdName"] : "";
                                person.LastName = (string)reader["LastName"];
                                person.NationalNo = (string)reader["NationalNo"];
                                person.DateOfBirth = (DateTime)reader["DateOfBirth"];
                                person.Gendor = (byte)reader["Gendor"];
                                person.Address = (string)reader["Address"];
                                person.Phone = (string)reader["Phone"];
                                person.Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : "";
                                person.NationalityCountryID = (int)reader["NationalityCountryID"];
                                person.ImagePath = reader["ImagePath"] != DBNull.Value ? (string)reader["ImagePath"] : "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                person = null;
          
            }

            return person;
        }
        public static clsPerson GetPersonInfoByNationalNo(string NationalNo)
        {
            clsPerson person = null;

            string query = "SELECT * FROM People WHERE NationalNo = @NationalNo";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NationalNo", NationalNo);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                person = new clsPerson();

                                person.PersonID = (int)reader["PersonID"];
                                person.FirstName = (string)reader["FirstName"];
                                person.SecondName = (string)reader["SecondName"];
                                person.ThirdName = reader["ThirdName"] != DBNull.Value ? (string)reader["ThirdName"] : "";
                                person.LastName = (string)reader["LastName"];
                                person.DateOfBirth = (DateTime)reader["DateOfBirth"];
                                person.Gendor = Convert.ToInt16(reader["Gendor"]);
                                person.Address = (string)reader["Address"];
                                person.Phone = (string)reader["Phone"];
                                person.Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : "";
                                person.NationalityCountryID = (int)reader["NationalityCountryID"];
                                person.ImagePath = reader["ImagePath"] != DBNull.Value ? (string)reader["ImagePath"] : "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                person = null;
            }

            return person;
        }
        public static int AddNewPerson(clsPerson Person)
        {
            int PersonID = -1;

            string query = @"INSERT INTO People (FirstName, SecondName, ThirdName,LastName,NationalNo,
                                         DateOfBirth,Gendor,Address,Phone, Email, NationalityCountryID,ImagePath)
                     VALUES (@FirstName, @SecondName,@ThirdName, @LastName, @NationalNo,
                             @DateOfBirth,@Gendor,@Address,@Phone, @Email,@NationalityCountryID,@ImagePath);
                     SELECT SCOPE_IDENTITY();";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", Person.FirstName);
                        command.Parameters.AddWithValue("@SecondName", Person.SecondName);

                        command.Parameters.AddWithValue("@ThirdName", GetNullableValue(Person.ThirdName));

                        command.Parameters.AddWithValue("@LastName", Person.LastName);
                        command.Parameters.AddWithValue("@NationalNo", Person.NationalNo);
                        command.Parameters.AddWithValue("@DateOfBirth", Person.DateOfBirth);
                        command.Parameters.AddWithValue("@Gendor", Person.Gendor);
                        command.Parameters.AddWithValue("@Address", Person.Address);
                        command.Parameters.AddWithValue("@Phone", Person.Phone);

                        command.Parameters.AddWithValue("@Email", GetNullableValue(Person.Email));


                        command.Parameters.AddWithValue("@NationalityCountryID", Person.NationalityCountryID);

                        command.Parameters.AddWithValue("@ImagePath", GetNullableValue(Person.ImagePath));

                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            PersonID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            return PersonID;
        }
        public static bool UpdatePerson(clsPerson Person)
        {
            int rowsAffected = 0;

            string query = @"Update People  
                    set FirstName = @FirstName,
                        SecondName = @SecondName,
                        ThirdName = @ThirdName,
                        LastName = @LastName, 
                        NationalNo = @NationalNo,
                        DateOfBirth = @DateOfBirth,
                        Gendor = @Gendor,
                        Address = @Address,  
                        Phone = @Phone,
                        Email = @Email, 
                        NationalityCountryID = @NationalityCountryID,
                        ImagePath = @ImagePath
                    where PersonID = @PersonID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", Person.PersonID);
                    command.Parameters.AddWithValue("@FirstName", Person.FirstName);
                    command.Parameters.AddWithValue("@SecondName", Person.SecondName);

                    command.Parameters.AddWithValue("@ThirdName",
                        string.IsNullOrEmpty(Person.ThirdName) ? DBNull.Value : (object)Person.ThirdName);

                    command.Parameters.AddWithValue("@LastName", Person.LastName);
                    command.Parameters.AddWithValue("@NationalNo", Person.NationalNo);
                    command.Parameters.AddWithValue("@DateOfBirth", Person.DateOfBirth);
                    command.Parameters.AddWithValue("@Gendor", Person.Gendor);
                    command.Parameters.AddWithValue("@Address", Person.Address);
                    command.Parameters.AddWithValue("@Phone", Person.Phone);

                    command.Parameters.AddWithValue("@Email",
                        string.IsNullOrEmpty(Person.Email) ? DBNull.Value : (object)Person.Email);

                    command.Parameters.AddWithValue("@NationalityCountryID", Person.NationalityCountryID);

                    command.Parameters.AddWithValue("@ImagePath",
                        string.IsNullOrEmpty(Person.ImagePath) ? DBNull.Value : (object)Person.ImagePath);

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch
            {
                return false;
            }

            return rowsAffected > 0;
        }
        public static bool DeletePerson(int PersonID)
        {
            int rowsAffected = 0;

            string query = @"Delete People where PersonID = @PersonID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch
            {
            }

            return (rowsAffected > 0);
        }
        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT People.PersonID, People.NationalNo,
             People.FirstName, People.SecondName, People.ThirdName, People.LastName,
                People.DateOfBirth, People.Gendor,
        CASE
        WHEN People.Gendor = 0 THEN 'Male'
        ELSE 'Female'
         END as GendorCaption,
         People.Address, People.Phone, People.Email,
           People.NationalityCountryID, Countries.CountryName, People.ImagePath
           FROM People
           INNER JOIN Countries ON People.NationalityCountryID = Countries.CountryID
           ORDER BY People.FirstName";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
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
            catch
            {
            }

            return dt;
        }
        public static bool IsPersonExist(int PersonID)
        {
            bool isFound = false;

            string query = "SELECT Found=1 FROM People WHERE PersonID = @PersonID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        isFound = reader.HasRows;
                    }
                }
            }
            catch
            {
                isFound = false;
            }

            return isFound;
        }
        public static bool IsPersonExist(string NationalNo)
        {
            bool isFound = false;

            string query = "SELECT Found=1 FROM People WHERE NationalNo = @NationalNo";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDALsettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        isFound = reader.HasRows;
                    }
                }
            }
            catch
            {
                isFound = false;
            }

            return isFound;
        }


    }
}
