using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccess1
{
    public class clsLocalDrivingLicenseApplicationData
    {
        public static bool GetLocalDrivingLicenseApplicationInfoByID(int LocalDrivingLicenseApplicationID, 
            ref int ApplicationID, ref int LicenseClassID)
        {
            bool IsFound= false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"select * from LocalDrivingLicenseApplications where LocalDrivingLicenseApplicationID
                        = @LocalDrivingLicenseApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if(Reader.Read())
                {
                    ApplicationID = (int)Reader["ApplicationID"];
                    LicenseClassID = (int)Reader["LicenseClassID"];

                    IsFound = true;
                }
                Reader.Close();
            }
            catch (Exception ex)
            {
                IsFound = false;

                Connection.Close();
            }
            finally
            {
                Connection.Close();
            }

            return IsFound;
        }

        public static bool GetLocalDrivingLicenseApplicationInfoByApplicationID(int ApplicationID ,
            ref int LocalDrivingLicenseApplicationID, ref int LicenseClassID)
        {
            bool IsFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"select * from LocalDrivingLicenseApplications where ApplicationID
                        = @ApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    LocalDrivingLicenseApplicationID = (int)Reader["LocalDrivingLicenseApplicationID"];
                    LicenseClassID = (int)Reader["LicenseClassID"];

                    IsFound = true;
                }
                Reader.Close();
            }
            catch (Exception ex)
            {
                IsFound = false;

                Connection.Close();
            }
            finally
            {
                Connection.Close();
            }

            return IsFound;
        }

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            DataTable DT = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"select * from LocalDrivingLicenseApplications_View order by ApplicationDate desc";

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
            catch (Exception ex)
            {
                Connection.Close();
            }
            finally
            {
                Connection.Close();
            }

            return DT;
        }

        public static int AddNewLocalDrivingLicenseApplication(int ApplicationID, int LicenseClassID)
        {
            int ApplicationLicenseID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"INSERT INTO LocalDrivingLicenseApplications (ApplicationID,LicenseClassID) value 
                        (@ApplicationID,@LicenseClassID); SELECT SCOPE_IDENTITY(); ";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    ApplicationLicenseID = insertedID;
                }
            }
            catch (Exception ex)
            {
                Connection.Close();
            }
            finally
            {
                Connection.Close();
            }

            return ApplicationLicenseID;
        }

        public static bool UpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID,int ApplicationID, int LicenseClassID)
        {
            bool IsUpdate = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"Update LocalDrivingLicenseApplications SET ApplicationID =@ApplicationID,
            LicenseClassID=@LicenseClassID,where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID ";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                Connection.Open();

                int rowsAffected = Command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    IsUpdate = true;
                }
            }
            catch (Exception ex)
            {
                Connection.Close();
            }
            finally
            {
                Connection.Close();
            }

            return IsUpdate;
        }

        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            bool IsUpdate = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"DELETE FROM LocalDrivingLicenseApplications 
                        where LocalDrivingLicenseApplicationID =@LocalDrivingLicenseApplicationID ";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                Connection.Open();

                int rowsAffected = Command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    IsUpdate = true;
                }
            }
            catch (Exception ex)
            {
                Connection.Close();
            }
            finally
            {
                Connection.Close();
            }

            return IsUpdate;
        }

        public static bool DousPassTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool Result = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"SELECT top 1 TestResult
                           FROM LocalDrivingLicenseApplications
                           INNER JOIN TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID
                           INNER JOIN Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                           WHERE (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)
                             AND (TestAppointments.TestTypeID = @TestTypeID)  
                           ORDER BY TestAppointments.TestAppointmentID desc";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                Connection.Open();

                int rowsAffected = Command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Result = true;
                }
            }
            catch (Exception ex)
            {
                Connection.Close();
            }
            finally
            {
                Connection.Close();
            }

            return Result;
        }

        public static bool TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {




            return true;

        }





    }
}
