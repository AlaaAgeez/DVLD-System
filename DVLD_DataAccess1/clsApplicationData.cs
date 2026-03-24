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
    public class clsApplicationData
    {
        public static bool GetApplicationInfoByID(int ApplicationID, ref int ApplicantPersonID, ref DateTime ApplicationDate,
            ref int ApplicationTypeID, ref byte ApplicationStatus, ref DateTime LastStatusDate, ref float PaidFees,
            ref int CreatedByUserID)
        {
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from Applications where ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(Query,Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    ApplicantPersonID = (int)Reader["ApplicantPersonID"];
                    ApplicationDate = (DateTime)Reader["ApplicationDate"];
                    ApplicationTypeID = (int)Reader["ApplicationTypeID"];
                    ApplicationStatus = (byte)Reader["ApplicationStatus"];
                    LastStatusDate = (DateTime)Reader["LastStatusDate"];
                    PaidFees = (float)Reader["PaidFees"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];

                    IsExist = true;
                }
                Reader.Close();
            }
            catch (Exception ex)
            {
                IsExist = false;
            }
            finally
            {
                Connection.Close();
            }

            return IsExist;
        }

        public static DataTable GetAllApplications()
        {
            DataTable DT = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from Applications order by Application DESC";

            SqlCommand Command = new SqlCommand(Query,Connection);

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
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return DT;
        }

        public static int AddNewApplication(int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
            byte ApplicationStatus, DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {
            int ApplicationID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"insert into Applications (ApplicantPersonID,ApplicationDate,ApplicationTypeID,ApplicationStatus,
                    LastStatusDate,PaidFees,CreatedByUserID) value (@ApplicantPersonID,@ApplicationDate,@ApplicationTypeID
                    ,@ApplicationStatus,@LastStatusDate,@PaidFees,@CreatedByUserID); SELECT  SCOPE_IDENTITY()";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            Command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            Command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int insertedID)) 
                {
                    ApplicationID = insertedID;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return ApplicationID;
        }

        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, 
            int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {
            bool UpdatExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"Update Applications set ApplicantPersonID = @ApplicantPersonID,ApplicationDate = @ApplicationDate,
                ApplicationTypeID = @ApplicationTypeID,ApplicationStatus = @ApplicationStatus,LastStatusDate = @LastStatusDate,
                PaidFees = @PaidFees,CreatedByUserID=@CreatedByUserID where ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            Command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            Command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                Connection.Open();

                int rowsAffected = Command.ExecuteNonQuery();

                if (rowsAffected > 0) 
                {
                    UpdatExist = true;
                }
            }
            catch (Exception ex)
            {
                UpdatExist = false;
            }
            finally
            {
                Connection.Close();
            }

            return UpdatExist;
        }

        public static bool DeleteApplican(int ApplicationID)
        {
            bool IsDeleted = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "DELETE FROM Applications where ApplicationID =@ApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                Connection.Open();

                int rowsAffected = Command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    IsDeleted = true;
                }
            }
            catch (Exception ex)
            {
                IsDeleted = false;
            }
            finally
            {
                Connection.Close();
            }
            return IsDeleted;
        }

        public static bool IsApplicationExist(int ApplicationID)
        {
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select Found = 1 from Applications where ApplicationID =@ApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                Connection.Open();

                int rowsAffected = Command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    IsExist = true;
                }
            }
            catch (Exception ex)
            {
                IsExist = false;
            }
            finally
            {
                Connection.Close();
            }

            return IsExist;
        }

        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {
            return (GetActiveApplicationID(PersonID, ApplicationTypeID ) != -1);
        }

        public static int GetActiveApplicationID(int PersonID, int ApplicationTypeID)
        {
            int ActiveApplicationID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"select ActiveApplicationID = ApplicationID from Applications where 
               ApplicantPersonID = @PersonID and ApplicationTypeID = @ApplicationTypeID and ApplicationStatus  = 1";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(),out int insertedID))
                {
                    ActiveApplicationID = insertedID;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return ActiveApplicationID;
        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID,int ApplicationTypeID,int LicenseClassID)
        {
            int ActiveApplicationID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"SELECT ActiveApplicationID = Applications.ApplicationID FROM
                        Applications INNER JOIN
                LocalDrivingLicenseApplications ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                           WHERE ApplicantPersonID = @ApplicantPersonID
                                    AND ApplicationTypeID = @ApplicationTypeID
                                 AND LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                                 AND ApplicationStatus = 1;";
            SqlCommand Command = new SqlCommand(Query, Connection);


            Command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if(Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    ActiveApplicationID = insertedID;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Connection.Close();
            }

            return ActiveApplicationID;
        }

        public static bool UpdateStatus(int ApplicationID,short NewStatus)
        {
            int rowAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "Update Applications set ApplicationStatus =@NewStatus,LastStatusDate = @LastStatusDate where ApplicationID =@ApplicationID ";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@NewStatus", NewStatus);
            Command.Parameters.AddWithValue("@LastStatusDate",DateTime.Now);

            try
            {
                Connection.Open();

                rowAffected = Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                Connection.Close();
            }

            return (rowAffected > 0);
        }
    }
}
