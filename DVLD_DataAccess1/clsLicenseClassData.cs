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
    public class clsLicenseClassData
    {
        public static bool GetLicenseClassInfoByID(int LicenseClassID,ref string ClassName, 
            ref string ClassDescription,ref byte MinimumAllowedAge, ref byte DefaultValidityLength,
            ref float ClassFees)
        {
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from LicenseClasses where LicenseClassID = @LicenseClassID";

            SqlCommand Command = new SqlCommand(Query,Connection);

            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if(Reader.Read())
                {
                    ClassName = (string)Reader["ClassName"];
                    ClassDescription = (string)Reader["ClassDescription"];
                    MinimumAllowedAge = (byte)Reader["MinimumAllowedAge"];
                    DefaultValidityLength = (byte)Reader["DefaultValidityLength"];
                    ClassFees = Reader["ClassFees"] == DBNull.Value ? 0f : Convert.ToSingle(Reader["ClassFees"]);

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

        public static bool GetLicenseClassInfoByClassName(string ClassName, ref int LicenseClassID,
    ref string ClassDescription, ref byte MinimumAllowedAge, ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool IsExist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from LicenseClasses where ClassName = @ClassName";

            SqlCommand Command = new SqlCommand(Query,Connection);

            Command.Parameters.AddWithValue("@ClassName", ClassName);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    LicenseClassID = (int)Reader["LicenseClassID"];
                    ClassDescription = (string)Reader["ClassDescription"];
                    MinimumAllowedAge = (byte)Reader["MinimumAllowedAge"];
                    DefaultValidityLength = (byte)Reader["DefaultValidityLength"];
                    ClassFees = (float)Reader["ClassFees"];

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

        public static DataTable GetAllLicenseClasses()
        {
            DataTable DT = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from LicenseClasses order by ClassName desc";

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

        public static int AddNewLicenseClass(string ClassName, string ClassDescription, byte MinimumAllowedAge, 
            byte DefaultValidityLength, float ClassFees)
        {
            int LicenseClassID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"insert into LicenseClasses (ClassName,ClassDescription,MinimumAllowedAge,DefaultValidityLength,ClassFees) 
                        VALUES 
                        (@ClassName,@ClassDescription,@MinimumAllowedAge,@DefaultValidityLength,@ClassFees); 
                        Select SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ClassName", ClassName);
            Command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
            Command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
            Command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
            Command.Parameters.AddWithValue("@ClassFees", ClassFees);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    LicenseClassID = insertedID;
                }
            }
            finally
            {
                Connection.Close();
            }

            return LicenseClassID;
        }

        public static bool UpdateLicenseClass(int LicenseClassID,string ClassName, string ClassDescription, 
            byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {
            bool IsUpdate = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = @"UPDATE LicenseClasses SET ClassName =@ClassName ,ClassDescription= @ClassDescription,
                    MinimumAllowedAge = @MinimumAllowedAge,DefaultValidityLength = @DefaultValidityLength,
                    ClassFees= @ClassFees WHERE LicenseClassID = @LicenseClassID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@ClassName", ClassName);
            Command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
            Command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
            Command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
            Command.Parameters.AddWithValue("@ClassFees", ClassFees);

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
