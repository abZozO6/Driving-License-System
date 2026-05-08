using DriverLicense_BLL.License;
using DriverLicense_DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicense_BLL.PeopleBLL
{
    public class clsDriverBLL
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        private clsPersonBLL _PersonInfo;
        public clsPersonBLL PersonInfo
        {
            get
            {
                if (_PersonInfo == null)
                {
                    _PersonInfo =
                        clsPersonBLL.Find(PersonID);
                }

                return _PersonInfo;
            }
        }

        public int DriverID { set; get; }
        public int PersonID { set; get; }
        public int CreatedByUserID { set; get; }
        public DateTime CreatedDate { get; }

        public clsDriverBLL()

        {
            DriverID = -1;
            PersonID = -1;
            CreatedByUserID = -1;
            CreatedDate = DateTime.Now;
            Mode = enMode.AddNew;

        }

        public clsDriverBLL(int DriverID, int PersonID, int CreatedByUserID, DateTime CreatedDate)

        {
            this.DriverID = DriverID;
            this.PersonID = PersonID;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedDate = CreatedDate;

            Mode = enMode.Update;
        }

        private bool _AddNewDriver()
        {

            DriverID = clsDriver.AddNewDriver(PersonID, CreatedByUserID);


            return DriverID != -1;
        }

        private bool _UpdateDriver()
        {

            return clsDriver.UpdateDriver(DriverID, PersonID, CreatedByUserID);
        }


        public static clsDriverBLL FindByDriverID(int driverID)
        {
            int personID = -1;
            int createdByUserID = -1;
            DateTime createdDate = DateTime.Now;

            if (!clsDriver.GetDriverInfoByDriverID( driverID, ref personID, ref createdByUserID, ref createdDate))
            {
                return null;
            }

            return new clsDriverBLL(driverID,personID,createdByUserID,createdDate);
        }

        public static clsDriverBLL FindByPersonID(int PersonID)
        {

            int DriverID = -1;
            int CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;

            if (!clsDriver.GetDriverInfoByPersonID(PersonID, ref DriverID, ref CreatedByUserID, ref CreatedDate))

                return null;
            else
                return new clsDriverBLL(DriverID, PersonID, CreatedByUserID, CreatedDate);

        }

        public static DataTable GetAllDrivers()
        {
            return clsDriver.GetAllDrivers();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDriver())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDriver();

            }

            return false;
        }


        public static DataTable GetLicenses(int DriverID)
        {
            return clsLicenseBLL.GetDriverLicenses(DriverID);
        }


        public static DataTable GetInternationalLicenses(int DriverID)
        {
            return clsInternationalLicenseBLL.GetDriverInternationalLicenses(DriverID);
        }

    }
}
