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
    public class clsTestTypeData
    {
        public static DataTable GetAllTestType()
        {
            DataTable Dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from TestTypes order by TestTypeID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    Dt.Load(Reader);
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
            return Dt;
        }

        public static bool GetTestTypeInfoByID(int TestTypeID, ref string TestTypeTitle, ref string TestTypeDescription,
            ref float TestTypeFees)
        {
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from TestTypes where TestTypeID = @TestTypeID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    IsExist = true;

                    TestTypeTitle = (string)Reader["TestTypeTitle"];
                    TestTypeDescription = (string)Reader["TestTypeDescription"];
                    TestTypeFees = Convert.ToInt32(Reader["TestTypeFees"]);
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

        public static int AddNewTestType(string TestTypeTitle, string TestTypeDescription, float TestTypeFees)
        {
            int TestTypeID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"INSERT INTO TestTypes (TestTypeTitle,TestTypeDescription,TestTypeFees) VALUES 
                        (@TestTypeTitle,@TestTypeDescription,@TestTypeFees) ; SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
            Command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
            Command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);

            try
            {
                Connection.Open();

                object result = Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestTypeID = insertedID;
                }
            }
            catch
            {
                TestTypeID = -1;
            }
            finally
            {
                Connection.Close();
            }
            return TestTypeID;
        }

        public static bool UpdateTestType(int TestTypeID,string TestTypeTitle,string TestTypeDescription,float TestTypeFees)
        {
            bool IsUpdate = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"Update TestTypes set TestTypeTitle = @TestTypeTitle ,TestTypeDescription =@TestTypeDescription,
                        TestTypeFees = @TestTypeFees where TestTypeID = @TestTypeID ";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
            Command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
            Command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                Connection.Open();

                int RowAffected = Command.ExecuteNonQuery();

                if(RowAffected >0)
                {
                    IsUpdate = true;
                }
            }
            catch
            {
                IsUpdate = false;
            }
            finally
            {
                Connection.Close();
            }

            return IsUpdate;
        }
    }
}
