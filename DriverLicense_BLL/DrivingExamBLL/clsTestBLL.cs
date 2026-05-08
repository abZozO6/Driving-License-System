using DriverLicense_DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicense_BLL.DrivingExamBLL
{
    public class clsTestBLL
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int TestID { set; get; }
        public int TestAppointmentID { set; get; }

        private clsTestAppointmentBLL _TestAppointmentInfo;
        public clsTestAppointmentBLL TestAppointmentInfo
        {
            get
            {
                if (_TestAppointmentInfo == null)
                {
                    _TestAppointmentInfo =
                        clsTestAppointmentBLL.Find(TestAppointmentID);
                }

                return _TestAppointmentInfo;
            }
        }

        public bool TestResult { set; get; }

        private string _Notes = "";
        public string Notes
        {
            get { return _Notes; }

            set
            {
                _Notes = value ?? "";
            }
        }  //to avoid NullReferenceException

        public int CreatedByUserID { set; get; }
        public clsTestBLL()

        {
            TestID = -1;
            TestAppointmentID = -1;
            TestResult = false;
            Notes = "";
            CreatedByUserID = -1;

            Mode = enMode.AddNew;

        }

        private clsTestBLL(int TestID, int TestAppointmentID,
            bool TestResult, string Notes, int CreatedByUserID)

        {
            this.TestID = TestID;
            this.TestAppointmentID = TestAppointmentID;
            this.TestResult = TestResult;
            this.Notes = Notes;
            this.CreatedByUserID = CreatedByUserID;

            Mode = enMode.Update;
        }


        private bool _AddNewTest()
        {

            TestID = clsTest.AddNewTest(TestAppointmentID,
                TestResult, Notes, CreatedByUserID);


            return TestID != -1;
        }

        private bool _UpdateTest()
        {

            return clsTest.UpdateTest(TestID, TestAppointmentID,
                TestResult, Notes, CreatedByUserID);
        }

        public static clsTestBLL Find(int TestID)
        {
            int TestAppointmentID = -1;
            bool TestResult = false; string Notes = ""; int CreatedByUserID = -1;

            if (clsTest.GetTestInfoByID(TestID,
            ref TestAppointmentID, ref TestResult,
            ref Notes, ref CreatedByUserID))

                return new clsTestBLL(TestID,
                        TestAppointmentID, TestResult,
                        Notes, CreatedByUserID);
            else
                return null;

        }

        public static clsTestBLL GetLastTest( int personID, int licenseClassID, clsTestTypeBLL.enTestType testTypeID)
        {
            int testID = -1;
            int testAppointmentID = -1;
            bool testResult = false;
            string notes = null;
            int createdByUserID = -1;

            if (!clsTest.GetLastTestByPersonAndTestTypeAndLicenseClass(personID,  licenseClassID, (int)testTypeID,ref testID,
                 ref testAppointmentID, ref testResult,  ref notes,   ref createdByUserID))
            {
                return null;
            }

            return new clsTestBLL( testID,testAppointmentID,testResult,notes,createdByUserID);
        }

        public static DataTable GetAllTests()
        {
            return clsTest.GetAllTests();

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
            return clsTest.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }

        private const byte TotalRequiredTests = 3;

        public static bool PassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            //if total passed test less than 3 it will return false otherwise will return true
            return GetPassedTestCount(LocalDrivingLicenseApplicationID) == TotalRequiredTests;
        }


    }
}
