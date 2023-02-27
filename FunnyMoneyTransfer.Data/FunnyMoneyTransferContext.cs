//https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/?tabs=dotnet-core-cli

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FunnyMoneyTransfer.Data;

public partial class FunnyMoneyTransferContext : DbContext
{
    public FunnyMoneyTransferContext()
    {
    }

    public FunnyMoneyTransferContext(DbContextOptions<FunnyMoneyTransferContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; } //TODO: Account model has TransferAccountIdFromNavigations and TransferAccountIdToNavigations prop names - can these be 'friendlier'?

    public virtual DbSet<StartingBalance> StartingBalances { get; set; }

    public virtual DbSet<Transfer> Transfers { get; set; } //TODO: Transfer model has AccountIdFromNavigation and AccountIdToNavigation prop names - can these be 'friendlier'?

    //public virtual DbSet<TransferStatus> TransferStatuses { get; set; } //TODO: Remove commented out code

    //public virtual DbSet<TransferType> TransferTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionStringFromConfiguration());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("accounts");

            entity.HasIndex(e => e.UserId, "UC_accounts_user_id").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.Account)
                .HasForeignKey<Account>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_accounts_users");
        });

        modelBuilder.Entity<StartingBalance>(entity =>
        {
            entity.ToTable("starting_balances");

            entity.HasIndex(e => e.AccountId, "UC_account_id").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("amount");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");

            entity.HasOne(d => d.Account).WithOne(p => p.StartingBalance)
                .HasForeignKey<StartingBalance>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_starting_balances_accounts");
        });

        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.ToTable("transfers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccountIdFrom).HasColumnName("account_id_from");
            entity.Property(e => e.AccountIdTo).HasColumnName("account_id_to");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("amount");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.TransferStatusId).HasColumnName("transfer_status_id");
            entity.Property(e => e.TransferTypeId).HasColumnName("transfer_type_id");

            entity.HasOne(d => d.AccountIdFromNavigation).WithMany(p => p.TransferAccountIdFromNavigations)
                .HasForeignKey(d => d.AccountIdFrom)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_transfers_accounts_from");

            entity.HasOne(d => d.AccountIdToNavigation).WithMany(p => p.TransferAccountIdToNavigations)
                .HasForeignKey(d => d.AccountIdTo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_transfers_accounts_to");

            //entity.HasOne(d => d.TransferStatus).WithMany(p => p.Transfers)
            //    .HasForeignKey(d => d.TransferStatusId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_transfers_transfer_statuses");

            //entity.HasOne(d => d.TransferType).WithMany(p => p.Transfers)
            //    .HasForeignKey(d => d.TransferTypeId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_transfers_transfer_types");
        });

        // I changed transfer status from a class mapped to a db table to an enum
        //modelBuilder.Entity<TransferStatus>(entity =>
        //{
        //    entity.ToTable("transfer_statuses");

        //    entity.HasIndex(e => e.Description, "UC_transfer_statuses_transfer_status_description").IsUnique();

        //    entity.Property(e => e.Id).HasColumnName("id");
        //    entity.Property(e => e.Description)
        //        .HasMaxLength(10)
        //        .IsUnicode(false)
        //        .HasColumnName("description");
        //});

        // I changed transfer type from a class mapped to a db table to an enum
        //modelBuilder.Entity<TransferType>(entity =>
        //{
        //    entity.ToTable("transfer_types");

        //    entity.HasIndex(e => e.Description, "UC_transfer_types_transfer_type_description").IsUnique();

        //    entity.Property(e => e.Id).HasColumnName("id");
        //    entity.Property(e => e.Description)
        //        .HasMaxLength(10)
        //        .IsUnicode(false)
        //        .HasColumnName("description");
        //});

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "UC_users_username").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    private void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity => entity.HasKey(e => e.Id));
        modelBuilder.Entity<StartingBalance>(entity => entity.HasKey(e => e.Id));
        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable(t => t.HasCheckConstraint("CK_transfers_not_same_account", "(account_id_from<>account_id_to)"));
            entity.ToTable(t => t.HasCheckConstraint("CK_transfers_amount_gt_0", "(amount > 0)"));
            entity.Property(p => p.TransferType).HasConversion<int>();  // I changed transfer type from a class mapped to a db table to an enum
            entity.Property(p => p.TransferStatus).HasConversion<int>(); // I changed transfer status from a class mapped to a db table to an enum
        });
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired();
        });
    }

    private static string GetConnectionStringFromConfiguration()  //TODO: Get this into Startup
    {
        string currentDirectory = Environment.CurrentDirectory;
        string configFileName = "appsettings.json";
        string fullPathToConfigFile = Path.Combine(currentDirectory, @"..\..\..\..\FunnyMoneyTransfer.Data", configFileName);

        IConfigurationRoot builder = new ConfigurationBuilder()
            .AddJsonFile(fullPathToConfigFile, optional: false)
            .Build();

        return builder.GetConnectionString("Project") ?? string.Empty;
    }
}
