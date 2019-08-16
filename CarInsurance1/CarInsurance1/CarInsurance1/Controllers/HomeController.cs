using System.Web.Mvc;
using CarInsurance1.Models;
using CarInsurance1.Controllers;


namespace CarInsurance1.Controllers
{
    public class HomeController : Controller
    {
        private object insuranceQuote;

        public ActionResult Index()
        {
            return View();
        }


        public int CalculateAge(System.DateTime dateOfBirth)
        {
            int age = 0;
            age = System.DateTime.Now.Year - dateOfBirth.Year;


            if (System.DateTime.Now.Month < dateOfBirth.Month || (System.DateTime.Now.Month == dateOfBirth.Month && System.DateTime.Now.Day < dateOfBirth.Day))
                age -= 1;

            return age;
        }



        [HttpPost]
        public ActionResult InsuranceQuote(string firstName, string lastName, string emailAddress, System.DateTime dateOfBirth, string carMake, string carModel, int carYear, int speedingTickets, bool dui = false, bool fullCoverage = false)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(carMake) || string.IsNullOrEmpty(carModel))
            {
                return View("Error");
            }
            else
            {
                using (InsuranceEntities db = new InsuranceEntities())
                {
                    decimal quote = 50;

                    var insuranceQuote = new Insuree();
                    insuranceQuote.FirstName = firstName;
                    insuranceQuote.LastName = lastName;
                    insuranceQuote.EmailAddress = emailAddress;
                    insuranceQuote.CarMake = carMake;
                    insuranceQuote.CarModel = carModel;
                    insuranceQuote.CarYear = carYear;
                    insuranceQuote.DUI = dui;
                    insuranceQuote.CoverageType = fullCoverage;
                    insuranceQuote.SpeedingTickets = speedingTickets;
                    insuranceQuote.DateOfBirth = dateOfBirth;
                    insuranceQuote.Quote = quote;



                  
                decimal duiPercentage = .25m;
                decimal addFullCoverage = .5m;
                int age = CalculateAge(dateOfBirth);




                if (age < 25)
                {
                    quote += 25;
                }

                else if (age < 18)
                {
                    quote += 100;
                }

                else if (age > 100)
                {
                    quote += 25;
                }


                if (carYear < 2000)
                {
                    quote += 25;
                }

                else if (carYear > 2015)
                {
                    quote += 25;
                }


                if (carMake == "Porsche")
                {
                    quote += 25;
                }

                else if (carMake == "Porsche" && carModel == "Carerra 911")
                {
                    quote += 25;
                }


                if (dui == true)
                {
                    quote *= duiPercentage;
                }

                if (speedingTickets > 0)
                {
                    quote += speedingTickets * 10;
                }

                if (fullCoverage == true)
                {
                    quote *= addFullCoverage;
                }


              
                
                    db.Insurees.Add(insuranceQuote);
                    db.SaveChanges();
                }



                return View("Success");
            }

        }
     }
}