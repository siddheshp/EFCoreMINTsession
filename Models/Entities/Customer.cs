using System.ComponentModel.DataAnnotations;

namespace EFCoreConApp.Models.Entities
{
    public class Customer
    {
        public string CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        [ConcurrencyCheck]
        public string City { get; set; }
        public string Country { get; set; }

        public override string ToString()
        {
            return $"Customer ID: {CustomerID}, Company Name: {CompanyName}, ContactName: {ContactName}, City: {City}, Country: {Country} ";
        }
    }
}