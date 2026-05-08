using DriverLicense_DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicense_BLL.PeopleBLL
{
    public class clsCountryBLL
    {

        public int ID { set; get; }
        public string CountryName { set; get; }

        public clsCountryBLL()

        {
            ID = -1;
            CountryName = "";

        }

        private clsCountryBLL(int ID, string CountryName)

        {
            this.ID = ID;
            this.CountryName = CountryName;
        }


        public static clsCountryBLL Find(int ID)
        {
            string CountryName = "";

            if (clsCountryData.GetCountryInfoByID(ID, ref CountryName))

                return new clsCountryBLL(ID, CountryName);
            else
                return null;

        }

        public static clsCountryBLL Find(string CountryName)
        {

            int ID = -1;

            if (clsCountryData.GetCountryInfoByName(CountryName, ref ID))

                return new clsCountryBLL(ID, CountryName);
            else
                return null;

        }

        public static DataTable GetAllCountries()
        {
            return clsCountryData.GetAllCountries();

        }


    }
}
