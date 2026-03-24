using DVLD_DataAccess1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness1
{
    public class clsTest
    {
        private enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode = enMode.AddNew;
        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }

        public clsTestAppointment TestAppointmentInfo { get; set; }

        public bool TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }

        public clsTest()
        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestResult = false;
            this.Notes = "";
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;
        }

        public clsTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            this.TestID = TestID;
            this.TestAppointmentID = TestAppointmentID;

            this.TestAppointmentInfo = clsTestAppointment.Find(TestAppointmentID);

            this.TestResult = TestResult;
            this.Notes = Notes;
            this.CreatedByUserID = CreatedByUserID;

            Mode = enMode.Update;
        }

        private bool _AddNewTest()
        {
            this.TestID = clsTestData.AddNewTest(this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);

            return (this.TestID != -1);
        }

        private bool _UpdateTest()
        {
            return clsTestData.UpdateTestType(this.TestID, this.TestAppointmentID, this.TestResult,
                this.Notes, this.CreatedByUserID);
        }

        public static clsTest Find(int TestID)
        {
            int TestAppointmentID = 0, CreatedByUserID = 0;
            bool TestResult = false;
            string Notes = "";

            if (clsTestData.GetTestInfoByID(TestID, ref TestAppointmentID, ref TestResult, ref Notes, 
                ref CreatedByUserID))
            {
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);

            }
            return null;
        }

        public static clsTest FindLastTestPerPersonAndLicenseClass
            (int PersonID, int LicenseClassID, clsTestType.enTestType TestTypeID)
        {
            int TestID = 0;
            int TestAppointmentID = 0;
            bool TestResult = false;
            string Notes = "";
            int CreatedByUserID = 0;

            if (clsTestData.GetLastTestByPersonAndTestTypeAndLicenseClass( PersonID, LicenseClassID, 
                (int)TestTypeID,ref TestID, ref TestAppointmentID, ref TestResult, ref Notes, ref CreatedByUserID))
            { 
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            }
            return null;
        }

        public static DataTable GetAllTests()
        {
            return clsTestData.GetAllTests();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTest())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateTest();
            }
            return false;
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTestData.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }

        public static bool PassedAllTest(int LocalDrivingLicenseApplicationID)
        {
            return GetPassedTestCount(LocalDrivingLicenseApplicationID) == 3;
        }
    }
}
