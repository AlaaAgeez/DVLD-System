using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess1
{
    public class clsDriverData
    {
        public static bool GetDriverInfoByDriverID(int DriverID, ref int PersonID, ref int CreatedByUserID,
            ref DateTime CreatedDate)
        {
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from Drivers where DriverID = @DriverID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    PersonID = (int)Reader["PersonID"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    CreatedDate = (DateTime)Reader["CreatedDate"];
                    IsExist = true;
                }
                Reader.Close();
            }
            finally
            {
                Connection.Close();
            }
            return IsExist;
        }

        public static bool GetDriverInfoByPersonID(int PersonID, ref int DriverID, ref int CreatedByUserID, 
            ref DateTime CreatedDate)
        {
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from Drivers where PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    DriverID = (int)Reader["DriverID"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    CreatedDate = (DateTime)Reader["CreatedDate"];
                    IsExist = true;
                }
                Reader.Close();
            }
            finally
            {
                Connection.Close();
            }
            return IsExist;
        }

        public static DataTable GetAllDriver()
        {
            DataTable DT = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from Drivers_View order by FullName";

            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if(Reader.HasRows)
                {
                    DT.Load(Reader);
                }
                Reader.Close();
            }
            finally
            {
                Connection.Close();
            }
            return DT;
        }

        public static int AddNewDriver(int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            int DriverID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"INSERT INTO Drivers (PersonID,CreatedByUserID,CreatedDate)VALUES
                            (@PersonID,@CreatedByUserID,@CreatedDate);
                                Select SCOPE_IDENTITY(); ";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@CreatedDate", CreatedDate);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    DriverID = insertedID;
                }
            }
            finally
            {
                Connection.Close();
            }

            return DriverID;
        }

        public static bool UpdateDriver(int DriverID ,int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            bool IsUpdate = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"UPDATE Drivers SET PersonID = @PersonID,CreatedByUserID = @CreatedByUserID,
                CreatedDate = @CreatedDate where DriverID = @DriverID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {
                Connection.Open();

                int RowAffected = Command.ExecuteNonQuery();

                if (RowAffected > 0)
                {
                    IsUpdate = true;
                }
            }
            finally
            {
                Connection.Close();
            }

            return IsUpdate;
        }

    }
}