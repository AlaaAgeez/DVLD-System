using DVLD_BuisnessLayer;
using DVLD_DataAccess1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness1
{
    public class clsUser
    {
        public enum enMode {AddNew = 0,Update =1};
        public enMode Mode = enMode.AddNew;

        public int UserID { get; set; }
        public int PersonID { get; set; }
        public clsPerson PersonInfo;
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }


        public clsUser()
        {
            this.UserID = -1;
            this.UserName = "";
            this.Password = "";
            this.IsActive = false;

            Mode = enMode.AddNew;
        }

        private clsUser(int UserID,int PersonID,string UserName,string Password,bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.PersonInfo = clsPerson.Find(PersonID);
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;

            Mode = enMode.Update;
        }

        private bool _AddNewUser()
        {
            this.UserID = clsUserData.AddNewUser(this.PersonID, this.UserName, this.Password, this.IsActive);

            return (UserID != -1);
        }

        private bool _UpdateUser()
        {
            if (clsUserData.UpdateUser(this.UserID, this.PersonID, this.UserName, this.Password, this.IsActive))
            {
                return true;
            }
            return false;
        }

        public static clsUser FindByUserID(int UserID)
        {
            int PersonID = 0;
            string UserName = "", Password = "";
            bool IsActive = false;

            if(clsUserData.GetUserInfoByUserID(UserID,ref PersonID,ref UserName,ref Password,ref IsActive))
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }

            return null;
        }

        public static clsUser FindByPersonID(int PersonID)
        {
            int UserID = 0;
            string UserName = "", Password = "";
            bool IsActive = false;

            if(clsUserData.GetUserInfoByPersonID(PersonID,ref UserID,ref UserName,ref Password,ref IsActive))
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }
            return null;
        }

        public static clsUser FindByUserNameAndPassword(string UserName,string Password)
        {
            int UserID = 0,PersonID = 0;
            bool IsActive = false;

            if (clsUserData.GetUserInfoByUsernameAndPassword(UserName, Password, ref UserID, ref PersonID, ref IsActive)) 
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }
            return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateUser();
            }
            return false;
        } 

        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }

        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }

        public static bool IsUserExist(int UserID)
        {
            return clsUserData.IsUserExist(UserID);
        }
        public static bool IsUserExist(string UserName)
        {
            return clsUserData.IsUserExist(UserName);
        }

        public static bool IsUserExistForPersonID(int PersonID)
        {
            return clsUserData.IsUserExistForPersonID(PersonID);
        }
    }
}
