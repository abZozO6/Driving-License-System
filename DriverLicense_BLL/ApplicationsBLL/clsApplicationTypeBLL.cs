using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DriverLicense_DAL;


namespace DriverLicense_BLL.ApplicationsBLL
{
    public class clsApplicationTypeBLL
    {


        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public int ID { set; get; }
        public string Title { set; get; }
        public float Fees { set; get; }

        public clsApplicationTypeBLL()

        {
            ID = -1;
            Title = "";
            Fees = 0;
            Mode = enMode.AddNew;

        }

        private clsApplicationTypeBLL(int ID, string ApplicationTypeTitel, float ApplicationTypeFees)

        {
            this.ID = ID;
            Title = ApplicationTypeTitel;
            Fees = ApplicationTypeFees;
            Mode = enMode.Update;
        }

        private bool _AddNewApplicationType()
        {
            //call DataAccess Layer 

            ID = ApplicationType.AddNewApplicationType(Title, Fees);


            return ID != -1;
        }

        private bool _UpdateApplicationType()
        {
            //call DataAccess Layer 

            return ApplicationType.UpdateApplicationType(ID, Title, Fees);
        }

        public static clsApplicationTypeBLL Find(int ID)
        {
            string Title = ""; float Fees = 0;

            if (ApplicationType.GetApplicationTypeInfoByID(ID, ref Title, ref Fees))

                return new clsApplicationTypeBLL(ID, Title, Fees);
            else
                return null;

        }

        public static DataTable GetAllApplicationTypes()
        {
            return ApplicationType.GetAllApplicationTypes();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplicationType())
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
