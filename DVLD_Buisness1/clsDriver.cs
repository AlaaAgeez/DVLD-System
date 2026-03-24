using DVLD_BuisnessLayer;
using DVLD_DataAccess1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness1
{
    public class clsDriver
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public clsPerson PersonInfo;

        public int DriverID { set; get; }
        public int PersonID { set; get; }
        public int CreatedByUserID { set; get; }
        public DateTime CreatedDate { get; }

        public clsDriver()
        {
            this.DriverID = -1;
            this.PersonID = -1;
            this.CreatedByUserID = -1;
            this.CreatedDate = DateTime.Now;

            Mode = enMode.AddNew;
        }

        public clsDriver(int DriverID, int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            this.DriverID = DriverID;
            this.PersonID = PersonID;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedDate = CreatedDate;
            this.PersonInfo = clsPerson.Find(PersonID);

            Mode = enMode.Update;
        }

        private bool _AddNewDriver()
        {
            this.DriverID = clsDriverData.AddNewDriver(PersonID, CreatedByUserID, CreatedDate);

            return (this.DriverID != -1);
        }

        private bool _UpdateDriver()
        {
            return clsDriverData.UpdateDriver(DriverID, PersonID, CreatedByUserID, CreatedDate);
        }

        public static clsDriver FindByDriverID(int DriverID)
        {
            int PersonID = -1, CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;

            if(clsDriverData.GetDriverInfoByDriverID(DriverID,ref PersonID,ref CreatedByUserID,ref CreatedDate))
            {
                return new clsDriver(DriverID, PersonID,  CreatedByUserID,  CreatedDate);
            }
            return null;
        }

        public static clsDriver FindByPersonID(int PersonID)
        {
            int DriverID = -1, CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;

            if (clsDriverData.GetDriverInfoByPersonID(PersonID, ref DriverID, ref CreatedByUserID,ref CreatedDate))
            {
                return new clsDriver(PersonID, PersonID, CreatedByUserID, CreatedDate);
            }
            return null;
        }

        public static DataTable GetAllDrivers()
        {
            return clsDriverData.GetAllDriver();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDriver())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateDriver();
            }
            return false;
        }

        public static DataTable GetLicenses(int DriverID)
        {
            return clsLicense.GetDriverLicenses(DriverID);
        }

        //public static DataTable GetInternationaLicenses(int DriverID)
        //{
        //    return clsLicense.GetInternationaDriverLicenses(DriverID);
        //}
    }
}
