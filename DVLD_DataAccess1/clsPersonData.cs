using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Security.Policy;

namespace DVLD_DataAccess
{
    public class clsPersonData
    {
        public static bool GetPersonInfoByID(int PersonID, ref string FirstName, ref string SecondName, ref string ThirdName,
                  ref string LastName, ref string NationalNo, ref DateTime DateOfBirth, ref short Gendor, ref string Address,
                  ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from People where PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsFound = true;

                    FirstName = (string)Reader["FirstName"];
                    SecondName = (string)Reader["SecondName"];

                    if (Reader["ThirdName"] != DBNull.Value)
                        ThirdName = (string)Reader["ThirdName"];
                    else
                        ThirdName = "";

                    LastName = (string)Reader["LastName"];
                    NationalNo = (string)Reader["NationalNo"];
                    DateOfBirth = (DateTime)Reader["DateOfBirth"];
                    Gendor = Convert.ToInt16(Reader["Gendor"]);
                    Address = (string)Reader["Address"];
                    Phone = (string)Reader["Phone"];

                    if (Reader["Email"] != DBNull.Value)
                        Email = (string)Reader["Email"];
                    else
                        Email = "";

                    NationalityCountryID = (int)Reader["NationalityCountryID"];

                    if (Reader["ImagePath"] != DBNull.Value)
                        ImagePath = (string)Reader["ImagePath"];
                    else
                        ImagePath = "";
                }
                else
                {
                    IsFound = false;
                }
                Reader.Close();
            }
            catch (Exception ex)
            {
                IsFound = false;
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }

        public static bool GetPersonInfoByNationalNo(string NationalNo, ref int PersonID, ref string FirstName, 
            ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, ref short Gendor,
            ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from People where NationalNo = @NationalNo";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsFound = true;

                    PersonID = (int)Reader["PersonID"];
                    FirstName = (string)Reader["FirstName"];
                    SecondName = (string)Reader["SecondName"];

                    if (Reader["ThirdName"] != DBNull.Value)
                        ThirdName = (string)Reader["ThirdName"];
                    else
                        ThirdName = "";

                    LastName = (string)Reader["LastName"];
                    NationalNo = (string)Reader["NationalNo"];
                    DateOfBirth = (DateTime)Reader["DateOfBirth"];
                    Gendor = Convert.ToInt16(Reader["Gendor"]);
                    Address = (string)Reader["Address"];
                    Phone = (string)Reader["Phone"];

                    if (Reader["Email"] != DBNull.Value)
                        Email = (string)Reader["Email"];
                    else
                        Email = "";

                    NationalityCountryID = (int)Reader["NationalityCountryID"];

                    if (Reader["ImagePath"] != DBNull.Value)
                        ImagePath = (string)Reader["ImagePath"];
                    else
                        ImagePath = "";
                }
                else
                {
                    IsFound = false;
                }
                Reader.Close();
            }
            catch (Exception ex)
            {
                IsFound = false;
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }

        public static int AddNewPerson(string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName,
            DateTime DateOfBirth, short Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            int PersonID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"insert into People (NationalNo,FirstName,SecondName,ThirdName,LastName,DateOfBirth,Gendor,Address,Phone,
                        Email,NationalityCountryID,ImagePath) Values(@NationalNo,@FirstName,@SecondName,@ThirdName,@LastName,@DateOfBirth,
                         @Gendor,@Address,@Phone,@Email,@NationalityCountryID,@ImagePath);SELECT SCOPE_IDENTITY()";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@NationalNo", NationalNo);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            Command.Parameters.AddWithValue("@SecondName", SecondName);


            if (!string.IsNullOrWhiteSpace(ThirdName))
                Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                Command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);


            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            Command.Parameters.AddWithValue("@Gendor", Gendor);
            Command.Parameters.AddWithValue("@Address", Address);
            Command.Parameters.AddWithValue("@Phone", Phone);

            if (!string.IsNullOrWhiteSpace(Email))
                Command.Parameters.AddWithValue("@Email", Email);
            else
                Command.Parameters.AddWithValue("@Email", DBNull.Value);

            Command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);


            if (!string.IsNullOrWhiteSpace(ImagePath))
                Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                Command.Parameters.AddWithValue("@ImagePath", DBNull.Value);

            try
            {
                Connection.Open();

                object result = Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    PersonID = insertedID;
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                Connection.Close();
            }

            return PersonID;
        }

        public static bool UpdatePerson(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName,
            string LastName, DateTime DateOfBirth, short Gendor, string Address, string Phone, string Email, int NationalityCountryID,
            string ImagePath)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"UPDATE People SET NationalNo = @NationalNo,FirstName =@FirstName,SecondName=@SecondName,ThirdName=@ThirdName,
                    LastName = @LastName,DateOfBirth=@DateOfBirth,Gendor= @Gendor,Address=@Address,Phone=@Phone,Email=@Email,
                     NationalityCountryID = @NationalityCountryID,ImagePath=@ImagePath where PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@NationalNo", NationalNo);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            Command.Parameters.AddWithValue("@SecondName", SecondName);

            if (!string.IsNullOrWhiteSpace(ThirdName))
                Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                Command.Parameters.AddWithValue("@ThirdName", DBNull.Value);


            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            Command.Parameters.AddWithValue("@Gendor", Gendor);
            Command.Parameters.AddWithValue("@Address", Address);
            Command.Parameters.AddWithValue("@Phone", Phone);

            if (!string.IsNullOrWhiteSpace(Email))
                Command.Parameters.AddWithValue("@Email", Email);
            else
                Command.Parameters.AddWithValue("@Email", DBNull.Value);

            Command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (!string.IsNullOrWhiteSpace(ImagePath))
                Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                Command.Parameters.AddWithValue("@ImagePath", DBNull.Value);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();

                int RousAffected = Command.ExecuteNonQuery();

                if (RousAffected > 0)
                {
                    IsFound = true;
                }
            }
            catch (Exception ex)
            {
                IsFound = true;
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }

        public static DataTable GetAllPeople()
        {
            DataTable Dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"select People.PersonID , People.NationalNo, People.FirstName, People.SecondName,
                        People.ThirdName, People.LastName,
                        case 
                        WHEN People.Gendor = 0 THEN 'Male' 
                        else 'Female'
                        end as Gendor,
                        People.DateOfBirth, Countries.CountryName as Nationalily, People.Phone, People.Email
                        from People
                        inner join Countries on People.NationalityCountryID = Countries.CountryID
                        order by People.PersonID;";

            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    Dt.Load(Reader);
                }
                Reader.Close();
            }
            catch (Exception ex)
            {



            }
            finally
            {
                Connection.Close();
            }
            return Dt;
        }

        public static bool DeletePerson(int PersonID)
        {
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "Delete from People where PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();

                int RowsAffcted = Command.ExecuteNonQuery();

                if (RowsAffcted > 0)
                {
                    IsExist = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }
            return IsExist;
        }

        public static bool IsPersonExist(int PersonID)
        {
            bool isExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "Select Found = 1 from People where PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    isExist = true;
                }
                else
                {
                    isExist = false;
                }
            }
            catch (Exception ex)
            {
                isExist = false;
            }
            finally
            {
                Connection.Close();
            }
            return isExist;
        }

        public static bool IsPersonExist(string NationalNo)
        {
            bool isExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "Select Found = 1 from People where NationalNo = @NationalNo";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    isExist = true;
                }
                else
                {
                    isExist = false;
                }
            }
            catch (Exception ex)
            {
                isExist = false;
            }
            finally
            {
                Connection.Close();
            }
            return isExist;
        }
    }
}
