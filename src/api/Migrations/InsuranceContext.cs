using System;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Migrations
{
    public sealed class InsuranceContext : DbContext
    {
        public InsuranceContext(DbContextOptions<InsuranceContext> options) : base(options)
        {
            Database.Migrate();
        }
        public DbSet<Insurance> Insurances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("papaya");
            
            var insuranceMap = new InsuranceMap(modelBuilder.Entity<Insurance>());
        }

        private class InsuranceMap
        {
            public InsuranceMap(EntityTypeBuilder<Insurance> entityBuilder)
            {
                entityBuilder.ToTable("insurance");
                
                entityBuilder.HasKey(x => x.ClaimCaseId);
                entityBuilder.HasIndex(x => x.ClaimCaseId).IsUnique();
                entityBuilder.HasIndex(x => x.UserName);
                

                entityBuilder.Property(x => x.ClaimCaseId).HasColumnName("claim_case_id");
                entityBuilder.Property(x => x.UserName).HasColumnName("user_name");
                entityBuilder.Property(x => x.HospitalName).HasColumnName("hospital_name");
                entityBuilder.Property(x => x.AdmittedAt).HasColumnName("admitted_at");
                entityBuilder.Property(x => x.DischargedAt).HasColumnName("discharged_at");
                entityBuilder.Property(x => x.HasFraud).HasColumnName("has_fraud");
            }
        }
    }
}
