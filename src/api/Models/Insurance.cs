using System;
using System.Text.Json.Serialization;

namespace api.Models
{
    public class Insurance 
    {
        public string ClaimCaseId { get; set; }
        public string UserName { get; set; }
        public string HospitalName { get; set; }
        public DateTime AdmittedAt { get; set; }
        public DateTime DischargedAt { get; set; }
        public string HasFraud { get; set; }
    }
}
