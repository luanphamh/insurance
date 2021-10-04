using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api;
using api.Handler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InsuranceController : Controller
    {
        private readonly IInsurance _insurance;
        public InsuranceController(IInsuranceRepository insuranceRepository, IInsurance insurance)
        {
            _insurance = insurance;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _insurance.GetAll());
        }

        [HttpPost()]
        public async Task<IActionResult> AddOrUpdateProfile(ProfileRequest[] profilesRequest)
        {
            await _insurance.AddOrUpdateProfilesAsync(profilesRequest);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> FindFraudProfiles(ScanFraudProfileRequest scanFraudProfileRequest)
        {
            var response = await _insurance.FindFraudProfile(scanFraudProfileRequest);

            var len = response?.Count();
            return len switch
            {
                1 => Ok(new { msg = "Fraud Not Found" }),
                > 1 => Ok(new { msg = "Fraud Found", data = response }),
                _ => Ok(new { msg = "Not Found" })
            };
        }
    }
}
