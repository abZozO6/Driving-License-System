using DriverLicense_BLL.PeopleBLL;
using DriverLicense_DAL;
using DriverLicense_DAL.People;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicense_BLL.License
{
    public class clsDetainedLicenseBLL
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public int DetainID { set; get; }
        public int LicenseID { set; get; }
        public DateTime DetainDate { set; get; }

        public float FineFees { set; get; }
        public int CreatedByUserID { set; get; }

        private clsUserBLL _CreatedByUserInfo;
        public clsUserBLL CreatedByUserInfo
        {
            get
            {
                if (_CreatedByUserInfo == null)
                {
                    _CreatedByUserInfo =
                        clsUserBLL.FindByUserID(CreatedByUserID);
                }

                return _CreatedByUserInfo;
            }
        }

        private clsUserBLL _ReleasedByUserInfo;
        public clsUserBLL ReleasedByUserInfo
        {
            get
            {
                if (_ReleasedByUserInfo == null &&
                    ReleasedByUserID != -1)
                {
                    _ReleasedByUserInfo =
                        clsUserBLL.FindByUserID(ReleasedByUserID);
                }

                return _ReleasedByUserInfo;
            }
        }
        public bool IsReleased { set; get; }
        public DateTime? ReleaseDate { get; set; }
        public int ReleasedByUserID { get; set; }
        public int? ReleaseApplicationID { set; get; }

        public clsDetainedLicenseBLL()
        {
            DetainID = -1;
            LicenseID = -1;
            DetainDate = DateTime.Now;
            FineFees = 0;
            CreatedByUserID = -1;

            IsReleased = false;

            ReleaseDate = null;
            ReleasedByUserID = 0;
            ReleaseApplicationID = null;

            Mode = enMode.AddNew;
        }

        private clsDetainedLicenseBLL( int detainID, int licenseID, DateTime detainDate, float fineFees, int createdByUserID, bool isReleased,
             DateTime? releaseDate,int releasedByUserID, int? releaseApplicationID)
        {
            DetainID = detainID;
            LicenseID = licenseID;
            DetainDate = detainDate;
            FineFees = fineFees;
            CreatedByUserID = createdByUserID;

            IsReleased = isReleased;

            ReleaseDate = releaseDate;
            ReleasedByUserID = releasedByUserID;
            ReleaseApplicationID = releaseApplicationID;

            Mode = enMode.Update;
        }

        private bool _AddNewDetainedLicense()
        {
            //call DataAccess Layer 

            DetainID = clsDetainedLicenseData.AddNewDetainedLicense(
                LicenseID, DetainDate, FineFees, CreatedByUserID);

            return DetainID != -1;
        }

        private bool _UpdateDetainedLicense()
        {
            //call DataAccess Layer 

            return clsDetainedLicenseData.UpdateDetainedLicense(
                DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID);
        }

        private static clsDetainedLicenseBLL _CreateDetainedLicenseObject( int detainID,int licenseID,DateTime detainDate,
            float fineFees, int createdByUserID, bool isReleased,
            DateTime? releaseDate,int releasedByUserID, int? releaseApplicationID)
        {
            return new clsDetainedLicenseBLL(
                detainID,
                licenseID,
                detainDate,
                fineFees,
                createdByUserID,
                isReleased,
                releaseDate,
                releasedByUserID,
                releaseApplicationID);
        } // Helper Method to avoid repeating constructor calls
       
        public static clsDetainedLicenseBLL Find(int detainID)
        {
            int licenseID = -1;
            DateTime detainDate = DateTime.Now;

            float fineFees = 0;

            int createdByUserID = -1;

            bool isReleased = false;

            DateTime? releaseDate = null;

            int releasedByUserID = 0;

            int? releaseApplicationID = null;

            if (!clsDetainedLicenseData.GetDetainedLicenseInfoByID( detainID, ref licenseID, ref detainDate, ref fineFees,
                ref createdByUserID, ref isReleased,  ref releaseDate, ref releasedByUserID, ref releaseApplicationID))
            {
                return null;
            }

            return _CreateDetainedLicenseObject( detainID,licenseID,detainDate,fineFees, createdByUserID,
                isReleased, releaseDate, releasedByUserID, releaseApplicationID);
        }

        public static clsDetainedLicenseBLL FindByLicenseID(int licenseID)
        {
            int detainID = -1;

            DateTime detainDate = DateTime.Now;

            float fineFees = 0;

            int createdByUserID = -1;

            bool isReleased = false;

            DateTime? releaseDate = null;

            int releasedByUserID = 0;

            int? releaseApplicationID = null;

            if (!clsDetainedLicenseData.GetDetainedLicenseInfoByLicenseID(licenseID,ref detainID,ref detainDate,ref fineFees,ref createdByUserID,
                ref isReleased, ref releaseDate, ref releasedByUserID,ref releaseApplicationID))
            {
                return null;
            }

            return _CreateDetainedLicenseObject(
                detainID,
                licenseID,
                detainDate,
                fineFees,
                createdByUserID,
                isReleased,
                releaseDate,
                releasedByUserID,
                releaseApplicationID);
        }

        public static DataTable GetAllDetainedLicenses()
        {
            return clsDetainedLicenseData.GetAllDetainedLicenses();

        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDetainedLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDetainedLicense();

            }

            return false;
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            return clsDetainedLicenseData.IsLicenseDetained(LicenseID);
        }

        //public bool ReleaseDetainedLicense(int ReleasedByUserID, int ReleaseApplicationID)
        //{
        //    return clsDetainedLicenseData.ReleaseDetainedLicense(this.DetainID,
        //           ReleasedByUserID, ReleaseApplicationID);
        //}


        public bool ReleaseDetainedLicense( int ReleasedByUserID,int ReleaseApplicationID)
        {
            bool released = clsDetainedLicenseData.ReleaseDetainedLicense(DetainID, ReleasedByUserID, ReleaseApplicationID);

            if (released)
            {
                IsReleased = true;

                ReleaseDate = DateTime.Now;

                this.ReleasedByUserID = ReleasedByUserID;

                this.ReleaseApplicationID = ReleaseApplicationID;
            }

            return released;
        }  // update DAL AND Obj
    }
}
