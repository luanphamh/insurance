using System;
using System.Text.Json.Serialization;

namespace api
{
    public class ProfileRequest
    {
        [JsonPropertyName("claim_case_id")]
        public string ClaimCaseId { get; set; }
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }
        [JsonPropertyName("hospital_name")]
        public string HospitalName { get; set; }
        [JsonPropertyName("admitted_at")]
        public DateTime AdmittedAt { get; set; }
        [JsonPropertyName("discharged_at")]
        public DateTime DischargedAt { get; set; }
    }
}