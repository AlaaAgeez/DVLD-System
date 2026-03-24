using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsCountryData
    {
        public static bool GetCountryInfoByID(int ID, ref string CountryName)
        {
            bool IsExist = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from Countries where CountryID = @CountryID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@CountryID", ID);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    CountryName = (string)Reader["CountryName"];
                    IsExist = true;
                }
                else
                    IsExist = false;

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

        public static bool GetCountryInfoByName(string CountryName, ref int ID)
        {
            bool IsExist = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Query = "select * from Countries where CountryName = @CountryName";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    ID = (int)Reader["CountryID"];
                    IsExist = true;
                }
                else
                {
                    IsExist = false;
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

        public static DataTable GetAllCountries()
        {
            DataTable DT = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessString.ConnectionString);

            string Qyery = "select * from Countries";


            SqlCommand Command = new SqlCommand(Qyery, Connection);

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

    }
}
