using HappiAdventure.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HappiAdventure.Application.Db.Configurations;

public class PlaceConfiguration : IEntityTypeConfiguration<Place>
{
    public void Configure(EntityTypeBuilder<Place> builder)
    {
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.HasIndex(e => e.Name).IsUnique();
        builder.HasIndex(e => e.Location).HasMethod("GIST");

        builder.HasMany(e => e.Activities)
            .WithMany(e => e.Places)
            .UsingEntity(j => j.ToTable("PlaceActivities"));
    }
}
