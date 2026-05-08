using DriverLicense_BLL.ApplicationsBLL;
using DriverLicense_BLL.PeopleBLL;
using DriverLicense_DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicense_BLL.License
{
    public class clsInternationalLicenseBLL :clsApplicationBLL
    {

        public enum enMode { AddNew = 0, Update = 1 };

        public enMode Mode = enMode.AddNew;


        public clsDriverBLL _DriverInfo;
        public clsDriverBLL DriverInfo
        {
            get
            {
                if (_DriverInfo == null)
                {
                    _DriverInfo =
                        clsDriverBLL.FindByDriverID(
                            DriverID);
                }

                return _DriverInfo;
            }
        }
        public int InternationalLicenseID { set; get; }
        public int DriverID { set; get; }
        public int IssuedUsingLocalLicenseID { set; get; }
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public bool IsActive { set; get; }


        public clsInternationalLicenseBLL()

        {
            //here we set the applicaiton type to New International License.
            ApplicationTypeID = (int)enApplicationType.NewInternationalLicense;

            InternationalLicenseID = -1;
            DriverID = -1;
            IssuedUsingLocalLicenseID = -1;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;

            IsActive = true;


            Mode = enMode.AddNew;

        }

        public clsInternationalLicenseBLL(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
             float PaidFees, int CreatedByUserID,int InternationalLicenseID, int DriverID, int IssuedUsingLocalLicenseID,
             DateTime IssueDate, DateTime ExpirationDate, bool IsActive)

        {
            //this is for the base clase
            base.ApplicationID = ApplicationID;
            base.ApplicantPersonID = ApplicantPersonID;
            base.ApplicationDate = ApplicationDate;
            ApplicationTypeID = (int)enApplicationType.NewInternationalLicense;
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

            Mode = enMode.Update;
        }

        private bool _AddNewInternationalLicense()
        {

            InternationalLicenseID = clsInternationalLicenseData.AddNewInternationalLicense(ApplicationID,DriverID,
                IssuedUsingLocalLicenseID,IssueDate, ExpirationDate, IsActive, CreatedByUserID);

            return InternationalLicenseID != -1;
        }

        private bool _UpdateInternationalLicense()
        {

            return clsInternationalLicenseData.UpdateInternationalLicense(InternationalLicenseID,ApplicationID, DriverID,
                IssuedUsingLocalLicenseID,IssueDate, ExpirationDate, IsActive, CreatedByUserID);
        }



        public static clsInternationalLicenseBLL Find(int InternationalLicenseID)
        {
            int ApplicationID = -1;
            int DriverID = -1; 
            int IssuedUsingLocalLicenseID = -1;
            DateTime IssueDate = DateTime.Now;
            DateTime ExpirationDate = DateTime.Now;
            bool IsActive = true; 
            int CreatedByUserID = 1;

            if (clsInternationalLicenseData.GetInternationalLicenseInfoByID(InternationalLicenseID, ref ApplicationID, ref DriverID,
                ref IssuedUsingLocalLicenseID,ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID))
            {
                //now we find the base application
                clsApplicationBLL Application = FindBaseApplication(ApplicationID);

                return new clsInternationalLicenseBLL(Application.ApplicationID, Application.ApplicantPersonID,
                        Application.ApplicationDate, Application.ApplicationStatus, Application.LastStatusDate,
                        Application.PaidFees, Application.CreatedByUserID, InternationalLicenseID, DriverID, IssuedUsingLocalLicenseID,
                        IssueDate, ExpirationDate, IsActive);

            }

            else
                return null;

        }

        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicenseData.GetAllInternationalLicenses();

        }

        public bool Save()
        {

            //Because of inheritance first we call the save method in the base class,
            //it will take care of adding all information to the application table.
            base.Mode = (clsApplicationBLL.enMode)Mode;
            if (!base.Save())
                return false;

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

            }

            return false;
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

