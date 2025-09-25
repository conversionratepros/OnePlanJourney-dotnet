using System.ComponentModel.DataAnnotations;

namespace OnePlanPetJourney.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public int leadId {get; set;}
        public string? DeliveryAddressLineOne { get; set; }
        public string? DeliveryAddressLineTwo { get; set; }
        public string? DeliveryCity { get; set; }
        public string? DeliveryPostalCode { get; set; }
        public string PhysicalAddressLineOne { get; set; }
        public string? PhysicalAddressLineTwo { get; set; }
        public string PhysicalCity { get; set; }
        public string PhysicalPostalCode { get; set; }
    }
}
