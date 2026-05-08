using DriverLicense_DAL;
using DriverLicense_DAL.People;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicense_BLL.PeopleBLL
{
    public class clsUserBLL
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int UserID { set; get; }
        public int PersonID { set; get; }

        public clsPersonBLL PersonInfo;
        public string UserName { set; get; }
        public string Password { set; get; }
        public bool IsActive { set; get; }

        public clsUserBLL()
        {
            UserID = -1;
            UserName = "";
            Password = "";
            IsActive = true;
            Mode = enMode.AddNew;
        }

        private clsUserBLL(clsUser user)

        {
            UserID = user.UserID;
            PersonID = user.PersonID;
            PersonInfo = clsPersonBLL.Find(user.PersonID);
            UserName = user.UserName;
            Password = user.Password;
            IsActive = user.IsActive;

            Mode = enMode.Update;
        }

        private bool _AddNewUser()
        {
            clsUser user = new clsUser();

            user.PersonID = PersonID;
            user.UserName = UserName;
            user.Password = Password;
            user.IsActive = IsActive;

            UserID = clsUserData.AddNewUser(user);

            return UserID != -1;
        }

        private bool _UpdateUser()
        {
            clsUser user = new clsUser();

            user.UserID = UserID;
            user.PersonID = PersonID;
            user.UserName = UserName;
            user.Password = Password;
            user.IsActive = IsActive;

            return clsUserData.UpdateUser(user);
        }

        public static clsUserBLL FindByUserID(int UserID)
        {
            clsUser user = clsUserData.GetUserInfoByUserID(UserID);

            if (user != null)
                return new clsUserBLL(user);
            else
                return null;
        }

        public static clsUserBLL FindByPersonID(int PersonID)
        {
            clsUser user = clsUserData.GetUserInfoByPersonID(PersonID);

            if (user != null)
                return new clsUserBLL(user);
            else
                return null;
        }

        public static clsUserBLL FindByUsernameAndPassword(string UserName, string Password)
        {
            clsUser user = clsUserData.GetUserInfoByUsernameAndPassword(UserName, Password);

            if (user != null)
                return new clsUserBLL(user);
            else
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

        public static bool isUserExist(int UserID)
        {
            return clsUserData.IsUserExist(UserID);
        }

        public static bool isUserExist(string UserName)
        {
            return clsUserData.IsUserExist(UserName);
        }

        public static bool isUserExistForPersonID(int PersonID) 
        {
            return clsUserData.IsUserExistForPersonID(PersonID);
        }


    }
}
