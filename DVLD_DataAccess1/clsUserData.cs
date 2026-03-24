using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess1
{
    public class clsUserData
    {
        public static bool GetUserInfoByUserID(int UserID ,ref int PersonID, ref string UserName, ref string Password,
            ref bool IsActive)
        {
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from users where UserID = @UserID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if(Reader.Read())
                {
                    IsExist = true;

                    PersonID = (int)Reader["PersonID"];
                    UserName = (string)Reader["UserName"];
                    Password = (string)Reader["Password"];
                    IsActive = (bool)Reader["IsActive"];
                }
                Reader.Close();
            }
            catch
            {
                return IsExist;
            }
            finally
            {
                Connection.Close();
            }

            return IsExist;
        }

        public static bool GetUserInfoByPersonID(int PersonID , ref int UserID,ref string UserName, ref string Password,
            ref bool IsActive)
        {
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from user where PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                SqlDataReader Reader  = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsExist = true;

                    UserID = (int)Reader["UserID"];
                    UserName = (string)Reader["UserName"];
                    Password = (string)Reader["Password"];
                    IsActive = (bool)Reader["IsActive"];
                }

                Reader.Close();
            }
            catch
            {
                IsExist = false;
            }
            finally
            {
                Connection.Close();
            }

            return IsExist;
        }

        public static bool GetUserInfoByUsernameAndPassword(string UserName, string Password, ref int UserID, ref int PersonID
            , ref bool IsActive)
        {
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from users where UserName = @UserName and Password = @Password";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@Password", Password);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if(Reader.Read())
                {
                    IsExist = true;

                    UserID = (int)Reader["UserID"];
                    PersonID = (int)Reader["PersonID"];
                    IsActive = (bool)Reader["IsActive"];
                }
                Reader.Close();
            }
            catch
            {
                IsExist = false;
            }
            finally
            {
                Connection.Close();
            }

            return IsExist;
        }

        public static int AddNewUser(int PersonID, string UserName, string Password, bool IsActive)
        {
            int UserID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"INSERT INTO Users (PersonID,UserName,Password,IsActive) VALUES 
                          (@PersonID,@UserName,@Password,@IsActive);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@Password", Password);
            Command.Parameters.AddWithValue("@IsActive", IsActive ? 1 : 0);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if(Result != null )
                {
                    UserID = Convert.ToInt32(Result);
                }
            }
            finally
            {
                Connection.Close();
            }

            return UserID;
        }

        public static bool UpdateUser(int User,int PersonID,string UserName,string Password,bool IsActive)
        {
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"UPDATE users set PersonID =@PersonID ,UserName = @UserName,Password = @Password,
                        IsActive = @IsActive WHERE UserID =@UserID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@Password", Password);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@UserID", User);

            try
            {
                Connection.Open();

                int RowAffected = Command.ExecuteNonQuery();

                if (RowAffected > 0)
                {
                    IsExist = true;
                }
            }
            finally
            {
                Connection.Close();
            }

            return IsExist;
        }

        public static DataTable GetAllUsers()
        {
            DataTable DT = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"select UserID,users.PersonID,FullName = FirstName + ' '+SecondName+ ' '+ThirdName+ ' '+LastName  
                    ,UserName,IsActive  from users inner join People on users.PersonID = People.PersonID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if(Reader.HasRows)
                {
                    DT.Load(Reader);
                }
            }
            finally
            {
                Connection.Close();
            }
            return DT;
        }

        public static bool DeleteUser(int UserID)
        {
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "DELETE FROM users where UserID = @UserID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                Connection.Open();

                int RowAffected = Command.ExecuteNonQuery();

                if(RowAffected > 0)
                {
                    IsExist = true;
                }
            }
            catch
            {
                IsExist = false;
            }
            finally
            {
                Connection.Close();
            }
            return IsExist;
        }

        public static bool IsUserExist(int UserID)
        {
            bool isExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select IsFound = 1 from users where UserID = @UserID ";

            SqlCommand  Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                Connection.Open();

                object result = Command.ExecuteScalar();

                if(result != null)
                {
                    isExist = true;
                }
            }
            catch
            {
                isExist = false;
            }
            finally
            {
                Connection.Close();
            }

            return isExist;
        }

        public static bool IsUserExist(string UserName)
        {
            bool isExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select IsFound = 1 from users where UserName = @UserName";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@UserName",UserName);

            try
            {
                Connection.Open();

                object result = Command.ExecuteScalar();

                if (result != null)
                {
                    isExist = true;
                }
            }
            catch
            {
                isExist = false;
            }
            finally
            {
                Connection.Close();
            }
            return isExist;
        }

        public static bool IsUserExistForPersonID(int PersonID)
        {
            bool isExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select IsFound = 1 from Users where PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();

                object result = Command.ExecuteScalar();

                if(result != null)
                {
                    isExist = true;
                }
            }
            catch
            {
                isExist = false;
            }
            finally
            {
                Connection.Close();
            }

            return isExist;
        }

        public static bool ChangePassword(int UserID,string NewPassword)
        {
            bool isExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "UPDATE USERS SET Password = @NewPassword WHERE UserID =@UserID ";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@NewPassword", NewPassword);
            Command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                Connection.Open();

                int RowAffected = Command.ExecuteNonQuery();

                if (RowAffected > 0)
                {
                    isExist = true;
                }
            }
            catch
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
