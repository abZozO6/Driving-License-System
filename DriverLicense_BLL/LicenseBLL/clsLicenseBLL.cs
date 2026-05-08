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
    public class clsLicenseBLL
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 };

        public clsDriverBLL DriverInfo;
        public int LicenseID { set; get; }
        public int ApplicationID { set; get; }
        public int DriverID { set; get; }
        public int LicenseClass { set; get; }

        private clsLicenseClassBLL _LicenseClassInfo;
        public  clsLicenseClassBLL LicenseClassInfo
        {
            get
            {
                if (_LicenseClassInfo == null)
                {
                    _LicenseClassInfo =
                        clsLicenseClassBLL.Find(LicenseClass);
                }

                return _LicenseClassInfo;
            }
        }


        private clsDetainedLicenseBLL _DetainedInfo;
        public  clsDetainedLicenseBLL DetainedInfo
        {
            get
            {
                if (_DetainedInfo == null)
                {
                    _DetainedInfo =
                        clsDetainedLicenseBLL.FindByLicenseID(
                            LicenseID);
                }

                return _DetainedInfo;
            }
        }

        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public string Notes { set; get; }
        public float PaidFees { set; get; }
        public bool IsActive { set; get; }
        public enIssueReason IssueReason { set; get; }
        public string IssueReasonText
        {
            get
            {
                return GetIssueReasonText(IssueReason);
            }
        }
        public int CreatedByUserID { set; get; }
        public bool IsDetained
        {
            get { return clsDetainedLicenseBLL.IsLicenseDetained(LicenseID); }
        }

        public clsLicenseBLL()

        {
            LicenseID = -1;
            ApplicationID = -1;
            DriverID = -1;
            LicenseClass = -1;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            Notes = "";
            PaidFees = 0;
            IsActive = true;
            IssueReason = enIssueReason.FirstTime;
            CreatedByUserID = -1;

            Mode = enMode.AddNew;

        }

        public clsLicenseBLL(int LicenseID, int ApplicationID, int DriverID, int LicenseClass,DateTime IssueDate,
            DateTime ExpirationDate, string Notes, float PaidFees, bool IsActive, enIssueReason IssueReason, int CreatedByUserID)

        {
            this.LicenseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.LicenseClass = LicenseClass;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.PaidFees = PaidFees;
            this.IsActive = IsActive;
            this.IssueReason = IssueReason;
            this.CreatedByUserID = CreatedByUserID;

            DriverInfo = clsDriverBLL.FindByDriverID(this.DriverID);

            Mode = enMode.Update;
        }

        private bool _AddNewLicense()
        {
            //call DataAccess Layer 

            LicenseID = clsLicense.AddNewLicense(ApplicationID, DriverID, LicenseClass,
               IssueDate, ExpirationDate, Notes, PaidFees,
               IsActive, (byte)IssueReason, CreatedByUserID);


            return LicenseID != -1;
        }

        private bool _UpdateLicense()
        {
            //call DataAccess Layer 

            return clsLicense.UpdateLicense(ApplicationID, LicenseID, DriverID, LicenseClass,
               IssueDate, ExpirationDate, Notes, PaidFees,
               IsActive, (byte)IssueReason, CreatedByUserID);
        }

        public static clsLicenseBLL Find(int LicenseID)
        {
            int ApplicationID = -1; 
            int DriverID = -1;
            int LicenseClass = -1;
            DateTime IssueDate = DateTime.Now;
            DateTime ExpirationDate = DateTime.Now;
            string Notes = "";
            float PaidFees = 0; 
            bool IsActive = true; 
            int CreatedByUserID = 1;
            byte IssueReason = 1;

            if (clsLicense.GetLicenseInfoByID(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClass,
            ref IssueDate, ref ExpirationDate, ref Notes,
            ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))

                return new clsLicenseBLL(LicenseID, ApplicationID, DriverID, LicenseClass,
                                     IssueDate, ExpirationDate, Notes,
                                     PaidFees, IsActive, (enIssueReason)IssueReason, CreatedByUserID);
            else
                return null;

        }

        public static DataTable GetAllLicenses()
        {
            return clsLicense.GetAllLicenses();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateLicense();

            }

            return false;
        }

        public static bool IsLicenseExistByPersonID(int PersonID, int LicenseClassID)
        {
            return GetActiveLicenseIDByPersonID(PersonID, LicenseClassID) != -1;
        }

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {

            return clsLicense.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);

        }

        public static DataTable GetDriverLicenses(int DriverID)
        {
            return clsLicense.GetDriverLicenses(DriverID);
        }

        public bool IsLicenseExpired()
        {

            return ExpirationDate < DateTime.Now;

        }

        public bool DeactivateCurrentLicense()
        {
            return clsLicense.DeactivateLicense(LicenseID);
        }

        public static string GetIssueReasonText(enIssueReason issueReason) //expression switch
        {
            return issueReason switch
            {
                enIssueReason.FirstTime =>"First Time",

                enIssueReason.Renew => "Renew",

                enIssueReason.DamagedReplacement => "Replacement for Damaged",

                enIssueReason.LostReplacement => "Replacement for Lost",

                _ => "Unknown"
            };
        }

        private clsApplicationBLL _CreateApplication( clsApplicationBLL.enApplicationType applicationType, int createdByUserID)
        {
            clsApplicationBLL application = new clsApplicationBLL();

            application.ApplicantPersonID = DriverInfo.PersonID;

            application.ApplicationDate = DateTime.Now;

            application.ApplicationTypeID =(int)applicationType;

            application.ApplicationStatus =  clsApplicationBLL.enApplicationStatus.Completed;

            application.LastStatusDate = DateTime.Now;

            application.PaidFees = clsApplicationTypeBLL.Find((int)applicationType).Fees;

            application.CreatedByUserID = createdByUserID;

            return application;
        }

        public int Detain(float FineFees, int CreatedByUserID)
        {
            clsDetainedLicenseBLL detainedLicense = new clsDetainedLicenseBLL();
            detainedLicense.LicenseID = LicenseID;
            detainedLicense.DetainDate = DateTime.Now;
            detainedLicense.FineFees = Convert.ToSingle(FineFees);
            detainedLicense.CreatedByUserID = CreatedByUserID;

            if (!detainedLicense.Save())
            {

                return -1;
            }

            return detainedLicense.DetainID;

        }

        public bool ReleaseDetainedLicense(int releasedByUserID, ref int applicationID)
        {
            clsApplicationBLL application =_CreateApplication( clsApplicationBLL.enApplicationType.ReleaseDetainedDrivingLicsense, releasedByUserID);

            if (!application.Save())
            {
                applicationID = -1;
                return false;
            }

            applicationID = application.ApplicationID;

            return DetainedInfo .ReleaseDetainedLicense(releasedByUserID, application.ApplicationID);
        }

        public clsLicenseBLL RenewLicense(string notes, int createdByUserID)
        {
            clsApplicationBLL application = _CreateApplication( clsApplicationBLL.enApplicationType.RenewDrivingLicense,createdByUserID);

            if (!application.Save())
            {
                return null;
            }

            clsLicenseBLL newLicense = new clsLicenseBLL();

            newLicense.ApplicationID = application.ApplicationID;

            newLicense.DriverID =DriverID;

            newLicense.LicenseClass = LicenseClass;

            newLicense.IssueDate = DateTime.Now;

            int defaultValidityLength = LicenseClassInfo.DefaultValidityLength;

            newLicense.ExpirationDate =DateTime.Now.AddYears(defaultValidityLength);

            newLicense.Notes = notes;

            newLicense.PaidFees =LicenseClassInfo.ClassFees;

            newLicense.IsActive = true;

            newLicense.IssueReason = enIssueReason.Renew;

            newLicense.CreatedByUserID =createdByUserID;

            if (!newLicense.Save())
            {
                return null;
            }

            // deactivate old license
            DeactivateCurrentLicense();

            return newLicense;
        }

        public clsLicenseBLL Replace(enIssueReason issueReason,int createdByUserID)
        {
            clsApplicationBLL.enApplicationType applicationType = 
              issueReason ==enIssueReason.DamagedReplacement? clsApplicationBLL.enApplicationType.ReplaceDamagedDrivingLicense : clsApplicationBLL.enApplicationType.ReplaceLostDrivingLicense;

            clsApplicationBLL application = _CreateApplication( applicationType, createdByUserID);

            if (!application.Save())
            {
                return null;
            }

            clsLicenseBLL newLicense = new clsLicenseBLL();

            newLicense.ApplicationID = application.ApplicationID;

            newLicense.DriverID =  DriverID;

            newLicense.LicenseClass = LicenseClass;

            newLicense.IssueDate = DateTime.Now;

            newLicense.ExpirationDate =  ExpirationDate;

            newLicense.Notes =  Notes;

            // no license fees for replacement
            newLicense.PaidFees = 0;

            newLicense.IsActive = true;

            newLicense.IssueReason = issueReason;

            newLicense.CreatedByUserID = createdByUserID;

            if (!newLicense.Save())
            {
                return null;
            }

            // deactivate old license
            DeactivateCurrentLicense();

            return newLicense;
        }


    }
}
