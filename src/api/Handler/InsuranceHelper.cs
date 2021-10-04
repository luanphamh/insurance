using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Handler
{
    public static class InsuranceHelper
    {
        public static IEnumerable<T> DuplicateHandle<T>(
            IEnumerable<IGrouping<string, T>> groups)
        {
            foreach (var itemGroup in groups)
            {
                if (itemGroup == null)
                    continue;
                
                if (itemGroup.Count() > 1)
                {
                    yield return itemGroup.Last();
                    continue;
                }
                yield return itemGroup.First();
            }
        }
        
        public static IEnumerable<Models.Insurance> ScanPeriodsOverlap(this IEnumerable<Models.Insurance> insurances, int length)
        {
            var insurancesDb = insurances as Models.Insurance[] ?? insurances.ToArray();
            var insurancesOverlap = new List<Models.Insurance>();
            for (var i = 0; i < length -1; i++)
            {
                for (var j = i + 1; j < length; j++)
                {
                    if (!IsOverlapped(insurancesDb[i], insurancesDb[j])) continue;

                    if (!insurancesOverlap.Contains(insurancesDb[i]))
                    {
                        insurancesOverlap.Add(insurancesDb[i]);
                    }
                    
                    if (!insurancesOverlap.Contains(insurancesDb[j]))
                    {
                        insurancesOverlap.Add(insurancesDb[j]);
                    }
                }
            }

            return insurancesOverlap;
        }

        private static bool IsOverlapped(Models.Insurance insurance1, Models.Insurance insurance2)
        {
            return  insurance1.AdmittedAt < insurance2.DischargedAt && insurance2.AdmittedAt < insurance1.DischargedAt;
        }

        public static IEnumerable<Models.Insurance> TickFraudProfile(IEnumerable<Models.Insurance> insurances, string tick)
        {
            var fraudInsurances = new List<Models.Insurance>();
            if (!insurances.Any())
                return fraudInsurances;
            
            var insurancesDbLen = insurances.Count();

            if (insurancesDbLen == 1)
            {
                var insurance = insurances.First();
                insurance.HasFraud = tick;
                fraudInsurances.Add(insurance);
            }
            else
            {
                var insurancesOverlap = insurances.ScanPeriodsOverlap(insurancesDbLen);
                insurancesOverlap.ToList().ForEach(insurance=>insurance.HasFraud = tick);
                fraudInsurances.AddRange(insurancesOverlap);
            }

            return fraudInsurances;
        }
    }
}