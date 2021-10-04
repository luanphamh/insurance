using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api;
using Hangfire;

namespace api.Handler
{
    public class Insurance : IInsurance
    {
        private readonly IInsuranceRepository _insuranceRepository;
        public Insurance(IInsuranceRepository insuranceRepository)
        {
            _insuranceRepository = insuranceRepository;
        }

        public async Task<IEnumerable<Models.Insurance>> GetAll()
        {
            return await _insuranceRepository.GetAllAsync();
        }
        
        public async Task AddOrUpdateProfilesAsync(IEnumerable<ProfileRequest> profilesRequest)
        {
            if (!profilesRequest.Any())
                return;
            
            var profileRequestGroupByClaimCaseId = profilesRequest
                .GroupBy(profile => profile.ClaimCaseId);
            var profileRequestHandle = InsuranceHelper
                .DuplicateHandle(profileRequestGroupByClaimCaseId);

            if (profileRequestHandle.Any())
            {
                 _insuranceRepository.AddOrUpdateProfilesAsync( profileRequestHandle
                    .Select(profile => new Models.Insurance()
                    {
                        ClaimCaseId = profile.ClaimCaseId,
                        UserName = profile.UserName,
                        HospitalName = profile.HospitalName,
                        AdmittedAt = profile.AdmittedAt,
                        DischargedAt = profile.DischargedAt
                    })).GetAwaiter().GetResult();
                    
                var userNames = profileRequestHandle
                    .Select(profile => profile.UserName)
                    .ToArray();
                    
                BackgroundJob.Enqueue(() =>
                    ScanFraudProfilesByUserName(
                        userNames));
            }
        }
        
        public async Task ScanAllFraudProfile()
        {
            var insuranceDb = await _insuranceRepository.GetAllAsync();
            var insuranceDbGroupByUserName = insuranceDb.GroupBy(insurance => insurance.UserName);
            var tmpTicks = new List<string>();
            foreach (var insuranceGroup in insuranceDbGroupByUserName)
            {
                var tick = Guid.NewGuid().ToString();
                var insurances = insuranceGroup.ToList();
                if (tmpTicks.Contains(insurances?.First().HasFraud)) continue;
                
                InsuranceHelper.TickFraudProfile(insurances, tick);
                tmpTicks.Add(tick);

            }
        }

        public async Task ScanFraudProfilesByUserName(IEnumerable<string> userNames)
        {
            if (!userNames.Any())
                return;
            
            foreach (var userName in userNames)
            {
                await ScanFraudProfileByUserName(userName, Guid.NewGuid().ToString());
            }
        }
        
        public async Task ScanFraudProfileByUserName(string userName, string tick)
        {
            var insurancesDb = await _insuranceRepository.GetProfilesByUserName(userName);
            var insureances = InsuranceHelper.TickFraudProfile(insurancesDb, tick);
            
            if (insureances?.Count() >1)
                _insuranceRepository.UpdateRange(insureances);
        }
        
        public async Task<IEnumerable<string>> FindFraudProfile(ScanFraudProfileRequest scanFraudProfileRequest)
        {
            var insurancesDb = await _insuranceRepository.GetFraudProfiles(scanFraudProfileRequest);
            
            return insurancesDb?
                .Select(x=>x.ClaimCaseId).ToList();
        }
    }

    public interface IInsurance
    {
        Task<IEnumerable<Models.Insurance>> GetAll();
        Task AddOrUpdateProfilesAsync(IEnumerable<ProfileRequest> profilesRequest);
        Task<IEnumerable<string>> FindFraudProfile(ScanFraudProfileRequest scanFraudProfileRequest);
        Task ScanAllFraudProfile();
        Task ScanFraudProfilesByUserName(IEnumerable<string> userNames);
        Task ScanFraudProfileByUserName(string userName, string tick);
    } 
}
