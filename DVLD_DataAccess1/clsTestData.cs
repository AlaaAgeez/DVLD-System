using DVLD_DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess1
{
    public class clsTestData
    {
        public static bool GetTestInfoByID(int TestID, ref int TestAppointmentID, ref bool TestResult, 
            ref string Notes, ref int CreatedByUserID)
        {
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from Tests where TestID = @TestID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestID", TestID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if(Reader.Read())
                {
                    TestAppointmentID = (int)Reader["TestAppointmentID"];
                    TestResult = (bool)Reader["TestResult"];
                    Notes = (string)Reader["Notes"];
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

        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass(int PersonID, int LicenseClassID,
            int TestTypeID, ref int TestID, ref int TestAppointmentID, ref bool TestResult, 
            ref string Notes, ref int CreatedByUserID)
        { 
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"SELECT TOP 1 
                        Tests.TestID, Tests.TestAppointmentID, Tests.TestResult, Tests.Notes, Tests.CreatedByUserID, 
                        Applications.ApplicantPersonID 
                    FROM LocalDrivingLicenseApplications
                    INNER JOIN TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID
                    INNER JOIN Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                    INNER JOIN Applications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID 
                    WHERE
                        Applications.ApplicantPersonID = @PersonID
                        AND LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                        AND TestAppointments.TestTypeID = @TestTypeID
                    ORDER BY 
                        Tests.TestAppointmentID DESC; ";


            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    TestID = (int)Reader["TestID"];
                    TestAppointmentID = (int)Reader["TestAppointmentID"];
                    TestResult = (bool)Reader["TestResult"];
                    Notes = Reader["Notes"] != DBNull.Value ? (string)Reader["Notes"] : "";
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

        public static DataTable GetAllTests()
        {
            DataTable DT = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from Tests order by TestID";

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

        public static int AddNewTest(int TestAppointmentID,bool TestResult,string Notes,int CreatedByUserID)
        {
            int TestID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"INSERT INTO Tests(TestAppointmentID,TestResult,Notes,CreatedByUserID) VALUES 
                    (@TestAppointmentID,@TestResult,@Notes,@CreatedByUserID);  SELECT SCOPE_IDENTITY();";


            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            Command.Parameters.AddWithValue("@TestResult", TestResult);
            Command.Parameters.AddWithValue("@Notes", Notes ?? (object)DBNull.Value);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if(Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    TestID = insertedID;
                }
            }
            finally
            {
                Connection.Close();
            }
            return TestID;
        }

        public static bool UpdateTestType(int TestID,  int TestAppointmentID,  bool TestResult,
             string Notes,  int CreatedByUserID)
        {
            bool IsUpdate = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"Update Tests SET TestAppointmentID = @TestAppointmentID,TestResult = @TestResult,
                    Notes =@Notes,CreatedByUserID= @CreatedByUserID 
                    where TestID = @TestID";

            SqlCommand Command = new SqlCommand(@Query, Connection);

            Command.Parameters.AddWithValue("@TestID", TestID);

            try
            {
                Connection.Open();

                int RowsAffected = Command.ExecuteNonQuery();

                if (RowsAffected > 0)
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

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            byte PassedTestCount = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string @Query = @"SELECT PassedTestCount = COUNT(Tests.TestID) FROM Tests

                    INNER JOIN TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID

                    WHERE TestAppointments.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                        AND Tests.TestResult = 1;";

            SqlCommand Command = new SqlCommand(@Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && Result != DBNull.Value)
                {
                    PassedTestCount = Convert.ToByte(Result);
                }
            }
            finally
            {
                Connection.Close();
            }

            return PassedTestCount;
        }
    }
}
