using DanskeBank.Domain.CompanyAggregate;
using DanskeBank.Infrastructure.TypeConversions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DanskeBank.Infrastructure.EntityConfigurations;

internal class CompanyEntityConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("companies", CompanyContext.DefaultSchema);
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Market);
        builder.Property(c => c.CompanyType);
        builder.Property(c => c.CompanyNumber).IsFixedLength().HasMaxLength(10);
        builder.Property(c => c.CompanyName).HasMaxLength(128);
        builder.OwnsMany(c => c.Schedule, b =>
        {
            b.WithOwner().HasForeignKey("CompanyId");
            b.Property(n => n.SendingDate).HasConversion<DateOnlyConverter, DateOnlyComparer>();
        });
    }
}
