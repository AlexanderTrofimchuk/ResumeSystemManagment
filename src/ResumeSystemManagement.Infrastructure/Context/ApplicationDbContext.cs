using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ResumeSystemManagement.Core.Entities;
using ResumeSystemManagement.Infrastructure.IdentityEntities;

namespace ResumeSystemManagement.Infrastructure.Context;

public class ApplicationDbContext(IConfiguration config):IdentityDbContext<AppUser, IdentityRole,string>
{
    private readonly IConfiguration _config = config;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(_config.GetConnectionString("DefaultConnection"), o => 
                o.MigrationsHistoryTable("__MigrationsHistory", "public"))
            .UseLoggerFactory(CreateLoggerFactory())
            .EnableSensitiveDataLogging();
    }
    
    private static ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });
    
    public virtual DbSet<Resume> Resumes { get; set; }
    public virtual DbSet<Position> Positions { get; set;}
    public virtual DbSet<UserProject> UserProjects { get; set; }
    public virtual DbSet<CandidateAttributeValue> ResumeAttributeValues { get; set; }
    public virtual DbSet<RecruiterLike> RecruiterLikes { get; set; }
    public virtual DbSet<ChatHistory> Histories { get; set; }
    public virtual DbSet<AttributeValueForList> AttributeValueForLists { get; set; }
    public virtual DbSet<AttributeType> AttributeTypes { get; set; }
    public virtual DbSet<AttributeLibrary> AttributeLibraries { get; set; }
    public virtual DbSet<AttributeFilter> AttributeFilters { get; set; }
    public virtual DbSet<CandidateAttributeValue> CandidateAttributeValues { get; set; }
    public virtual DbSet<AttributeCategory> AttributeCategories { get; set; }
    public virtual DbSet<PositionAttributeLibrary> PositionAttributeLibraries { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>(entity =>
        {
            entity.HasMany(p => p.Resumes)
                .WithOne()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(p => p.Projects)
                .WithOne()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(p => p.RecruiterLikes)
                .WithOne()
                .HasForeignKey(d => d.RecruiterId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(p => p.ChatHistories)
                .WithOne()
                .HasForeignKey(d => d.SentBy)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            
            entity.HasMany(p => p.CandidateAttributeValues)
                .WithOne()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });
        
        builder.Entity<Resume>(entity =>
        {
            entity.HasMany(p => p.Likes)
                .WithOne(d => d.Resume)
                .HasForeignKey(d => d.ResumeId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(p => p.Histories)
                .WithOne(d => d.Resume)
                .HasForeignKey(d => d.ResumeId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(p => p.Likes)
                .WithOne(d => d.Resume)
                .HasForeignKey(d => d.ResumeId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        builder.Entity<Position>(entity =>
        {
            entity.Property(e => e.Version)
                .IsRowVersion();
            
            entity.HasMany(p => p.Resumes)
                .WithOne(d => d.Position)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(p => p.AttributeFilters)
                .WithOne(d => d.Position)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        builder.Entity<AttributeLibrary>(entity =>
        {
            entity.Property(e => e.Version)
                .IsRowVersion();
            
            entity.HasMany(p => p.AttributeValueForLists)
                .WithOne(d => d.Attribute)
                .HasForeignKey(d => d.AttributeId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(p => p.AttributeFilters)
                .WithOne(d => d.Attribute)
                .HasForeignKey(d => d.AttributeId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            
            entity.HasMany(p => p.CandidateAttributeValues)
                .WithOne(d => d.Attribute)
                .HasForeignKey(d => d.AttributeId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        builder.Entity<PositionAttributeLibrary>(entity =>
        {
            entity.HasKey(e => new { e.PositionId, e.AttributeLibraryId});
            
            entity.HasOne(e => e.Position)
                .WithMany(e => e.PositionAttributeLibraries)
                .HasForeignKey(e => e.PositionId);

            entity.HasOne(e => e.AttributeLibrary)
                .WithMany(e => e.PositionAttributeLibraries)
                .HasForeignKey(e => e.AttributeLibraryId);
        });

        builder.Entity<AttributeCategory>(entity =>
        {
            entity.HasMany(p => p.AttributeLibraries)
                .WithOne(d => d.AttributeCategory)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        builder.Entity<AttributeType>(entity =>
        {
            entity.HasMany(p => p.AttributeLibraries)
                .WithOne(d => d.AttributeType)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });
    }
}