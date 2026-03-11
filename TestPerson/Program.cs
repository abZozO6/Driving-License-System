using DriverLicense_DAL;
using System.Data.SqlClient;

clsPerson person = clsPersonData.GetPersonInfoByID(1);

if (person != null)
{
    Console.WriteLine("Person Found");
    Console.WriteLine("Name: " + person.FirstName + " " + person.LastName);
    Console.WriteLine("Phone: " + person.Phone);
    Console.WriteLine("Email: " + person.Email);
}
else
{
    Console.WriteLine("Person Not Found");
}