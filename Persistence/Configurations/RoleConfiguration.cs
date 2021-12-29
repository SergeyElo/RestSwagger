using Common.Identity;
using Domain.Core.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.HasData(
            new RoleEntity
            {
                Id = Guid.Parse("cb027da7-62e3-4678-859b-ffdb92664c78"),
                Name = IdentityRoles.User,
                NormalizedName = IdentityRoles.User.ToUpper()
            },
            new RoleEntity
            {
                Id = Guid.Parse("a3ee775e-6333-4366-9ab4-28ba5b20a2ba"),
                Name = IdentityRoles.Admin,
                NormalizedName = IdentityRoles.Admin.ToUpper()
            });
        }
    }
}
