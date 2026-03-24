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
    public class clsInternationalLicenseData
    {
        public static bool GetInternationalLicenseInfoByID(int InternationalLicenseID,
                 ref int ApplicationID, ref int DriverID, ref int IssuedUsingLocalLicenseID,
                 ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive, ref int CreatedByUserID,
                 ref DateTime CreatedDate)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string query = "SELECT * FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

            try
            {
                connection.Open();

                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.Read())
                {
                    ApplicationID = (int)Reader["ApplicationID"];
                    DriverID = (int)Reader["DriverID"];
                    IssuedUsingLocalLicenseID = (int)Reader["IssuedUsingLocalLicenseID"];
                    IssueDate = (DateTime)Reader["IssueDate"];
                    ExpirationDate = (DateTime)Reader["ExpirationDate"];
                    IsActive = (bool)Reader["IsActive"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    CreatedDate = (DateTime)Reader["CreatedDate"];

                    isFound = true;
                }
                Reader.Close();
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static DataTable GetAllInternationalLicenses()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string query = @"SELECT InternationalLicenseID, ApplicationID, DriverID, 
                              IssuedUsingLocalLicenseID, IssueDate, 
                              ExpirationDate, IsActive
                              FROM InternationalLicenses
                              ORDER BY IsActive DESC, ExpirationDate DESC";
                         
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.HasRows)
                {
                    dt.Load(Reader);
                }

                Reader.Close();
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string query = @"SELECT InternationalLicenseID, ApplicationID, IssuedUsingLocalLicenseID, IssueDate, 
                            ExpirationDate, IsActive
                            FROM InternationalLicenses 
                            WHERE DriverID = @DriverID
                            ORDER BY ExpirationDate DESC";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                connection.Open();

                SqlDataReader Reader = command.ExecuteReader();

                if (Reader.HasRows)
                {
                    dt.Load(Reader);
                }

                Reader.Close();
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static int AddNewInternationalLicense(int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID,
    DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            int InternationalLicenseID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string query = @"UPDATE InternationalLicenses SET IsActive = 0
                         WHERE DriverID = @DriverID AND IsActive = 1;

                          INSERT INTO InternationalLicenses
                          (ApplicationID,DriverID,IssuedUsingLocalLicenseID,IssueDate,ExpirationDate,
                              IsActive,CreatedByUserID )

                          VALUES(@ApplicationID,@DriverID,@IssuedUsingLocalLicenseID,@IssueDate,
                              @ExpirationDate,@IsActive,@CreatedByUserID);

                          SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DriverID", DriverID); 
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    InternationalLicenseID = insertedID;
                }
            }
            finally
            {
                connection.Close();
            }

            return InternationalLicenseID;
        }

        public static bool UpdateInternationalLicense(int InternationalLicenseID, int ApplicationID,
    int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate,
    bool IsActive, int CreatedByUserID)
        {
            bool IsUpdate = false;

            SqlConnection connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string query = @"UPDATE InternationalLicenses SET
                            ApplicationID = @ApplicationID,
                            DriverID = @DriverID,
                            IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID,
                            IssueDate = @IssueDate,
                            ExpirationDate = @ExpirationDate,
                            IsActive = @IsActive,
                            CreatedByUserID = @CreatedByUserID
                        WHERE InternationalLicenseID = @InternationalLicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();

                int RowAffected = command.ExecuteNonQuery();

                if (RowAffected > 0)
                {
                    IsUpdate = true;
                }
            }
            finally
            {
                connection.Close();
            }

            return IsUpdate;
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            int InternationalLicenseID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string query = @"SELECT TOP 1 InternationalLicenseID
                         FROM InternationalLicenses
                         WHERE DriverID = @DriverID 
                         AND GETDATE() BETWEEN IssueDate AND ExpirationDate
                         ORDER BY ExpirationDate DESC;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    InternationalLicenseID = (int)result;
                }
            }
            finally
            {
                connection.Close();
            }

            return InternationalLicenseID;
        }





    }
}
