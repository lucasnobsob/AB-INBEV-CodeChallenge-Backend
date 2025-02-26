using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AB_INBEV.Domain.Models;

namespace AB_INBEV.Infra.Data.Mappings
{
    public class EmployeeMap : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(e => e.Id)
                .HasColumnName("Id");

            builder.Property(e => e.FirstName)
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.LastName)
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Email)
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Document)
                .HasColumnType("varchar(12)")
                .HasMaxLength(12)
                .IsRequired();

            builder.HasMany(e => e.Phones)
                .WithOne(p => p.Employee)
                .HasForeignKey(p => p.EmployeeId);

            builder.Property(e => e.BirthDate)
                .HasColumnType("datetime2")
                .IsRequired();

            // builder.HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);
            builder.HasQueryFilter(p => !p.IsDeleted);

            builder.ToTable("Employee");
        }
    }
}
