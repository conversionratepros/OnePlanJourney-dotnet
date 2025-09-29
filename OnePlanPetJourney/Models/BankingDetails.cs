using System;
using System.ComponentModel.DataAnnotations;

namespace OnePlanPetJourney.Models
{
    public class BankingDetails
    {
        [Key]
        public int Id { get; set; }
        public bool isPersonal { get; set; }
        public bool isPassport { get; set; }
        public string AccountPayersID { get; set; }
        public string AccountHolder { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string BranchCode { get; set; }
        public string BankAccountType { get; set; }
        public DateTime DebitDate { get; set; }
        public DateTime PolicyStart { get; set; }
        public DateTime? SecondStartDate { get; set; }
        public string HearAboutUs { get; set; }
        public string ConfirmPhoneNumber { get; set; }
        public bool ConsentConfirm { get; set; }

        public int leadId { get; set; }
    }
}
