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
    public class clsDetainedLicenseData
    {
        public static bool GetDetainedLicenseInfoByID(int DetainID, ref int LicenseID, ref DateTime DetainDate, 
            ref float FineFees, ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate,
            ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from DetainedLicenses where DetainID = @DetainID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@DetainID", DetainID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if(Reader.Read())
                {
                    LicenseID = (int)Reader["LicenseID"];
                    DetainDate = (DateTime)Reader["DetainDate"];
                    FineFees = (float)Reader["FineFees"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IsReleased = (bool)Reader["IsReleased"];
                    ReleaseDate = (DateTime)Reader["ReleaseDate"];
                    ReleasedByUserID = (int)Reader["ReleasedByUserID"];
                    ReleaseApplicationID = (int)Reader["ReleaseApplicationID"];

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

        public static bool GetDetainedLicenseInfoByLicenseID(int LicenseID, ref int DetainID, ref DateTime DetainDate,
    ref float FineFees, ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate,
    ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from DetainedLicenses where LicenseID = @LicenseID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    DetainID = (int)Reader["DetainID"];
                    DetainDate = (DateTime)Reader["DetainDate"];
                    FineFees = (float)Reader["FineFees"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IsReleased = (bool)Reader["IsReleased"];
                    ReleaseDate = (DateTime)Reader["ReleaseDate"];
                    ReleasedByUserID = (int)Reader["ReleasedByUserID"];
                    ReleaseApplicationID = (int)Reader["ReleaseApplicationID"];

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

        public static DataTable GetAllDetainedLicenses()
        {
            DataTable DT = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from DetainedLicenses_View order by IsReleased";

            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
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

        public static int AddNewDetainedLicenses(int LicenseID, DateTime DetainDate, float FineFees,
            int CreatedByUserID, bool IsReleased, DateTime ReleaseDate, int ReleasedByUserID,
            int ReleaseApplicationID)
        {
            int DetainID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"INSERT INTO DetainedLicenses (LicenseID,DetainDate,FineFees,CreatedByUserID,
                        IsReleased,ReleaseDate,ReleasedByUserID,ReleaseApplicationID) VALUES
                        (@LicenseID,@DetainDate,@FineFees,@CreatedByUserID,@IsReleased,@ReleaseDate,@ReleasedByUserID,
                        @ReleaseApplicationID); SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@DetainDate", DetainDate);
            Command.Parameters.AddWithValue("@FineFees", FineFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@IsReleased", IsReleased);
            Command.Parameters.AddWithValue("@ReleaseDate", IsReleased ? ReleaseDate : (object)DBNull.Value);
            Command.Parameters.AddWithValue("@ReleasedByUserID", IsReleased ? ReleasedByUserID : (object)DBNull.Value);
            Command.Parameters.AddWithValue("@ReleaseApplicationID", IsReleased ? ReleaseApplicationID : (object)DBNull.Value);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    DetainID = insertedID;
                }                
            }
            finally
            {
                Connection.Close();
            }

            return DetainID;
        }

        public static bool UpdateDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate, float FineFees,
            int CreatedByUserID)
        {
            bool IsUpdated = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"UPDATE DetainedLicenses SET LicenseID = @LicenseID,DetainDate = @DetainDate,
                    FineFees=@FineFees, CreatedByUserID=@CreatedByUserID WHERE DetainID =@DetainID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@DetainID", DetainID);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@DetainDate", DetainDate);
            Command.Parameters.AddWithValue("@FineFees", FineFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                Connection.Open();

                int RowAffected = Command.ExecuteNonQuery();

                if (RowAffected >0)
                {
                    IsUpdated = true;
                }
            }
            finally
            {
                Connection.Close();
            }

            return IsUpdated;
        }

        public static bool ReleaseDetainedLicense(int DetainID, int ReleasedByUserID, int ReleaseApplicationID)
        {
            bool IsReleaseDetained = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"UPDATE dbo.DetainedLicenses
                            SET IsReleased = 1,
                            ReleaseDate = @ReleaseDate,
                            ReleasedByUserID = @ReleasedByUserID,
                            ReleaseApplicationID = @ReleaseApplicationID
                            WHERE DetainID = @DetainID;";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@DetainID", DetainID);
            Command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
            Command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            Command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);

            try
            {
                Connection.Open();

                int RowsAffected = Command.ExecuteNonQuery();

                if (RowsAffected > 0)
                {
                    IsReleaseDetained = true;
                }
            }
            finally
            {
                Connection.Close();
            }

            return IsReleaseDetained;
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            bool IsDetained = false;

            SqlConnection connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string query = @"select IsDetained = 1 from DetainedLicenses
                            where LicenseID = @LicenseID and IsReleased = 0 ;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                object Result = command.ExecuteScalar();

                if (Result != null)
                {
                    IsDetained = true;
                }
            }
            finally
            {
                connection.Close();
            }
            return IsDetained;
        }
    }
}
