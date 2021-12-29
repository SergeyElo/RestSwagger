using Domain.Core.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Persistence.Configurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public AdminConfiguration()
        {
        }
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasData(
                new UserEntity
                {
                    Id = Guid.Parse("7ca013b4-7c38-4906-91c7-77642b1b067b"),
                    UserName = "a-saik@mail.ru",
                    NormalizedUserName = "a-saik@mail.ru".ToUpper(),
                    Email = "a-saik@mail.ru",
                    NormalizedEmail = "a-saik@mail.ru".ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAEPxdj2FfkrB4B07Ar+QpZ0dEeO+r+I6fJyU92/XpYgbXoxrlkJQYOtVtVf2yQytRcg==",
                    SecurityStamp = "VTL7MPAUUXL4VXTGYCIMMJ6L62NPOZEY",
                    LockoutEnabled = false
                });
        }
    }
}
