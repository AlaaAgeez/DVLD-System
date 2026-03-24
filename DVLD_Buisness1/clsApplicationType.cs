using DVLD_DataAccess1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness1
{
    public class clsApplicationType
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int ID {  get; set; }
        public string Title { get; set; }
        public float Fees { get; set; }

        public clsApplicationType()
        {
            this.ID = -1;
            this.Title ="";
            this.Fees = -1;

            Mode = enMode.AddNew;
        }

        private clsApplicationType(int ID, string ApplicationTypeTitle, float ApplicationFees)
        {
            this.ID = ID;
            this.Title = ApplicationTypeTitle;
            this.Fees = ApplicationFees;

            Mode = enMode.Update;
        }

        private bool _AddNewApplicationType()
        {
            this.ID = clsApplicationTypeData.AddNewApplicationType(this.Title, this.Fees);

            return (this.ID != -1);
        }

        private bool _UpdateApplicationType()
        {
            return clsApplicationTypeData.UpdateApplicationType(this.ID, this.Title, this.Fees);
        }

        public static DataTable GetAllApplicationType()
        {
            return clsApplicationTypeData.GetAllApplicationType();
        }

        public static clsApplicationType Find(int ID)
        {
            string Title = "";
            float Fees = -1;

            if (clsApplicationTypeData.GetApplicationTypeInfoID(ID,ref Title,ref Fees))
            {
                return new clsApplicationType(ID, Title, Fees);
            }
            return null;
        }

        public bool Save()
        {
            switch(Mode)
            {
               case enMode.AddNew:
                    if(_AddNewApplicationType())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateApplicationType();
            }
            return false;
        }
    }
}
