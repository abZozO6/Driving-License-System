using DriverLicense_BLL.ApplicationsBLL;
using DriverLicense_BLL.DrivingExamBLL;
using DriverLicense_BLL.License;
using DriverLicense_BLL.PeopleBLL;
using DriverLicense_DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicense_BLL.LicenseBLL
{
    public class clsLocalDrivingLicenseApplicationBLL : clsApplicationBLL
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int LocalDrivingLicenseApplicationID { set; get; }
        public int LicenseClassID { set; get; }

        private clsLicenseClassBLL _LicenseClassInfo;

        public clsLicenseClassBLL LicenseClassInfo
        {
            get
            {
                if (_LicenseClassInfo == null)
                    _LicenseClassInfo = clsLicenseClassBLL.Find(LicenseClassID);

                return _LicenseClassInfo;
            }
        }
        public clsPersonBLL ApplicantPersonInfo
        {
            get
            {
                return clsPersonBLL.Find(ApplicantPersonID);
            }
        }
        //  lazy loading
        public string PersonFullName
        {
            get
            {
                return ApplicantPersonInfo.FullName;
            }
        }

        public clsLocalDrivingLicenseApplicationBLL()

        {
            LocalDrivingLicenseApplicationID = -1;
            LicenseClassID = -1;


            Mode = enMode.AddNew;

        }

        private clsLocalDrivingLicenseApplicationBLL(int LocalDrivingLicenseApplicationID,int ApplicationID, int ApplicantPersonID,
          DateTime ApplicationDate, int ApplicationTypeID, enApplicationStatus ApplicationStatus,
          DateTime LastStatusDate, float PaidFees, int CreatedByUserID,int LicenseClassID)
        {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.LicenseClassID = LicenseClassID;

            Mode = enMode.Update;
        }

        private bool _AddNewLocalDrivingLicenseApplication()
        {
            //call DataAccess Layer 

            LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseApplicationData.AddNewLocalDrivingLicenseApplication
                (
                ApplicationID, LicenseClassID);

            return LocalDrivingLicenseApplicationID != -1;
        }

        private bool _UpdateLocalDrivingLicenseApplication()
        {
            //call DataAccess Layer 

            return clsLocalDrivingLicenseApplicationData.UpdateLocalDrivingLicenseApplication
                (
                LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID);

        }

        public static clsLocalDrivingLicenseApplicationBLL FindByLocalDrivingAppLicenseID(int LocalDrivingLicenseApplicationID)
        {
            // 
            int ApplicationID = -1, LicenseClassID = -1;

            bool IsFound = clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByID
                (LocalDrivingLicenseApplicationID, ref ApplicationID, ref LicenseClassID);


            if (IsFound)
            {
                //now we find the base application
                clsApplicationBLL Application = FindBaseApplication(ApplicationID);

                //we return new object of that person with the right data
                return new clsLocalDrivingLicenseApplicationBLL(
                    LocalDrivingLicenseApplicationID, Application.ApplicationID,
                    Application.ApplicantPersonID,
                                     Application.ApplicationDate, Application.ApplicationTypeID,
                                    Application.ApplicationStatus, Application.LastStatusDate,
                                     Application.PaidFees, Application.CreatedByUserID, LicenseClassID);
            }
            else
                return null;


        }

        public static clsLocalDrivingLicenseApplicationBLL FindByApplicationID(int ApplicationID)
        {
            // 
            int LocalDrivingLicenseApplicationID = -1, LicenseClassID = -1;

            bool IsFound = clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByApplicationID
                (ApplicationID, ref LocalDrivingLicenseApplicationID, ref LicenseClassID);


            if (IsFound)
            {
                //now we find the base application
                clsApplicationBLL Application = FindBaseApplication(ApplicationID);

                //we return new object of that person with the right data
                return new clsLocalDrivingLicenseApplicationBLL(
                    LocalDrivingLicenseApplicationID, Application.ApplicationID,
                    Application.ApplicantPersonID, Application.ApplicationDate, Application.ApplicationTypeID,
                    Application.ApplicationStatus, Application.LastStatusDate,
                    Application.PaidFees, Application.CreatedByUserID, LicenseClassID);
            }
            else
                return null;


        }

        public bool Save()
        {
            // Sync child mode with parent mode before calling base.Save()
            base.Mode =(clsApplicationBLL.enMode) Mode;

            // Save data into Applications table first.
            // If saving the base application fails, stop the whole process.
            if (!base.Save())
                return false;

            // After saving the base application,
            // save the Local Driving License Application data.
            switch (Mode)
            {
                case enMode.AddNew:

                    // Insert new record into LocalDrivingLicenseApplications table
                    if (_AddNewLocalDrivingLicenseApplication())
                    {
                        // Object is now stored in DB,
                        // future Save() calls should perform UPDATE instead of INSERT.
                        Mode = enMode.Update;

                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    // Update existing LocalDrivingLicenseApplication record
                    return _UpdateLocalDrivingLicenseApplication();
            }

            return false;
        }

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenseApplicationData.GetAllLocalDrivingLicenseApplications();
        }

        public bool Delete()
        {
            // Delete child record first to avoid FK constraint issues
            if (!clsLocalDrivingLicenseApplicationData.DeleteLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID))
            {
                return false;
            }

            // Delete base application record
            return base.Delete();
        }

        public bool DoesPassTestType(clsTestTypeBLL.enTestType TestTypeID)

        {
            return clsLocalDrivingLicenseApplicationData.DoesPassTestType(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public bool DoesPassPreviousTest(clsTestTypeBLL.enTestType CurrentTestType)
        {

            switch (CurrentTestType)
            {
                case clsTestTypeBLL.enTestType.VisionTest:
                    //in this case no required prvious test to pass.
                    return true;

                case clsTestTypeBLL.enTestType.WrittenTest:
                    //Written Test, you cannot sechdule it before person passes the vision test.
                    //we check if pass visiontest 1.

                    return DoesPassTestType(clsTestTypeBLL.enTestType.VisionTest);


                case clsTestTypeBLL.enTestType.StreetDrivingTest:

                    //Street Test, you cannot sechdule it before person passes the written test.
                    //we check if pass Written 2.
                    return DoesPassTestType(clsTestTypeBLL.enTestType.WrittenTest);

                default:
                    return false;
            }
        }

        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID,clsTestTypeBLL.enTestType TestTypeID)

        {
            return clsLocalDrivingLicenseApplicationData.DoesPassTestType(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public bool DoesAttendTestType(clsTestTypeBLL.enTestType TestTypeID)

        {
            return clsLocalDrivingLicenseApplicationData.DoesAttendTestType(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public byte TotalTrialsPerTest(clsTestTypeBLL.enTestType testTypeID)
        {
            return TotalTrialsPerTest( LocalDrivingLicenseApplicationID, testTypeID);
        }

        public static byte TotalTrialsPerTest(int localDrivingLicenseApplicationID,clsTestTypeBLL.enTestType testTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialsPerTest(localDrivingLicenseApplicationID,(int)testTypeID);
        }

        public static bool AttendedTest(int LocalDrivingLicenseApplicationID, clsTestTypeBLL.enTestType TestTypeID)

        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialsPerTest(LocalDrivingLicenseApplicationID, (int)TestTypeID) > 0;
        }

        public bool AttendedTest(clsTestTypeBLL.enTestType testTypeID)
        {
            return AttendedTest(LocalDrivingLicenseApplicationID, testTypeID);
        }

        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, clsTestTypeBLL.enTestType TestTypeID)

        {

            return clsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public bool IsThereAnActiveScheduledTest(clsTestTypeBLL.enTestType TestTypeID)

        {

            return IsThereAnActiveScheduledTest(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public clsTestBLL GetLastTestPerTestType(clsTestTypeBLL.enTestType TestTypeID)
        {
            return clsTestBLL.GetLastTest(ApplicantPersonID, LicenseClassID, TestTypeID);
        }

        public byte GetPassedTestCount()
        {
            return clsTestBLL.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }

        public bool PassedAllTests()
        {
            return clsTestBLL.PassedAllTests(LocalDrivingLicenseApplicationID);
        }

        public static bool PassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            //if total passed test less than 3 it will return false otherwise will return true
            return clsTestBLL.PassedAllTests(LocalDrivingLicenseApplicationID);
        }

        public int IssueLicenseForTheFirtTime(string Notes, int CreatedByUserID)
        {
            int DriverID = -1;

            clsDriverBLL Driver = clsDriverBLL.FindByPersonID(ApplicantPersonID);

            if (Driver == null)
            {
                //we check if the driver already there for this person.
                Driver = new clsDriverBLL();

                Driver.PersonID = ApplicantPersonID;
                Driver.CreatedByUserID = CreatedByUserID;
                if (Driver.Save())
                {
                    DriverID = Driver.DriverID;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                DriverID = Driver.DriverID;
            }
            //now we diver is there, so we add new licesnse

            clsLicenseBLL License = new clsLicenseBLL();
            License.ApplicationID = ApplicationID;
            License.DriverID = DriverID;
            License.LicenseClass = LicenseClassID;
            License.IssueDate = DateTime.Now;
            License.ExpirationDate = DateTime.Now.AddYears(LicenseClassInfo.DefaultValidityLength);
            License.Notes = Notes;
            License.PaidFees = LicenseClassInfo.ClassFees;
            License.IsActive = true;
            License.IssueReason = clsLicenseBLL.enIssueReason.FirstTime;
            License.CreatedByUserID = CreatedByUserID;

            if (License.Save())
            {
                //now we should set the application status to complete.
                SetComplete();

                return License.LicenseID;
            }

            else
                return -1;
        }

        public bool IsLicenseIssued()
        {
            return GetActiveLicenseID() != -1;
        }

        public int GetActiveLicenseID()
        {  //this will get the license id that belongs to this application
            return clsLicenseBLL.GetActiveLicenseIDByPersonID(ApplicantPersonID, LicenseClassID);
        }

    }
}
