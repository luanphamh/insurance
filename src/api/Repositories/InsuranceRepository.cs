using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using api;
using api.Migrations;
using api.Models;

namespace Api
{
    public interface IInsuranceRepository: IGenericRepository<Insurance> 
    {
        Task AddOrUpdateProfilesAsync(IEnumerable<Insurance> entity);
        Task<IEnumerable<Insurance>> GetFraudProfiles(ScanFraudProfileRequest scanFraudProfileRequest);
        Task<IEnumerable<Insurance>> GetProfilesByUserName(string userName);
    }

    public class InsuranceRepository : GenericRepository<Insurance>, IInsuranceRepository
    {
        public InsuranceRepository(InsuranceContext context) : base(context)
        {

        }
        
        public async Task AddOrUpdateProfilesAsync(IEnumerable<Insurance> entities)
        {
            foreach (var entity in entities)
            {
                var existedInsurance = await GetByIdAsync(insuranceDb => insuranceDb.ClaimCaseId == entity.ClaimCaseId);
                
                if(existedInsurance != null)
                    _context.Entry(existedInsurance).CurrentValues.SetValues(entity);
                else
                    await AddAsync(entity);
            }
            
            await SaveAsync();
        }
        public async Task<IEnumerable<Insurance>> GetFraudProfiles(ScanFraudProfileRequest scanFraudProfileRequest)
        {
            var existedInsurance = await GetByIdAsync(
                insuranceDb => insuranceDb.ClaimCaseId == scanFraudProfileRequest.ClaimCaseId);
                
            if (existedInsurance == null)
                return null;
                
            if (string.IsNullOrEmpty(existedInsurance.HasFraud))
                return new List<Insurance>() {existedInsurance};
                
            return await GetAsync(insuranceDb => insuranceDb.HasFraud == existedInsurance.HasFraud);
        }
        
        public async Task<IEnumerable<Insurance>> GetProfilesByUserName(string userName)
        {
            return await GetAsync(insuranceDb => insuranceDb.UserName == userName);

        }
        
    }
    
}
