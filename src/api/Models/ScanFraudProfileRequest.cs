using System;
using System.Text.Json.Serialization;

namespace api
{
    public class ScanFraudProfileRequest
    {
        [JsonPropertyName("claim_case_id")]
        public string ClaimCaseId { get; set; }
    }
}