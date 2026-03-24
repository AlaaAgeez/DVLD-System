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
    public class clsTestAppointmentData
    {
        public static bool GetTestAppointmentInfoByID(int TestAppointmentID, ref int TestTypeID,
            ref int LocalDrivingLicenseApplicationID, ref DateTime AppointmentDate, ref float PaidFees,
            ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from TestAppointments where TestAppointmentID = @TestAppointmentID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    TestTypeID = (int)Reader["TestTypeID"];
                    LocalDrivingLicenseApplicationID = (int)Reader["LocalDrivingLicenseApplicationID"];
                    AppointmentDate = (DateTime)Reader["AppointmentDate"];
                    PaidFees = (float)Reader["PaidFees"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IsLocked = (bool)Reader["IsLocked"];
                    RetakeTestApplicationID = (int)Reader["RetakeTestApplicationID"];

                    IsFound = true;
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
            return IsFound;
        }

        public static bool GetLastTestAppointment(int LocalDrivingLicenseApplicationID,int TestTypeID, 
            ref int TestAppointmentID, ref DateTime AppointmentDate, ref float PaidFees, ref int CreatedByUserID,
            ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"select top 1 * from TestAppointments
                            where (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID )
                            and (TestTypeID = @TestTypeID)
                            ORDER BY TestAppointmentID DESC;";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    TestAppointmentID = (int)Reader["TestAppointmentID"];
                    AppointmentDate = (DateTime)Reader["AppointmentDate"];
                    PaidFees = (float)Reader["PaidFees"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IsLocked = (bool)Reader["IsLocked"];
                    RetakeTestApplicationID = (int)Reader["RetakeTestApplicationID"];

                    IsFound = true;
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
            return IsFound;
        }

        public static DataTable GetAllTestAppointments()
        {
            DataTable DT = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"select * from TestAppointments_View
                            ORDER BY AppointmentDate DESC;";

            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    DT.Load(Reader);
                }
                else
                {
                    Reader.Close();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }
            return DT;
        }

        public static DataTable GetApplicationTestAllTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID,int TestTypeID)
        {
            DataTable DT = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);
            string Query = @"SELECT TestAppointmentID, AppointmentDate, PaidFees, IsLocked
                        FROM TestAppointments
                        WHERE
                        (TestTypeID = @TestTypeID)
                    AND (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)
                    order by TestAppointmentID desc;";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    DT.Load(Reader);
                }
                else
                {
                    Reader.Close();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }
            return DT;
        }

        public static int AddNewTestAppointment(int TestTypeID,
            int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, float PaidFees,
            int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            int TestAppointmentID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"INSERT INTO TestAppointments(TestTypeID,LocalDrivingLicenseApplicationID,
            AppointmentDate,PaidFees,CreatedByUserID,IsLocked,RetakeTestApplicationID) VALUES (@TestTypeID,
            @LocalDrivingLicenseApplicationID,@AppointmentDate,@PaidFees,@CreatedByUserID,@IsLocked,@RetakeTestApplicationID);
                    SELECT SCOPE_IDENTITY()";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@IsLocked", IsLocked);
            Command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    TestAppointmentID = insertedID;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }
            return TestAppointmentID;
        }

        public static bool UpdateTestAppointment(int TestAppointmentID , int TestTypeID,
          int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, float PaidFees,
             int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            bool IsUpdate = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"UPDATE TestAppointments SET TestTypeID =@TestTypeID,
                LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID,AppointmentDate = @AppointmentDate,
                PaidFees = @PaidFees,CreatedByUserID=@CreatedByUserID,IsLocked=@IsLocked,RetakeTestApplicationID = @RetakeTestApplicationID
                where TestAppointmentID = @TestAppointmentID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@IsLocked", IsLocked);
            Command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

            try
            {
                Connection.Open();

                int rowAffected = Command.ExecuteNonQuery();

                if (rowAffected > 0)
                {
                    IsUpdate = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }
            return IsUpdate;
        }

        public static int GetTestID(int TestAppointmentID)
        {
            int TestID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"select TestID from Tests where TestAppointmentID =@TestAppointmentID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    TestID = insertedID;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }
            return TestID;
        }        
    }
}
