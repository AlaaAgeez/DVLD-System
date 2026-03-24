using DVLD_DataAccess1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness1
{
    public class clsTestType
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public enum enTestType { VisionTest = 1,WrittenTest = 2 , StreetTest = 3 }

        public clsTestType.enTestType ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Fees { get; set; }

        public clsTestType()
        {
            this.ID = clsTestType.enTestType.VisionTest;
            this.Title = "";
            this.Description = "";
            this.Fees = 0;

            Mode = enMode.AddNew;
        }

        private clsTestType(clsTestType.enTestType ID, string TestTypeTitle, string Description, float Fees)
        {
            this.ID = ID;
            this.Title = TestTypeTitle;
            this.Description = Description;
            this.Fees = Fees;

            Mode = enMode.Update;
        }

        private bool _AddNewTestType()
        {

            this.ID = (clsTestType.enTestType)clsTestTypeData.AddNewTestType(this.Title, this.Description, this.Fees);

            return (this.Title != "");
        }

        private bool _UpdateTestType()
        {
            if (clsTestTypeData.UpdateTestType((int)this.ID, this.Title, this.Description, this.Fees))
            {
                return true;
            }
            return false;
        }

        public static clsTestType Find(clsTestType.enTestType TestTypeID)
        {
            string TestTypeTitle = "", TestTypeDescription = "";
            float TestTypeFees = -1;

            if (clsTestTypeData.GetTestTypeInfoByID((int)TestTypeID, ref TestTypeTitle, ref TestTypeDescription, ref TestTypeFees))
            {
                return new clsTestType(TestTypeID, TestTypeTitle, TestTypeDescription, TestTypeFees);

            }
            return null;
        }

        public static DataTable GetAllTestType()
        {
            return clsTestTypeData.GetAllTestType();
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewTestType())
                    {
                        return true ;
                        Mode = enMode.Update;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                   return _UpdateTestType();
            }
            return false;
        }
    }
}
