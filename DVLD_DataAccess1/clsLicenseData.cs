using DVLD_DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccess1
{
    public class clsLicenseData
    {
        public static bool GetLicenseInfoByID(int LicenseID, ref int ApplicationID, ref int DriverID,
            ref int LicenseClass, ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes,
            ref float PaidFees, ref bool IsActive, ref byte IssueReason, ref int CreatedByUserID)
        {
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from Licenses where LicenseID = @LicenseID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if(Reader.Read())
                {
                    ApplicationID = (int)Reader["ApplicationID"];
                    DriverID = (int)Reader["DriverID"];
                    LicenseClass = (int)Reader["LicenseClass"];
                    IssueDate = (DateTime)Reader["IssueDate"];
                    ExpirationDate = (DateTime)Reader["ExpirationDate"];
                    Notes = Reader["Notes"] == DBNull.Value ? "" : (string)Reader["Notes"];
                    PaidFees = Reader["PaidFees"] == DBNull.Value ? 0f : Convert.ToSingle(Reader["PaidFees"]);
                    IsActive = (bool)Reader["IsActive"];
                    IssueReason = (byte)Reader["IssueReason"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];

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

        public static DataTable GetAllLicenses()
        {
            DataTable DT = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from Licenses";

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

        public static DataTable GetDriverLicenses(int DriverID)
        {
            DataTable DT = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"SELECT
                     Licenses.LicenseID,
                     ApplicationID,
                     LicenseClasses.ClassName, Licenses.IssueDate,
                     Licenses.ExpirationDate, Licenses.IsActive
                 FROM Licenses INNER JOIN
                      LicenseClasses ON Licenses.LicenseClassID = LicenseClasses.LicenseClassID
                 where DriverID=@DriverID
                 Order By IsActive Desc, ExpirationDate Desc";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@DriverID", DriverID);

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

        public static int AddNewLicense(int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate,
            DateTime ExpirationDate, string Notes, float PaidFees, bool IsActive, byte IssueReason,
            int CreatedByUserID)
        {
            int LicenseID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"INSERT INTO Licenses
             (ApplicationID,DriverID,LicenseClass,IssueDate,ExpirationDate,Notes,PaidFees,IsActive,IssueReason,CreatedByUserID) 
               VALUES 
             (@ApplicationID,@DriverID,@LicenseClass,@IssueDate,@ExpirationDate,@Notes,@PaidFees,@IsActive,@IssueReason,@CreatedByUserID);
                SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
            Command.Parameters.AddWithValue("@IssueDate", IssueDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            Command.Parameters.AddWithValue("@Notes", Notes);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@IssueReason", IssueReason);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            
            try
            {
                Connection.Open();

                object Reslut = Command.ExecuteScalar();

                if (Reslut != null && int.TryParse(Reslut.ToString(), out int insertedID)) 
                {
                    LicenseID = insertedID;
                }
            }
            finally
            {
                Connection.Close();
            }

            return LicenseID;
        }

        public static bool UpdateLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass,
            DateTime IssueDate, DateTime ExpirationDate, string Notes, float PaidFees, bool IsActive,
            byte IssueReason, int CreatedByUserID)
        {
            bool IsUpdate = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"Update Licenses set ApplicationID = @ApplicationID,DriverID =@DriverID,
            LicenseClass=@LicenseClass,IssueDate=@IssueDate,ExpirationDate=@ExpirationDate,Notes=@Notes,
              PaidFees=@PaidFees,IsActive=@IsActive,IssueReason=@IssueReason,CreatedByUserID=@CreatedByUserID
                where LicenseID = @LicenseID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
            Command.Parameters.AddWithValue("@IssueDate", IssueDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            Command.Parameters.AddWithValue("@Notes", Notes);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@IssueReason", IssueReason);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

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

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"SELECT Licenses.LicenseID
                            FROM Licenses INNER JOIN
                            Drivers ON Licenses.DriverID = Drivers.DriverID and 
                            WHERE
                            Licenses.LicenseClass = @LicenseClass
                            AND Drivers.PersonID = @PersonID
                            AND IsActive=1;";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@LicenseClass", LicenseClassID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;
                }
            }
            finally
            {
                Connection.Close();
            }

            return LicenseID;
        }

        public static bool DeactivateLicense(int LicenseID)
        {
            bool IsDeactivated = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"UPDATE Licenses SET IsActive = 0 WHERE LicenseID=@LicenseID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                Connection.Open();

                int rowsAffected = Command.ExecuteNonQuery();

                if(rowsAffected > 0)
                {
                    IsDeactivated = true;
                }
            }
            finally
            {
                Connection.Close();
            }

            return IsDeactivated;
        }
    }
}
