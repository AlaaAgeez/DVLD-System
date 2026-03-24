using DVLD_DataAccess1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness1
{
    public class clsInternationalLicense : clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public clsDriver DriverInfo;

        public int InternationalLicenseID { set; get; }
        public int DriverID { set; get; }
        public int IssuedUsingLocalLicenseID { set; get; }
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public bool IsActive { set; get; }

        public clsInternationalLicense()
        {
            this.InternationalLicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.IsActive = true;

            base.ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense;
            base.ApplicationStatus = enApplicationStatus.New;
            base.PaidFees = 0;

            this.Mode = enMode.AddNew;
        }

        public clsInternationalLicense(int ApplicationID, int ApplicantPersonID,
        DateTime ApplicationDate, enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
        float PaidFees, int CreatedByUserID,
        int InternationalLicenseID, int DriverID, int IssuedUsingLocalLicenseID,
        DateTime IssueDate, DateTime ExpirationDate, bool IsActive)
        {
            base.ApplicationID = ApplicationID;
            base.ApplicantPersonID = ApplicantPersonID;
            base.ApplicationDate = ApplicationDate;

            base.ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense;

            base.ApplicationStatus = ApplicationStatus;
            base.LastStatusDate = LastStatusDate;
            base.PaidFees = PaidFees;
            base.CreatedByUserID = CreatedByUserID;

            this.InternationalLicenseID = InternationalLicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.IsActive = IsActive;

            this.CreatedByUserID = CreatedByUserID;

            this.DriverInfo = clsDriver.FindByDriverID(this.DriverID);

            Mode = enMode.Update;
        }

        private bool _AddNewInternationalLicense()
        {
            this.InternationalLicenseID = clsInternationalLicenseData.AddNewInternationalLicense(
                    this.ApplicationID,this.DriverID,this.IssuedUsingLocalLicenseID,this.IssueDate,
                    this.ExpirationDate,this.IsActive,this.CreatedByUserID);

            return (this.InternationalLicenseID != -1);
        }

        private bool _UpdateInternationalLicense()
        {
            return clsInternationalLicenseData.UpdateInternationalLicense(
                this.InternationalLicenseID,this.ApplicationID,this.DriverID,this.IssuedUsingLocalLicenseID,
                this.IssueDate,this.ExpirationDate,this.IsActive,this.CreatedByUserID);
        }

        public static clsInternationalLicense Find(int InternationalLicenseID)
        {
            int ApplicationID = -1, DriverID = -1, IssuedUsingLocalLicenseID = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.MinValue, ExpirationDate = DateTime.MinValue, CreatedDate = DateTime.MinValue;
            bool IsActive = false;

            if (clsInternationalLicenseData.GetInternationalLicenseInfoByID(InternationalLicenseID,
                                        ref ApplicationID,ref DriverID,ref IssuedUsingLocalLicenseID,
                                        ref IssueDate,ref ExpirationDate,ref IsActive,ref CreatedByUserID,
                                        ref CreatedDate))
            {
                clsApplication Application = clsApplication.FindBaseApplication(ApplicationID);

                if (Application == null)
                    return null;

                return new clsInternationalLicense(
                    Application.ApplicationID,
                    Application.ApplicantPersonID,
                    Application.ApplicationDate,
                    Application.ApplicationStatus,
                    Application.LastStatusDate,
                    Application.PaidFees,
                    Application.CreatedByUserID,

                    InternationalLicenseID,
                    DriverID,
                    IssuedUsingLocalLicenseID,
                    IssueDate,
                    ExpirationDate,
                    IsActive
                );
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicenseData.GetAllInternationalLicenses();
        }

        public bool Save()
        {
            base.Mode = (clsApplication.enMode)Mode;

            if (!base.Save())
            {
                return false;
            }

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewInternationalLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateInternationalLicense();

                default:
                    return false;
            }
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            return clsInternationalLicenseData.GetActiveInternationalLicenseIDByDriverID(DriverID);
        }

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            return clsInternationalLicenseData.GetDriverInternationalLicenses(DriverID);
        }
    }
}
