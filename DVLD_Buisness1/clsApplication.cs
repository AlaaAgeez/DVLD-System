using DVLD_BuisnessLayer;
using DVLD_DataAccess1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness1
{
    public class clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public enum enApplicationType { 
            NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, 
            RetakeTest = 7 }

        public enum enApplicationStatus { New = 1, Cancelled = 2, Completed = 3 }

        public int ApplicationID { set; get; }
        public int ApplicantPersonID { set; get; }
        public clsPerson PersonInfo { set; get; }
        public string ApplicantFullName 
        {
            get
            {
                return clsPerson.Find(ApplicantPersonID).FullName;
            }
        }
        public DateTime ApplicationDate { set; get; }
        public int ApplicationTypeID { set; get; }
        public clsApplicationType ApplicationTypeInfo;
        public enApplicationStatus ApplicationStatus { set; get; }
        public string StatusText
        {
            get
            {
                switch (ApplicationStatus)
                {
                    case enApplicationStatus.New:
                        return "New";

                    case enApplicationStatus.Cancelled:
                        return "Cancelled";

                    case enApplicationStatus.Completed:
                        return "Completed";

                    default:
                        return "Unknown";
                }
            }
        }
        
        public DateTime LastStatusDate { set; get; }
        public float PaidFees { set; get; }
        public int CreatedByUserID { set; get; }
        public clsUser CreatedByUserInfo;


        public clsApplication()
        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = -1;
            this.ApplicationStatus = enApplicationStatus.New;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
            this.Mode = enMode.AddNew;
        }

        private clsApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate
            , int ApplicationTypeID, enApplicationStatus ApplicationStatus, DateTime LastStatusDate, float PaidFees,
            int CreatedByUserID)
        {
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;

            this.PersonInfo = clsPerson.Find(ApplicantPersonID);

            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;

            this.ApplicationTypeInfo = clsApplicationType.Find(ApplicationTypeID);

            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = clsUser.FindByUserID(CreatedByUserID);

            this.Mode = enMode.Update;
        }

        private bool _AddNewApplication()
        {
            this.ApplicationID = clsApplicationData.AddNewApplication(this.ApplicantPersonID, this.ApplicationDate, this.ApplicationTypeID,
            (byte)this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);

            return (this.ApplicationID != -1);
        }

        private bool _UpdateApplication()
        {
            return clsApplicationData.UpdateApplication(this.ApplicationID, this.ApplicantPersonID, this.ApplicationDate,
            this.ApplicationTypeID, (byte)this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
        }

        public static clsApplication FindBaseApplication(int ApplicationID)
        {
            int ApplicantPersonID = -1,   ApplicationTypeID = -1, CreatedByUserID = -1;
            DateTime ApplicationDate = DateTime.Now, LastStatusDate = DateTime.Now;
            byte ApplicationStatus = 0;
            float PaidFees = -1;

            if (clsApplicationData.GetApplicationInfoByID(ApplicationID, ref ApplicantPersonID, ref ApplicationDate, 
                ref ApplicationTypeID, ref ApplicationStatus, ref LastStatusDate, ref PaidFees, ref CreatedByUserID)) 
            {
                return new clsApplication(ApplicationID, ApplicantPersonID, ApplicationDate, ApplicationTypeID,
                    (enApplicationStatus)ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID);
            }
            return null;
        }

        public bool Cancel()
        {
            return clsApplicationData.UpdateStatus(ApplicationID, 2);
        }

        public bool SetCompleted()
        {
            return clsApplicationData.UpdateStatus(ApplicationID, 3);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplication())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateApplication();
            }
            return false;
        }

        public bool Delete()
        {
            return clsApplicationData.DeleteApplican(this.ApplicationID);
        }

        public static bool IsApplicationExist(int ApplicationID)  
        {
            return clsApplicationData.IsApplicationExist(ApplicationID);
        }

        public static bool DoesPersonHaveActiveApplication(int ApplicationID, int ApplicationTypeID)
        {
            return clsApplicationData.DoesPersonHaveActiveApplication(ApplicationID, ApplicationTypeID);
        }

        public bool DoesPersonHaveActiveApplication(int ApplicationTypeID)
        {
            return clsApplicationData.IsApplicationExist(ApplicationTypeID);
        }

        public static int GetActiveApplicationID(int PersonID, clsApplication.enApplicationType ApplicationTypeID)
        {
            return clsApplicationData.GetActiveApplicationID(PersonID, (int)ApplicationTypeID);
        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID, int ApplicationTypeID, int LicenseClassID)
        {
            return clsApplicationData.GetActiveApplicationIDForLicenseClass(PersonID, ApplicationTypeID, LicenseClassID);
        }

        //public static int GetActiveApplicationID(clsApplication.enApplicationType ApplicationTypeID)
        //{
        //    return clsApplicationData.GetActiveApplicationID((int)ApplicationTypeID);

        //}
    }
}
