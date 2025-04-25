using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using TaskTimeTracker.Entities;

namespace TaskTimeTracker.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class TaskTimeTrackerDbContext :
    AbpDbContext<TaskTimeTrackerDbContext>,
    ITenantManagementDbContext,
    IIdentityDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */
    public DbSet<Project> Projects { get; set; }
    public DbSet<Tasks> Tasks { get; set; }
    public DbSet<Time> TimeEntries { get; set; } // Added Time entity


    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public TaskTimeTrackerDbContext(DbContextOptions<TaskTimeTrackerDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureTenantManagement();
        builder.ConfigureBlobStoring();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(TaskTimeTrackerConsts.DbTablePrefix + "YourEntities", TaskTimeTrackerConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});


        // Configure Project entity
        builder.Entity<Project>(entity =>
        {
            entity.ToTable("Projects"); // Table name
            entity.HasKey(p => p.Id); // Primary key

            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(256); // Name column configuration

            entity.Property(p => p.Description)
                  .HasColumnType("text"); // Description column configuration

            entity.Property(p => p.StartDate)
                  .HasColumnType("timestamp without time zone"); // StartDate column configuration

            entity.Property(p => p.EndDate)
                  .HasColumnType("timestamp without time zone"); // EndDate column configuration

            entity.Property(p => p.Status)
                  .HasMaxLength(50); // Status column configuration

            entity.HasOne(p => p.User)
                  .WithMany()
                  .HasForeignKey(p => p.UserId)
                  .OnDelete(DeleteBehavior.Restrict); // Foreign key to User
        });

        // Configure Tasks entity
        builder.Entity<Tasks>(entity =>
        {
            entity.ToTable("Tasks"); // Table name
            entity.HasKey(t => t.Id); // Primary key

            entity.Property(t => t.Title)
                  .IsRequired()
                  .HasMaxLength(256); // Title column configuration

            entity.Property(t => t.Description)
                  .HasColumnType("text"); // Description column configuration

            entity.Property(t => t.DueDate)
                  .HasColumnType("timestamp without time zone"); // DueDate column configuration

            entity.Property(t => t.Status)
                  .HasConversion<string>() // Store enum as string
                  .IsRequired();

            entity.Property(t => t.Priority)
                  .HasConversion<string>() // Store enum as string
                  .IsRequired();

            entity.HasOne(t => t.Project)
                  .WithMany()
                  .HasForeignKey(t => t.ProjectId)
                  .OnDelete(DeleteBehavior.Restrict); // Foreign key to Project

            entity.HasOne(t => t.User)
                  .WithMany()
                  .HasForeignKey(t => t.UserId)
                  .OnDelete(DeleteBehavior.Restrict); // Foreign key to User
        });

        // Configure Time entity
        builder.Entity<Time>(entity =>
        {
            entity.ToTable("TimeEntries"); // Table name
            entity.HasKey(t => t.Id); // Primary key

            entity.Property(t => t.Date)
                  .HasColumnType("timestamp without time zone")
                  .IsRequired(); // Date column configuration

            entity.Property(t => t.Hours)
                  .IsRequired(); // Hours column configuration

            entity.Property(t => t.Notes)
                  .HasColumnType("text"); // Notes column configuration

            entity.HasOne(t => t.Tasks)
                  .WithMany()
                  .HasForeignKey(t => t.TasksId)
                  .OnDelete(DeleteBehavior.Restrict); // Foreign key to Tasks

            entity.HasOne(t => t.User)
                  .WithMany()
                  .HasForeignKey(t => t.UserId)
                  .OnDelete(DeleteBehavior.Restrict); // Foreign key to User
        });

    }
}
