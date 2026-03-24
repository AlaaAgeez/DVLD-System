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
    public class clsApplicationTypeData
    {
        public static bool GetApplicationTypeInfoID(int ApplicationTypeID,ref string ApplicationTypeTitle,ref float ApplicationFees)
        {
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from ApplicationTypes where ApplicationTypeID = @ApplicationTypeID";

            SqlCommand Command = new SqlCommand(Query,Connection);

            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if(Reader.Read())
                {
                    IsExist = true;

                    ApplicationTypeTitle = (string)Reader["ApplicationTypeTitle"];
                    ApplicationFees = Convert.ToSingle(Reader["ApplicationFees"]);
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
            return IsExist;
        }

        public static DataTable GetAllApplicationType()
        {
            DataTable DT = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from ApplicationTypes order by ApplicationTypeTitle";

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
                //throw; 
            }
            finally
            {
                Connection.Close();
            }
            return DT;
        }

        public static int AddNewApplicationType(string ApplicationTypeTitle, float ApplicationFees)
        {
            int ApplicationTypeID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"INSERT INTO ApplicationType (ApplicationTypeTitle,ApplicationFees) VALUES
                        (@ApplicationTypeTitle,@ApplicationFees)
                        SELECT SCOPE_IDENTITY();";

            SqlCommand Command  = new SqlCommand(Query, Connection);

            try
            {
                Connection.Open();

                object result = Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(),out int insertedID))
                {
                    ApplicationTypeID = insertedID;
                }
            }
            catch
            {

            }
            finally
            {
                Connection.Close();
            }
            return ApplicationTypeID;
        }

        public static bool UpdateApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, float ApplicationFees)
        {
            bool IsUpdate = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"update ApplicationTypes SET ApplicationTypeTitle =@ApplicationTypeTitle,
                    ApplicationFees=@ApplicationFees where ApplicationTypeID =@ApplicationTypeID  ";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
            Command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                Connection.Open();

                int RowAffected = Command.ExecuteNonQuery();

                if (RowAffected >0)
                {
                    IsUpdate = true;
                }
            }
            catch
            {
                IsUpdate = false;
                throw;
            }
            finally
            {
                Connection.Close();
            }

            return IsUpdate;
        }


    }
}
