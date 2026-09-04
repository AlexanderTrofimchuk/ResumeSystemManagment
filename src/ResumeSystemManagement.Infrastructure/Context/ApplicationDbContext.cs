using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Infrastructure.IdentityEntities;

namespace ResumeSystemManagement.Infrastructure.Context;

public class ApplicationDbContext(IConfiguration config):IdentityDbContext<User, IdentityRole,string>
{
    private readonly IConfiguration _config = config;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(_config.GetConnectionString("DefaultConnection"), o => 
                o.MigrationsHistoryTable("__MigrationsHistory", "public"));
    }
    
    public virtual DbSet<Resume> Resumes { get; set; }
    public virtual DbSet<Position> Positions { get; set;}
    public virtual DbSet<UserProject> UserProjects { get; set; }
    public virtual DbSet<ResumeAttributeValue> ResumeAttributeValues { get; set; }
    public virtual DbSet<RecruterLike> RecruterLikes { get; set; }
    public virtual DbSet<ChatHistory> Histories { get; set; }
    public virtual DbSet<AttributeValueForList> AttributeValueForLists { get; set; }
    public virtual DbSet<AttributeType> AttributeTypes { get; set; }
    public virtual DbSet<AttributeLibrary> AttributeLibraries { get; set; }
    public virtual DbSet<AttributeFilter> AttributeFilters { get; set; }
    public virtual DbSet<AttributeCategory> AttributeCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(entity =>
        {
            entity.HasMany(p => p.Resumes)
                .WithOne()
                .HasForeignKey(d => d.UserId)
                .IsRequired();

            entity.HasMany(p => p.Projects)
                .WithOne()
                .HasForeignKey(d => d.UserId);

            entity.HasMany(p => p.RecruterLikes)
                .WithOne()
                .HasForeignKey(d => d.RecruterId)
                .IsRequired();

            entity.HasMany(p => p.ChatHistories)
                .WithOne()
                .HasForeignKey(d => d.SentBy)
                .IsRequired();
        });
        
        builder.Entity<Resume>(entity =>
        {
            entity.HasMany(p => p.RecruterLikes)
                .WithOne(d => d.Resume)
                .HasForeignKey(d => d.ResumeId)
                .IsRequired();

            entity.HasMany(p => p.Histories)
                .WithOne(d => d.Resume)
                .HasForeignKey(d => d.ResumeId)
                .IsRequired();

            entity.HasMany(p => p.Likes)
                .WithOne(d => d.Resume)
                .HasForeignKey(d => d.ResumeId)
                .IsRequired();
        });

        builder.Entity<Position>(entity =>
        {
            entity.HasMany(p => p.Resumes)
                .WithOne(d => d.Position)
                .HasForeignKey(d => d.PositionId)
                .IsRequired();

            entity.HasMany(p => p.AttributeFilters)
                .WithOne(d => d.Position)
                .HasForeignKey(d => d.Position)
                .IsRequired();
        });

        builder.Entity<AttributeLibrary>(entity =>
        {
            entity.HasMany(p => p.AttributeValueForLists)
                .WithOne(d => d.Attribute)
                .HasForeignKey(d => d.AttributeId)
                .IsRequired();

            entity.HasMany(p => p.AttributeFilters)
                .WithOne(d => d.Attribute)
                .HasForeignKey(d => d.AttributeId)
                .IsRequired();
            
            entity.HasMany(p => p.ResumeAttributeValues)
                .WithOne(d => d.Attribute)
                .HasForeignKey(d => d.AttributeId)
                .IsRequired();
        });

        builder.Entity<AttributeCategory>(entity =>
        {
            entity.HasMany(p => p.AttributeLibraries)
                .WithOne(d => d.AttributeCategory)
                .HasForeignKey(d => d.CategoryId)
                .IsRequired();
        });

        builder.Entity<AttributeType>(entity =>
        {
            entity.HasMany(p => p.AttributeLibraries)
                .WithOne(d => d.AttributeType)
                .HasForeignKey(d => d.TypeId)
                .IsRequired();
        });
    }
}