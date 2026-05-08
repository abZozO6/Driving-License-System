using DriverLicense_DAL;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace DriverLicense_BLL.PeopleBLL
{
    public class clsPersonBLL
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName}{SecondName}{ThirdName}{LastName}";//Expression-bodied property
        public  string NationalNo {  get; set; }
        public DateTime DateOfBirth { get; set; }
        public short Gender {  get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int NationalityCountryID { set; get; }
        public clsCountryBLL CountryInfo { get; set; }

        private string _ImagePath;
        public string ImagePath { get; set; } = "";

        public clsPersonBLL()

        {
            PersonID = -1;
            FirstName = "";
            SecondName = "";
            ThirdName = "";
            LastName = "";
            DateOfBirth = DateTime.MinValue;
            Address = "";
            PhoneNumber = "";
            Email = "";
            NationalityCountryID = -1;
            ImagePath = "";

            Mode = enMode.AddNew;
        }

        private clsPersonBLL(clsPerson Person)

        {
            PersonID = Person.PersonID;
            FirstName = Person.FirstName;
            SecondName = Person.SecondName;
            ThirdName = Person.ThirdName;
            LastName = Person.LastName;
            NationalNo = Person.NationalNo;
            DateOfBirth = Person.DateOfBirth;
            Gender = Person.Gendor;
            Address = Person.Address;
            PhoneNumber = Person.Phone;
            Email = Person.Email;
            NationalityCountryID = Person.NationalityCountryID;
            ImagePath = Person.ImagePath ?? "";
            CountryInfo = clsCountryBLL.Find(NationalityCountryID);          
            Mode = enMode.Update;
        }

        private bool _AddNewPerson()
        {
            clsPerson person = new clsPerson();

            person.FirstName = FirstName;
            person.SecondName = SecondName;
            person.ThirdName = ThirdName;
            person.LastName = LastName;
            person.NationalNo = NationalNo;
            person.DateOfBirth = DateOfBirth;
            person.Gendor = Gender;
            person.Address = Address;
            person.Phone = PhoneNumber;
            person.Email = Email;
            person.NationalityCountryID = NationalityCountryID;
            person.ImagePath = ImagePath;

            PersonID = clsPersonData.AddNewPerson(person);

            return PersonID != -1;
        }

        private bool _UpdatePerson()
        {
            clsPerson person = new clsPerson();

            person.FirstName = FirstName;
            person.SecondName = SecondName;
            person.ThirdName = ThirdName;
            person.LastName = LastName;
            person.NationalNo = NationalNo;
            person.DateOfBirth = DateOfBirth;
            person.Gendor = Gender;
            person.Address = Address;
            person.Phone = PhoneNumber;
            person.Email = Email;
            person.NationalityCountryID = NationalityCountryID;
            person.ImagePath = ImagePath;

            return clsPersonData.UpdatePerson(person);



        }

        public static clsPersonBLL Find(int PersonID)
        {
            clsPerson person = clsPersonData.GetPersonInfoByID(PersonID);

            if (person != null)
                return new clsPersonBLL(person);
            else
                return null;
        }
        public static clsPersonBLL Find(string NationalNo)
        {

            clsPerson person = clsPersonData.GetPersonInfoByNationalNo(NationalNo);


            if (person != null)
                return new clsPersonBLL(person);
            else
                return null;

        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdatePerson();

            }

            return false;
        }

        public static DataTable GetAllPeople()
        {
            return clsPersonData.GetAllPeople();
        }

        public static bool DeletePerson(int ID)
        {
            return clsPersonData.DeletePerson(ID);
        }
        public static bool isPersonExist(int ID)
        {
            return clsPersonData.IsPersonExist(ID);
        }

        public static bool isPersonExist(string NationlNo)
        {
            return clsPersonData.IsPersonExist(NationlNo);
        }

    }
}
