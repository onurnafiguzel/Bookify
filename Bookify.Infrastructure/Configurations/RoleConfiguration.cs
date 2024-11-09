using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
	public void Configure(EntityTypeBuilder<Role> builder)
	{
		builder.ToTable("roles");

		builder.HasKey(x => x.Id);

		builder.HasMany(x => x.Users)
			.WithMany(y => y.Roles);

		builder.HasData(Role.Registered);
	}
}
