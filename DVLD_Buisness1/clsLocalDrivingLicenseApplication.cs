using DVLD_DataAccess1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness1
{
    public class clsLocalDrivingLicenseApplication : clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int LocalDrivingLicenseApplicationID { get; set; }
        public int LicenseClassID { get; set; }
        //public clsLicenseClass LicenseClassInfo;

        public string PersonFullName
        {
            get
            {
                return base.PersonInfo.FullName;
            }
        }

        public clsLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = -1;
            this.LicenseClassID = -1;

            Mode = enMode.AddNew;
        }

        public clsLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicantPersonID,
            DateTime ApplicationDate, int ApplicationTypeID, enApplicationStatus ApplicationStatus,
            DateTime LastStatusDate, float PaidFees, int CreatedByUserID, int LicenseClassID)
        {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = (int)ApplicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.LicenseClassID = LicenseClassID;

            //this.LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);

            Mode = enMode.Update;
        }

        private bool _AddNewLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseApplicationData.AddNewLocalDrivingLicenseApplication
                (this.ApplicationID,this.LicenseClassID);

            return (this.LocalDrivingLicenseApplicationID !=-1);
        }

        private bool _UpdateLocalDrivingLicenseApplication()
        {
            return clsLocalDrivingLicenseApplicationData.UpdateLocalDrivingLicenseApplication
                (this.LocalDrivingLicenseApplicationID,this.ApplicationID,this.LicenseClassID);
        }

        public static clsLocalDrivingLicenseApplication FindByLocalDrivingApplicationID(int LocalDrivingLicenseApplicationID)
        {
            int ApplicationID = -1, LicenseClassID = -1;

            bool IsFound = clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByID
                (LocalDrivingLicenseApplicationID, ref ApplicationID, ref LicenseClassID);

            if (IsFound)
            {
                clsApplication Application = clsApplication.FindBaseApplication(ApplicationID);

                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, Application.ApplicantPersonID,
                    Application.ApplicationDate, Application.ApplicationTypeID, (enApplicationStatus)Application.ApplicationStatus,
                    Application.LastStatusDate, Application.PaidFees, Application.CreatedByUserID, LicenseClassID);
            }
            else
            {
                return null;
            }
        }

        public static clsLocalDrivingLicenseApplication FindByApplicationID(int ApplicationID)
        {
            int LocalDrivingLicenseApplicationID = -1;
            int LicenseClassID = -1;

            bool IsFound = clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByApplicationID
                (ApplicationID, ref LocalDrivingLicenseApplicationID, ref LicenseClassID);

            if (IsFound)
            {
                clsApplication Application = clsApplication.FindBaseApplication(ApplicationID);

                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID,Application.ApplicantPersonID,
                    Application.ApplicationDate,Application.ApplicationTypeID,(enApplicationStatus)Application.ApplicationStatus,
                    Application.LastStatusDate,Application.PaidFees, Application.CreatedByUserID,LicenseClassID);
            }
            else
            {
                return null;
            }
        }

        public bool Save()
        {
            base.Mode = (clsApplication.enMode)Mode;

            if (!base.Save())
                return false;

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLocalDrivingLicenseApplication())
                    {
                        Mode = enMode.Update;
                        return true; 
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateLocalDrivingLicenseApplication();
            }
            return false; 
        }

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenseApplicationData.GetAllLocalDrivingLicenseApplications();
        }

        public bool Delete()
        {
            if (this.LocalDrivingLicenseApplicationID == -1 || this.ApplicationID == -1)
            {
                return false;
            }

            if (!clsLocalDrivingLicenseApplicationData.DeleteLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID))
            {
                return false;
            }

            return base.Delete();
        }

        public int IssueLicenseForTheFirtTime(string Notes, int CreatedByUserID)
        {
            int DriverID = -1;

            clsDriver Driver = clsDriver.FindByPersonID(this.ApplicantPersonID);

            if (Driver == null)
            {
                //we check if the driver already there for this person.
                Driver = new clsDriver();

                Driver.PersonID = this.ApplicantPersonID;
                Driver.CreatedByUserID = CreatedByUserID;
                if (Driver.Save())
                {
                    DriverID = Driver.DriverID;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                DriverID = Driver.DriverID;
            }

            //now we diver is there, so we add new licesnse
            clsLicense License = new clsLicense();
            License.ApplicationID = this.ApplicationID;
            License.DriverID = DriverID;
            License.LicenseClass = this.LicenseClassID;
            License.IssueDate = DateTime.Now;
            //License.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            License.Notes = Notes;
            //License.PaidFees = this.LicenseClassInfo.ClassFees;
            License.IsActive = true;
            License.IssueReason = clsLicense.enIssueReason.FirstTime;
            License.CreatedByUserID = CreatedByUserID;

            if (License.Save())
            {
                //now we should set the application status to complete
                //this.SetComplete();
                return License.LicenseID;
            }
            else
            {
                return -1;
            }
        }





    }
}




