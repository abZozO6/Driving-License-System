using DriverLicense_DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicense_BLL.DrivingExamBLL
{
    public class clsTestTypeBLL
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetDrivingTest = 3 };

        public enTestType ID { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public float Fees { set; get; }
        public clsTestTypeBLL()

        {
            ID = enTestType.VisionTest;
            Title = "";
            Description = "";
            Fees = 0;
            Mode = enMode.AddNew;

        }

        private clsTestTypeBLL(enTestType ID, string TestTypeTitle, string Description, float TestTypeFees)

        {
            this.ID = ID;
            Title = TestTypeTitle;
            this.Description = Description;

            Fees = TestTypeFees;
            Mode = enMode.Update;
        }

        private bool _AddNewTestType()
        {
            ID = (enTestType)
                clsTestType.AddNewTestType(
                    Title,
                    Description,
                    Fees);

            return (int)ID != -1;
        }

        private bool _UpdateTestType()
        {
            //call DataAccess Layer 

            return clsTestType.UpdateTestType((int)ID, Title, Description, Fees);
        }

        public static clsTestTypeBLL Find(enTestType TestTypeID)
        {
            string Title = "", Description = ""; float Fees = 0;

            if (clsTestType.GetTestTypeInfoByID((int)TestTypeID, ref Title, ref Description, ref Fees))

                return new clsTestTypeBLL(TestTypeID, Title, Description, Fees);
            else
                return null;

        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestType.GetAllTestTypes();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestType())
                    {

                        Mode = enMode.Update;
                        return true;
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
