using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AB_INBEV.Domain.Models;

namespace AB_INBEV.Infra.Data.Mappings
{
    public class EmployeeMap : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(c => c.Id)
                .HasColumnName("Id");

            builder.Property(c => c.FirstName)
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.LastName)
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            // builder.HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);
            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
