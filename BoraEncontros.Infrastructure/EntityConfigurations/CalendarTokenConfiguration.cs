using BoraEncontros.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BoraEncontros.Infrastructure.EntityConfigurations;

public class CalendarTokenConfiguration : IEntityTypeConfiguration<CalendarToken>
{
    public void Configure(EntityTypeBuilder<CalendarToken> builder)
    {
        builder.HasKey(o => o.Id);
    }
}
